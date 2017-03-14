using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_promotion_BuyToHalf : AdminPage
{
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
        masterSettings.BuyOrHalf = isOnOff;
        SettingsManager.Save(masterSettings);
        this.ShowMsg("保存成功", true);
    }
    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (!this.Page.IsPostBack)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            bool isOnOff = masterSettings.BuyOrHalf;
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