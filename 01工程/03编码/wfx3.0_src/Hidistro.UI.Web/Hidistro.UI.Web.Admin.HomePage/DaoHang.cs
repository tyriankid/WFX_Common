using ASPNET.WebControls;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.VShop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.HomePage
{
    public class DaoHang : BaseModel
	{
        public DataTable dt =null;
        public string[] licons = null;
        public string dis = "";
        public string wzdis = "";
        public string[] tl = null;
		protected void Page_Load(object sender, System.EventArgs e)
		{
            InitData();
		}
        private void InitData()
        {
            dt = DataBaseHelper.GetDataTable("YiHui_HomePage_Model", " PageID ='" + this.PKID+"'");
            licons = dt.Rows[0]["PMContents"].ToString().TrimEnd('♢').Split('♢');//♦
            dis = (dt.Rows[0]["PMImgDisplay"]+"") == "1" ? "block" : "none";
            wzdis = (dt.Rows[0]["PMTxtDisplay"] + "") == "1" ? "block" : "none";
            tl = (dt.Rows[0]["PMValue2"] + "").Split('♦');
        }
	}
}
