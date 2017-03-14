using ASPNET.WebControls;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.VShop;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Controls
{
    public class BaseModel : System.Web.UI.UserControl 
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
            
		}
        private Guid _pkid;
        private string _pagesn;
        public Guid PKID
        {
            set { _pkid = value; }
            get { return _pkid; }
        }
        public string PageSN {
            set { _pagesn = value; }
            get { return _pagesn; }
        }
	}
}
