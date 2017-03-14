using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Function;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
    public class Debug : AdminPage
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(base.Request["ID"]))
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("Vshop-Member");
                    string s = "";
                    if (cookie != null)
                    {
                        s = cookie.Value;
                        cookie.Value = null;
                        cookie.Expires = DateTime.Now.AddYears(-1);
                        HttpContext.Current.Response.Cookies.Set(cookie);
                    }

                    cookie = new HttpCookie("Vshop-Member")
                    {
                        Value = base.Request["ID"],
                        Expires = DateTime.Now.AddYears(10)
                    };
                    HttpContext.Current.Response.Cookies.Add(cookie);

                    Response.Redirect("/Vshop/index.aspx");
                }

            }
		}
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
			base.Render(writer);
		}

	}
}
