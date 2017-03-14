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
    public class XuanXiang : BaseModel
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

                //DataTable dtDetail = DataBaseHelper.GetDataTable("YIHui_Votes_Model_Detail", "VMID = '" + this.PKID + "'");
                //int i = 0;
                //foreach (DataRow drdetail in dtDetail.Rows)
                //{
                //    i++;
                //    ControlHtml += string.Format("<li id='nav_li{0}' n='{0}' style='background-position:15px 50px;'>{1}</li>", i, drdetail["Name"].ToString());
                //}

                string[] strValues = dt.Rows[0]["PMContents"].ToString().Split('♦');
                if (strValues.Length == 3)
                {
                    TitleZ = strValues[0];
                    TitleC = strValues[1];

                    string[] strItemHtml = strValues[2].Split('♢');
                    if (strItemHtml.Length > 0)
                    {
                        int i = 0;
                        foreach (string itemhtml in strItemHtml)
                        {
                            i++;
                            if (!string.IsNullOrEmpty(itemhtml))
                            {
                                string[] strHtml = itemhtml.Split('□');
                                if (strHtml.Length == 3)
                                    ControlHtml += string.Format("<li id='nav_li{0}' n='{0}' style='background-position:15px 10px;'>{1}</li>", i, strHtml[2]);
                            }
                        }
                    }
                }
            }
            
        }
	}
}
