using Hidistro.Core.Jobs;
using System;
using System.Web;
namespace Hidistro.UI.Web
{
	public class Global : System.Web.HttpApplication
	{
		protected void Application_AuthenticateRequest(object sender, System.EventArgs e)
		{
		}
		protected void Application_BeginRequest(object sender, System.EventArgs e)
		{
		}
		protected void Application_End(object sender, System.EventArgs e)
		{
			Hidistro.Core.Jobs.Jobs.Instance().Stop();
		}
		protected void Application_Error(object sender, System.EventArgs e)
		{
		}
		protected void Application_Start(object sender, System.EventArgs e)
		{
            Hidistro.Core.Jobs.Jobs.Instance().Start();
		}
		protected void Session_End(object sender, System.EventArgs e)
		{
		}
		protected void Session_Start(object sender, System.EventArgs e)
		{
		}
	}
}
