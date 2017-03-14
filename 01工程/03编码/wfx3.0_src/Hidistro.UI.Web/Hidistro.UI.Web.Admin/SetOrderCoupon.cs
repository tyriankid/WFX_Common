using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.Web.Admin
{
    [AdministerCheck(true)]
    public class SetOrderCoupon : AdminPage
    {
        protected System.Web.UI.WebControls.Button btnOK;
        protected System.Web.UI.WebControls.TextBox txtPriceRent;
        protected System.Web.UI.WebControls.CheckBox chkOnOff;

        protected void btnOK_Click(object sender, System.EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string str = this.txtPriceRent.Text.Trim();
            if (!this.chkOnOff.Checked) str = "0";
            masterSettings.orderCouponRent = str.ToInt();
            SettingsManager.Save(masterSettings);
            this.ShowMsg("保存成功", true);
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.txtPriceRent.Text = masterSettings.orderCouponRent.ToString();
                this.chkOnOff.Checked = masterSettings.orderCouponRent>0;
            }
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
        }

    }
}
