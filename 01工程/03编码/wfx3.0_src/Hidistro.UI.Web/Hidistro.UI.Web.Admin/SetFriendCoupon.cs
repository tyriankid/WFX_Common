using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.Web.Admin
{
    [AdministerCheck(true)]
    public class SetFriendCoupon : AdminPage
    {
        protected System.Web.UI.WebControls.Button btnOK;
        protected System.Web.UI.WebControls.DropDownList ddlCoupons;
        protected System.Web.UI.WebControls.CheckBox chkOnOff;

        protected void btnOK_Click(object sender, System.EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string str = this.ddlCoupons.SelectedValue;
            if (!this.chkOnOff.Checked) str = "0";
            masterSettings.friendCouponId = str.ToInt();
            SettingsManager.Save(masterSettings);
            this.ShowMsg("保存成功", true);
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                DataTable dtCoupons = CouponHelper.GetAllCoupons();
                foreach (DataRow row in dtCoupons.Rows)
                {
                    ddlCoupons.Items.Add(new ListItem() { 
                        Text = row["name"].ToString(),
                        Value = row["CouponId"].ToString()
                    });
                }
                if (masterSettings.friendCouponId > 0 && dtCoupons.Rows.Count > 0)
                    this.ddlCoupons.SelectedValue = masterSettings.friendCouponId.ToString();
                        //Items.FindByValue(masterSettings.friendCouponId.ToString()).Selected=true;
                this.chkOnOff.Checked = masterSettings.friendCouponId > 0;
            }
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
        }

    }
}
