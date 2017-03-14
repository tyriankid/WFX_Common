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
    public class ShiJian : BaseModel
	{
        public DataTable dt =null;
        public string PMID;
        public string Bt;
        public string TitleZ;
        public string TitleC;

		protected void Page_Load(object sender, System.EventArgs e)
		{
            InitData();
		}
        private void InitData()
        {
            dt = DataBaseHelper.GetDataTable("YiHui_HomePage_Model", " PageID ='" + this.PKID+"'");
            if (dt.Rows.Count > 0)
            {
                PMID = dt.Rows[0]["PMID"].ToString();
                if (!string.IsNullOrEmpty(dt.Rows[0]["PMTop"].ToString()))
                    Bt = dt.Rows[0]["PMTop"].ToString() == "1" ? "true" : "false";
                else
                    Bt = string.Empty;
                string[] strValues = dt.Rows[0]["PMContents"].ToString().Split('♦');
                if (strValues.Length == 2)
                {
                    TitleZ = strValues[0];
                    TitleC = strValues[1];
                }
            }
            
        }
	}
}
