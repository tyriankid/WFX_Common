using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.vshop
{
    public class ModifyGoods : AdminPage
    {
        private int ProductId;
        protected System.Web.UI.WebControls.Literal productName;
        protected HtmlInputHidden info;
        protected HtmlInputHidden mainId;
        protected HtmlInputHidden cId;
        protected HtmlInputHidden cSource;
        protected HtmlInputHidden cCode;

        protected void btnAddButton_Click(object sender, System.EventArgs e)
        {
                //得到原始数据
                DataTable dtDataBase = DataBaseHelper.GetDataTable("Hishop_ProductsList", string.Format("CommodityId={0}", ProductId));
                //设置表的主键
                dtDataBase.PrimaryKey = new DataColumn[] { dtDataBase.Columns["id"] };
                //用户输入的DataTable(新表等于旧表)
                DataTable newDt = dtDataBase.Clone();
                newDt.PrimaryKey = new DataColumn[] { newDt.Columns["id"] };
                //更新用户输入的数据
                //获取隐藏域的值
                string strUserInput = info.Value;//(id#code#来源@id2#code2#来源2)
                //隐藏域的值去分号分割
                string[] strUserInputTrs = strUserInput.Split(';');
                if (info.Value != "")
                {
                    foreach (string strUserInputTr in strUserInputTrs)
                    {
                        string[] strProduct = strUserInputTr.Split(',');
                        DataRow drNew = newDt.NewRow();
                        drNew["id"] = (string.IsNullOrEmpty(strProduct[0])) ? Guid.NewGuid() : new Guid(strProduct[0]);
                        drNew["CommodityId"] = ProductId;
                        drNew["CommoditySource"] = strProduct[1];
                        drNew["CommodityCode"] = strProduct[2];
                        newDt.Rows.Add(drNew);
                    }
                }
                dtDataBase = DataBaseHelper.GetDtDifferent(dtDataBase, newDt);//根据两个表的不同生成新的dt1,用于整表提交

                //整表提交
                if (DataBaseHelper.CommitDataTable(dtDataBase, "Select * From Hishop_ProductsList") != -1)
                {
                    this.ShowMsgAndReUrl("保存成功！", true, "ModifyGoods.aspx?ProductId=" + ProductId);
                }             
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            ProductId = Convert.ToInt32(base.Request.QueryString["ProductId"]);
            if (!this.Page.IsPostBack)
            {
                this.pageInit();
                this.GetHishop_ProductsId(ProductId);
            }    
        }

        private void GetHishop_ProductsId(int ProductId)
        {
            ProductQuery query = ProductHelper.GetHishop_ProductsId(ProductId);
            this.productName.Text = query.ProductName;
        }

        private void pageInit()
        {
            //得到原始数据
            DataTable dtDataBase = DataBaseHelper.GetDataTable("Hishop_ProductsList", string.Format("CommodityId={0}", ProductId));
            foreach (DataRow row in dtDataBase.Rows)
            {
                mainId.Value += row["id"].ToString() + ";";
                cId.Value += row["CommodityId"].ToString() + ";";
                cSource.Value += row["CommoditySource"].ToString() + ";";
                cCode.Value += row["CommodityCode"].ToString() + ";";
            }
            mainId.Value = mainId.Value.TrimEnd(';');
            cId.Value = cId.Value.TrimEnd(';');
            cSource.Value = cSource.Value.TrimEnd(';');
            cCode.Value = cCode.Value.TrimEnd(';');
        }
    }
}
