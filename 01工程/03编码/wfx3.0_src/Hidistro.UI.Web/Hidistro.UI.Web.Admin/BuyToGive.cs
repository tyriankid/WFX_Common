using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[AdministerCheck(true)]
    public class BuyToGive : AdminPage
	{
        protected RadioButton rbOn;
        protected RadioButton rbOff;
		protected System.Web.UI.WebControls.Button btnOK;

		protected void btnOK_Click(object sender, System.EventArgs e)
		{
			bool isOnOff;
            if (rbOn.Checked)
			{
                isOnOff = true;
			}
			else
			{
                isOnOff = false;
			}
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            masterSettings.BuyOrGive = isOnOff;
            SettingsManager.Save(masterSettings);
            this.ShowMsg("保存成功", true);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                bool isOnOff = masterSettings.BuyOrGive;
                if (isOnOff)
                {
                    rbOff.Checked = false;
                    rbOn.Checked = true;
                }
                else
                {
                    rbOn.Checked = false;
                    rbOff.Checked = true;
                }
			}
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
		}
	}
}
