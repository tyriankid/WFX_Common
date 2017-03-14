using ASPNET.WebControls;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.VShop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.HomePage
{
    public class ShangPin : BaseModel
	{
        public DataTable dt =null;
        public string StyleName = "";
        protected Repeater rptGoods;
		protected void Page_Load(object sender, System.EventArgs e)
		{
            InitData();
		}
        private void InitData()
        {
            try
            {
                dt = DataBaseHelper.GetDataTable("YiHui_HomePage_Model", " PageID ='" + this.PKID + "'");
                string sql = "select  p.*,(select MIN(SalePrice) from Hishop_SKUs where ProductId =p.ProductId) as SalePrice from Hishop_Products as p  where p.SaleStatus=1 and p.ProductId in (" + dt.Rows[0]["PMContents"] + ") ";
                rptGoods.DataSource = DataBaseHelper.GetDataSet(sql);
                rptGoods.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
         
	}
}
