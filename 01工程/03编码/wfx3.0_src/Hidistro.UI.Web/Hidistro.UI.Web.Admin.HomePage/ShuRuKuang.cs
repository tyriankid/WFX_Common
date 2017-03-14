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
    public class ShuRuKuang : BaseModel
	{
        public DataTable dt =null;
        public string PMID;
        public string Bt;
        public string TitleZ;
        public string TitleC;
        public string ControlHtml;

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
                if (dt.Rows[0]["PMStyle"].ToString() == "input")
                    ControlHtml = string.Format("<input type=\"text\" id=\"input_{0}\" class=\"w_txt\">", PageSN);
                else
                    ControlHtml = string.Format("<textarea class=\"w_txt\" id=\"textarea_{0}\"></textarea>", PageSN);
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
