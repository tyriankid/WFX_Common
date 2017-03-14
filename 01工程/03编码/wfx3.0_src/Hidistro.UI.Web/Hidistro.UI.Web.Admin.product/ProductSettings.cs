using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml;
using Hidistro.Core;
using Hidistro.Core.Entities;
using kindeditor.Net;
using System.Web.UI.HtmlControls;
namespace Hidistro.UI.Web.Admin.product 
{
	[PrivilegeCheck(Privilege.EditProducts)]
    public class ProductSettings : AdminPage
	{
        protected System.Web.UI.WebControls.Button btnSave;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton radioreviewson;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton radioreviewsoff;
        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            masterSettings.EnableProductReviews = this.radioreviewson.Checked;
            SettingsManager.Save(masterSettings);
            this.ShowMsg("修改成功", true);
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.radioreviewson.Checked = masterSettings.EnableProductReviews;
                this.radioreviewsoff.Checked = !masterSettings.EnableProductReviews;


            }
        }
	}
}
