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
    public class GunDong : BaseModel
	{
        public DataTable dt =null;
        public string[] cons = null;
        public string height = "";
        public string[] tl = null;
		protected void Page_Load(object sender, System.EventArgs e)
		{
            InitData();
		}
        private void InitData()
        {
            dt = DataBaseHelper.GetDataTable("YiHui_HomePage_Model", " PageID ='" + this.PKID+"'");
            height = dt.Rows[0]["PMHeight"]+"";
            cons = dt.Rows[0]["PMContents"].ToString().Split('♦');
            tl = (dt.Rows[0]["PMValue2"] + "").Split('♦');
        }
	}
}
