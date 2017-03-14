using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Excel;
using Hidistro.ControlPanel.Function;
using Hidistro.Entities;
using Hidistro.SaleSystem.Vshop;
using Hidistro.SqlDal.Commodities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_sales_ExcelOrderPrint : AdminPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.btnUnPack.Click += new EventHandler(this.UnPackOrderInfos);
        this.btDS.Click += new EventHandler(this.excelDS);
        this.btDS.Enabled = false;
        if (!Page.IsPostBack)
        {
            DataTable dtChannelList = DistributorGradeBrower.GetChannelList();
            foreach (DataRow row in dtChannelList.Rows)
            {
                ListItem item = new ListItem();
                item.Value = row["id"].ToString();
                item.Text = row["ChannelName"].ToString();
                ddlChannelList.Items.Add(item);
            }
        }
    }
    private DataSet dtOrder;
    public void UnPackOrderInfos(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(fileUpload.FileName) && fileUpload.FileContent.Length > 0 && !string.IsNullOrEmpty(ddlChannelList.SelectedValue))
        {
            #region  导入模板格式及表头判断
            string filePath = "";
            try
            {
                filePath = MapPath("/Storage/temp/") + "yyyyMMddHHmmss" + fileUpload.FileName;
                fileUpload.SaveAs(filePath);
            }
            catch (Exception ex)
            {
                this.ShowMsg("出错了：" + ex.Message, false);
            }
            if (Path.GetExtension(filePath).ToLower() != ".xls")
            {
                this.ShowMsg("导入的文件不是xls文件，请检查文件格式。", false);
                return;
            }
            ExcelDBClass excelDBClass = new ExcelDBClass(filePath, true);
            //Excel转为DataTable
            DataTable dtOrderInfoExcel = excelDBClass.ExportToDataSet().Tables[0];
            //表头格式验证
            string strHeaders = CustomConfigHelper.Instance.IsOrderLeading();
            string[] arrayHeader = strHeaders.Split(',');
            for (int i = 0; i < dtOrderInfoExcel.Columns.Count; i++)
            {
                if (dtOrderInfoExcel.Columns[i].ColumnName != arrayHeader[i])
                {
                    this.ShowMsg("导入的文件格式错误,请检查文件。", false);
                    return;
                }
            }
            //删除（dtOrderInfo）空白行
            DataRow[] blank = dtOrderInfoExcel.Select("商品编号 is null");
            for (int i = 0; i < blank.Count(); i++)
            {
                blank[i].Delete();
            }
            dtOrderInfoExcel.AcceptChanges();
            #endregion
            //新增列
            dtOrderInfoExcel.Columns.Add("errorInfo", typeof(string));
            dtOrderInfoExcel.Columns["errorInfo"].DefaultValue = "";
            dtOrderInfoExcel.Columns.Add("errorFields", typeof(string));
            dtOrderInfoExcel.Columns["errorFields"].DefaultValue = ",";
            //查询订单表，订单详情表，商品库存表
            string ordeInfo = "select*from Hishop_Orders;select*from Hishop_OrderItems;select*from Hishop_SKUs";
            DataSet dtOrderOrigin = DataBaseHelper.GetDataSet(ordeInfo);
            dtOrder = dtOrderOrigin.Clone();
            bool productId = true;
            bool errorInfo = true;
            DataRow drStock=null;
            foreach (DataRow dr in dtOrderInfoExcel.Rows)
            {
                DataRow newOrder = dtOrder.Tables[0].NewRow();
                DataRow newOederItem = dtOrder.Tables[1].NewRow();
                #region 数据验证（数据格式和重复性）
                    for (int s = 0; s < dtOrderOrigin.Tables[0].Rows.Count; s++)
                    {
                        if (dr["原始订单编号"].ToString() == dtOrderOrigin.Tables[0].Rows[s]["Sender"].ToString())
                        {
                            dr["errorInfo"] = dr["errorInfo"] + "原始订单编号已存在！";
                            dr["errorFields"] = dr["errorFields"] + "," + "原始订单编号";
                            errorInfo = false;
                        }
                    }
                    if (string.IsNullOrEmpty(dr["商品编号"].ToString()) || dataValidation(dr["商品编号"].ToString()) == "")
                    {
                        dr["errorInfo"] = dr["errorInfo"] + "商品编号不能为空或商品编号不存在！";
                        dr["errorFields"] = dr["errorFields"] + "," + "商品编号";
                        productId = false;
                        errorInfo = false;
                    }
                    else
                    {
                        string[] stOrderSku = dataValidation(dr["商品编号"].ToString()).Split(',');
                        newOederItem["ProductId"] = stOrderSku.GetValue(0).ToString();
                        newOederItem["SkuId"] = stOrderSku.GetValue(1).ToString();
                        //newOederItem["CostPrice"] = stOrderSku.GetValue(2).ToString();
                        //newOederItem["ItemListPrice"] = stOrderSku.GetValue(3).ToString();
                        //newOederItem["ItemDescription"] = stOrderSku.GetValue(4).ToString();
                        //newOederItem["Weight"] = stOrderSku.GetValue(2).ToString();
                    }
                if (!productId || string.IsNullOrEmpty(dr["商品数量"].ToString()) || !PageValidateHelper.IsNumber(dr["商品数量"].ToString()) || productStock(dr["商品编号"].ToString(), dr["商品数量"].ToString())==null)
                {
                    dr["errorInfo"] = dr["errorInfo"] + "商品数量不合法或库存不足！";
                    dr["errorFields"] = dr["errorFields"] + "," + "商品数量";
                    errorInfo = false;
                }
                else
                {
                    drStock = productStock(dr["商品编号"].ToString(), dr["商品数量"].ToString());
                }
                if (string.IsNullOrEmpty(dr["成本价"].ToString()) || !PageValidateHelper.IsDecimal(dr["成本价"].ToString()))
                {
                    dr["errorInfo"]= dr["errorInfo"] + "成本价为小数型";
                    dr["errorFields"] = dr["errorFields"] + "," + "成本价";
                    errorInfo = false;
                }
                if (string.IsNullOrEmpty(dr["商品单价"].ToString()) || !PageValidateHelper.IsDecimal(dr["商品单价"].ToString()))
                {
                    dr["errorInfo"]= dr["errorInfo"] + "商品单价为小数型！";
                    dr["errorFields"] = dr["errorFields"] + "," + "商品单价";
                    errorInfo = false;
                }
                //if (string.IsNullOrEmpty(dr["商品重量"].ToString()) || !PageValidateHelper.IsNumber(dr["商品重量"].ToString()))
                //{
                //    dr["errorInfo"]= dr["errorInfo"] + "商品重量为整数型！";
                //    dr["errorFields"] = dr["errorFields"] + "商品重量";
                //    errorInfo = false;
                //}
                if (string.IsNullOrEmpty(dr["订单产生时间"].ToString()) || !PageValidateHelper.IsDate(dr["订单产生时间"].ToString()))
                {
                    dr["errorInfo"]= dr["errorInfo"] + "订单产生时间格式错误！";
                    dr["errorFields"] = dr["errorFields"] + "," + "订单产生时间";
                    errorInfo = false;
                }
                if (string.IsNullOrEmpty(dr["发货时间"].ToString()) || !PageValidateHelper.IsDate(dr["发货时间"].ToString()))
                {
                    dr["errorInfo"]= dr["errorInfo"] + "发货时间格式错误！";
                    dr["errorFields"] = dr["errorFields"] + "," + "发货时间";
                    errorInfo = false;
                }
                if (string.IsNullOrEmpty(dr["收货人姓名"].ToString()))
                {
                    dr["errorInfo"] = dr["errorInfo"] + "收货人姓名不能为空！";
                    dr["errorFields"] = dr["errorFields"] + "," + "收货人姓名";
                    errorInfo = false;
                }
                if (string.IsNullOrEmpty(dr["收货人手机"].ToString()) || !PageValidateHelper.IsPhone(dr["收货人手机"].ToString().Trim()))
                {
                    dr["errorInfo"] = dr["errorInfo"] + "收货人手机不能为空或者格式错误！";
                    dr["errorFields"] = dr["errorFields"] + "," + "收货人手机";
                    errorInfo = false;
                }
                if (string.IsNullOrEmpty(dr["物流公司"].ToString()) || dr["物流公司"].ToString() != "圆通速递"&&dr["物流公司"].ToString() != "韵达快运")
                {
                    dr["errorInfo"] = dr["errorInfo"] + "物流公司不能为空或物流不存在！";
                    dr["errorFields"] = dr["errorFields"] + "," + "物流公司";
                    errorInfo = false;
                }
                if (string.IsNullOrEmpty(dr["送货地区"].ToString())||(dr["送货地区"].ToString().IndexOf("省")<0&&dr["送货地区"].ToString().IndexOf("市")<0))
                {   
                    dr["errorInfo"] = dr["errorInfo"] + "送货地区不能为空格式为**省**市**！";
                    dr["errorFields"] = dr["errorFields"] + "," + "送货地区";
                    errorInfo = false;
                }
                if (string.IsNullOrEmpty(dr["详细地址"].ToString()))
                {
                    dr["errorInfo"] = dr["errorInfo"] + "详细地址不能为空！";
                    dr["errorFields"] = dr["errorFields"] + "," + "详细地址";
                    errorInfo = false;
                }
                if (string.IsNullOrEmpty(dr["备注"].ToString()))
                {
                    dr["errorInfo"] = dr["errorInfo"] + "备注不能为空！";
                    dr["errorFields"] = dr["errorFields"] + "," + "备注";
                    errorInfo = false;
                }
                #endregion
                    if (errorInfo)
                    {
                        #region 创建新表
                        newOrder["OrderId"] = CustomConfigHelper.Instance.GenerateOrderIdByOriginOrderid(dr["原始订单编号"].ToString());
                        newOrder["OrderDate"] = dr["订单产生时间"];
                        newOrder["PayDate"] = dr["付款时间"];
                        newOrder["ShippingDate"] = dr["发货时间"];
                        newOrder["FinishDate"] = dr["收货时间"];
                        newOrder["ManagerRemark"] = dr["备注"];
                        newOrder["ShippingRegion"] = dr["送货地区"];
                        newOrder["Address"] = dr["详细地址"];
                        newOrder["ExpressCompanyName"] = dr["物流公司"];
                        newOrder["CellPhone"] = dr["收货人手机"];
                        newOrder["ShipTo"] = dr["收货人姓名"];
                        //数据库不能为空字段
                        newOrder["UserId"] = 0;
                        newOrder["Username"] = 0;
                        newOrder["sender"] = dr["原始订单编号"];
                        newOrder["OrderStatus"] = 3;//默认为3（已发货）
                        newOrder["ChannelId"] = new Guid(ddlChannelList.SelectedValue);//渠道商id]
                        if (dr["物流公司"].ToString()== "圆通速递")
                        {
                            newOrder["ExpressCompanyAbb"] = "yuantong";
                        }
                        else
                        {
                            newOrder["ExpressCompanyAbb"] = "yunda";
                        }
                        newOederItem["OrderId"] = newOrder["OrderId"];
                        newOederItem["Quantity"] = dr["商品数量"];
                        newOederItem["CostPrice"] = dr["成本价"];
                        newOederItem["ItemListPrice"] = dr["商品单价"];
                        newOederItem["ItemDescription"] = dr["商品描述"];
                        //newOederItem["Weight"] = dr["商品重量"];
                        newOederItem["OrderItemsStatus"] = 3;
                        //数据库不能为null字段
                        newOederItem["ShipmentQuantity"] = dr["商品数量"];
                        newOederItem["ItemAdjustedPrice"] = 0;
                        newOederItem["ItemAdjustedCommssion"] = 0;
                        #endregion
                    }
                dtOrder.Tables[0].Rows.Add(newOrder);
                dtOrder.Tables[1].Rows.Add(newOederItem);
                if (drStock != null)
                {
                    #region 商品库存减少
                    drStock.Table.PrimaryKey = new DataColumn[] { drStock.Table.Columns["SkuId"] };
                    dtOrder.Tables[2].PrimaryKey = new DataColumn[] { dtOrder.Tables[2].Columns["SkuId"] };
                    DataRow drOrderSkusSave = dtOrder.Tables[2].Rows.Find(drStock["SkuId"]);
                    if (drOrderSkusSave == null)
                    {
                        drOrderSkusSave = dtOrder.Tables[2].Rows.Add(drStock.ItemArray);
                        drOrderSkusSave.AcceptChanges();
                    }
                    drOrderSkusSave["Stock"]=int.Parse(drOrderSkusSave["Stock"].ToString())-int.Parse(dr["商品数量"].ToString());
                    if (int.Parse(drOrderSkusSave["Stock"].ToString()) < 0)
                    {
                        dr["errorInfo"] = "库存不足！";
                        dr["errorFields"] = dr["errorFields"] + "," + "商品数量";
                    }
                    #endregion
                }
                dr["errorInfo"].ToString();
            }
            #region 原始订单和商品单号完全相同判断
            for (int i = 0; i < dtOrderInfoExcel.Rows.Count; i++)
            {
                for (int a = 0; a < dtOrderInfoExcel.Rows.Count; a++)
                {
                    if (i != a)
                    {
                        if (dtOrderInfoExcel.Rows[i]["原始订单编号"].ToString() == dtOrderInfoExcel.Rows[a]["原始订单编号"].ToString() && dtOrderInfoExcel.Rows[i]["商品编号"].ToString() == dtOrderInfoExcel.Rows[a]["商品编号"].ToString())
                        {
                            dtOrderInfoExcel.Rows[i]["errorInfo"] = dtOrderInfoExcel.Rows[i]["errorInfo"] + "数据重复！";
                            dtOrderInfoExcel.Rows[i]["errorFields"] = dtOrderInfoExcel.Rows[i]["errorFields"] + "," + "原始订单编号" + "," + "商品编号";
                            errorInfo = false;
                        }
                    }
                }
            }
            #endregion
            if (dtOrderInfoExcel.Select("errorInfo<>''").Count() > 0)
            {
                this.ShowMsg("验证未通过!", false);
            }
            else
            {
                this.ShowMsg("验证通过!", true);
                //验证通过后，将excel的值放入viewstate中，用于导入时的整表提交对比差异
                ViewState["dtOrder"] = dtOrder;
                this.btDS.Enabled =true;
            }
            //数据绑定(repeater)
            this.repeateExcel.DataSource = dtOrderInfoExcel;
            this.repeateExcel.DataBind();
        }
        else
        {
            this.ShowMsg("Excel文件未导入或渠道商未选择！", false);
        }
    }
    /// <summary>
    /// Excel模版下载
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExcelDownLoad_Click(object sender, EventArgs e)
    {
        string fileName = "Excel模板.xls";//客户端保存的文件名
        string filePath = Server.MapPath("../../Storage/Templates/订单导入模板.xls");//路径
        FileInfo fileInfo = new FileInfo(filePath);
        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
        Response.AddHeader("Content-Length", fileInfo.Length.ToString());
        Response.AddHeader("Content-Transfer-Encoding", "binary");
        Response.ContentType = "application/octet-stream";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
        Response.WriteFile(fileInfo.FullName);
        Response.Flush();
        Response.End();
    }
    /// <summary>
    /// 商品编号验证
    /// </summary>
    /// <param name="productID"></param>
    /// <returns></returns>
    protected string dataValidation(string productID)
    {
        if (productID != null && productID != "")
        {
            string commodityCode = "SELECT*FROM hishop_ProductsList where CommodityCode='"+productID+"'";
            DataSet dtCommodityCode= DataBaseHelper.GetDataSet(commodityCode);
            if (dtCommodityCode.Tables[0].Rows.Count > 0)
            {
                string commodityID = dtCommodityCode.Tables[0].Rows[0]["CommodityID"].ToString();
                //获取并返回商品信息,字段包括:productid,skuid,costprice,saleprice,productname,weight
                string roderNews = "select HS.*,HP.ProductName from Hishop_SKUs HS right join Hishop_Products HP on hs.ProductId=hp.ProductId where HP.ProductId=" + commodityID + "";
                DataSet dtOrderProductId = DataBaseHelper.GetDataSet(roderNews);
                if (dtOrderProductId.Tables[0].Rows.Count > 0)
                {   
                    string returnNumber=dtOrderProductId.Tables[0].Rows[0]["ProductId"].ToString()+","+dtOrderProductId.Tables[0].Rows[0]["SkuId"].ToString();
                    returnNumber += "," +((decimal)dtOrderProductId.Tables[0].Rows[0]["weight"]).ToString("F2");
                    return returnNumber;
                }
            }
        }
        return "";
    }

    /// <summary>
    /// 商品数量验证
    /// </summary>
    /// <param name="productID"></param>
    /// <param name="Stock"></param>
    /// <returns></returns>
    protected DataRow productStock(string productID, string Stock)
    {
        if (productID != "" && Stock != "")
        {
            string commodityCode = "SELECT*FROM hishop_ProductsList where CommodityCode='"+ productID +"'";
            DataSet dtCommodityCode = DataBaseHelper.GetDataSet(commodityCode);
            if (dtCommodityCode.Tables[0].Rows.Count > 0)
            {
                string commodityID = dtCommodityCode.Tables[0].Rows[0]["CommodityID"].ToString();
                string roderNews = "select*from Hishop_SKUs where ProductId=" + commodityID + "";
                DataSet dtOrderProductId = DataBaseHelper.GetDataSet(roderNews);
                if (dtOrderProductId.Tables[0].Rows.Count > 0)
                {
                    dtOrderProductId.Tables[0].PrimaryKey = new DataColumn[] { dtOrderProductId.Tables[0].Columns["SkuId"] };
                    int i = int.Parse(dtOrderProductId.Tables[0].Rows[0]["Stock"].ToString());
                    int a = int.Parse(Stock);
                    if (i - a > 0)
                    {
                        string skuId = dtOrderProductId.Tables[0].Rows[0]["SkuId"].ToString();
                        DataRow drStock = dtOrderProductId.Tables[0].Rows.Find(skuId);
                        return drStock;
                    }
                }
            }
        }
      return null;
    }
    /// <summary>
    /// 数据同步数据库
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void excelDS(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["dtOrder"] != null)
            {
                dtOrder = (DataSet)ViewState["dtOrder"];
            }
            DeleteSameRow(dtOrder.Tables[0], "Sender");
            string sqls = "select*from Hishop_Orders;select*from Hishop_OrderItems;select*from Hishop_SKUs";
            string[] sql = sqls.Split(';');
            int count = DataBaseHelper.CommitDataSet(dtOrder, sql);
            if (count > 0)
            {
                this.ShowMsg("导入成功！", true);
            }
            else
            {
                this.ShowMsg("数据同步失败！", false);
            }
        }
        catch (Exception ex)
        {
            this.ShowMsg("出错了：" + ex.Message, false);
        }
        finally
        {
            ViewState["dtOrder"] = null;
        }
    }
    /// <summary>
    /// 原始订单编号相同 则删除多余数据
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="Field"></param>
    /// <returns></returns>
    public static DataTable DeleteSameRow(DataTable dt, string Field)
    {
        ArrayList indexList = new ArrayList();
        // 找出待删除的行索引   
        for (int i = 0; i < dt.Rows.Count - 1; i++)
        {
            if (!IsContain(indexList, i))
            {
                for (int j = i + 1; j < dt.Rows.Count; j++)
                {
                    if (dt.Rows[i][Field].ToString() == dt.Rows[j][Field].ToString())
                    {
                        indexList.Add(j);
                    }
                }
            }
        }
        // 根据待删除索引列表删除行   
        for (int i = indexList.Count - 1; i >= 0; i--)
        {
            int index = Convert.ToInt32(indexList[i]);
            dt.Rows.RemoveAt(index);
        }
        return dt;
    }
    /// <summary>   
    /// 判断数组中是否存在   
    /// </summary>   
    /// <param name="indexList">数组</param>   
    /// <param name="index">索引</param>   
    /// <returns></returns>   
    public static bool IsContain(ArrayList indexList, int index)
    {
        for (int i = 0; i < indexList.Count; i++)
        {
            int tempIndex = Convert.ToInt32(indexList[i]);
            if (tempIndex == index)
            {
                return true;
            }
        }
        return false;
    }
}