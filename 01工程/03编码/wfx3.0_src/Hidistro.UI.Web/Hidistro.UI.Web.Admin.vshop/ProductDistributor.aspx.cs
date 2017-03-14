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
    public class ProductDistributor : AdminPage
    {
        private int ProductId;
        protected System.Web.UI.WebControls.Literal productName;
        protected HtmlInputHidden into;
        protected HtmlInputHidden mainId;
        protected HtmlInputHidden cProductId;
        protected HtmlInputHidden cDistributor;
        protected HtmlInputHidden cPrice;
        
        protected void btnAddButton_Click(object sender, System.EventArgs e)
        {
            //得到表的数据源
            DataTable dataBase = DataBaseHelper.GetDataTable("Hishop_ProductDistributor", string.Format("ProductId ='{0}'", ProductId));
            //获取当前主键表的列数
            dataBase.PrimaryKey = new DataColumn[] { dataBase.Columns["id"] };
            //新表克隆旧表的架构
            DataTable newDt = dataBase.Clone();
            //获取当前主键表的列数
            newDt.PrimaryKey = new DataColumn[] { newDt.Columns["id"] };
             //获取隐藏域的值
            string strUserInput = into.Value;
            //隐藏域的值去分号
            string[] strUserInputTrs = strUserInput.Split(';');
            //如果隐藏域的值不为空
            if (into.Value != "")
            {
                //循环
                foreach (string UserInputTrs in strUserInputTrs)
                {
                    //去逗号
                    string[] sUserInputTr = UserInputTrs.Split(',');
                    //创建相同构架的表
                    DataRow drNew = newDt.NewRow();
                    //三元运算符 如果sUserInputTr[0]索引为空 就生成新的Guid 
                    drNew["id"] = (string.IsNullOrEmpty(sUserInputTr[0])) ? Guid.NewGuid() : new Guid(sUserInputTr[0]);
                    drNew["ProductId"] = ProductId;
                    drNew["Distributor"] = sUserInputTr[1];
                    drNew["Price"] = sUserInputTr[2];
                    newDt.Rows.Add(drNew);
                }            
            }
            //根据两个表生成的Datatable 用于整表提交
            dataBase = DataBaseHelper.GetDtDifferent(dataBase, newDt);

            if (DataBaseHelper.CommitDataTable(dataBase, "Select * From Hishop_ProductDistributor") != -1)
            {
                this.ShowMsgAndReUrl("保存成功！", true, "ProductDistributor.aspx?ProductId=" + ProductId);
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
            DataTable dataBase = DataBaseHelper.GetDataTable("Hishop_ProductDistributor", string.Format("ProductId ='{0}'", ProductId));
            foreach (DataRow row in dataBase.Rows)
            {             
                mainId.Value += row["id"].ToString() + ";";
                cProductId.Value += row["ProductId"].ToString() + ";";
                cDistributor.Value += row["Distributor"].ToString() + ";";
                cPrice.Value += row["Price"].ToString() + ";";
            }
                mainId.Value = mainId.Value.TrimEnd(';');
                cProductId.Value = cProductId.Value.TrimEnd(';');
                cDistributor.Value = cDistributor.Value.TrimEnd(';');
                cPrice.Value = cPrice.Value.TrimEnd(';');
        }
    }
}
