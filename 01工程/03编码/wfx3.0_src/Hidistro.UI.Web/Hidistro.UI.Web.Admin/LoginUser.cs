using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using System;
using System.Web;
namespace Hidistro.UI.Web.Admin
{
	public class LoginUser : System.Web.IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		public void ProcessRequest(System.Web.HttpContext context)
		{
			string s = "";
			string str2 = context.Request.QueryString["action"];
			if (!string.IsNullOrEmpty(str2) && str2 == "login")
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				s = string.Concat(new string[]
				{
					"{\"sitename\":\"",
					masterSettings.SiteName,
					"\",\"username\":\"",
					ManagerHelper.GetCurrentManager().UserName,
					"\"}"
				});
			}
			context.Response.ContentType = "text/json";
			context.Response.Write(s);
		}
	}
}
