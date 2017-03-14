
using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Vshop;
using Hidistro.SqlDal.Members;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.agent
{
    public partial class Closed : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (!masterSettings.isCloseStore)             
                    this.radioOpenStoreInfoSanzuoOff.Checked = true;
                else 
                    this.radioOpenStoreInfoSanzuoOn.Checked = true;

            }
            this.btnDutyOff.Click += new EventHandler(this.btnDutyOff_Click);
        }
        private void btnDutyOff_Click(object sender, System.EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            masterSettings.isCloseStore = this.radioOpenStoreInfoSanzuoOn.Checked;
            SettingsManager.Save(masterSettings);
            this.ShowMsg("修改成功", true);
        }
    }
}
