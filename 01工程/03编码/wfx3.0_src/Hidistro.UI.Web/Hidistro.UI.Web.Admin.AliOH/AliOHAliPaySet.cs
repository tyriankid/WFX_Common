namespace Hidistro.UI.Web.Admin.AliOH
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    //using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;
    using System.Xml;

    [PrivilegeCheck(Privilege.AliohMobileAlipaySet)]
    public class AliOHAliPaySet : AdminPage
    {
        protected Button btnAdd;
        protected YesNoRadioButtonList radEnableWapAliPay;
        protected TextBox txtAccount;
        protected TextBox txtKey;
        protected TextBox txtPartner;

        protected void btnOK_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            masterSettings.EnableAliOHAliPay = this.radEnableWapAliPay.SelectedValue;
            SettingsManager.Save(masterSettings);
            string text = string.Format("<xml><Partner>{0}</Partner><Key>{1}</Key><Seller_account_name>{2}</Seller_account_name></xml>", this.txtPartner.Text, this.txtKey.Text, this.txtAccount.Text);
            PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("hishop.plugins.payment.ws_wappay.wswappayrequest");
            if (paymentMode == null)
            {
                PaymentModeInfo info2 = new PaymentModeInfo {
                    Name = "支付宝手机网页支付",
                    Gateway = "hishop.plugins.payment.ws_wappay.wswappayrequest",
                    Description = string.Empty,
                    IsUseInpour = false,
                    Charge = 0M,
                    IsPercent = false,
                    ApplicationType = PayApplicationType.payOnWAP,
                    Settings = HiCryptographer.Encrypt(text)
                };
                SalesHelper.CreatePaymentMode(info2);
            }
            else
            {
                PaymentModeInfo info4 = paymentMode;
                info4.Settings = HiCryptographer.Encrypt(text);
                info4.ApplicationType = PayApplicationType.payOnWAP;
                SalesHelper.UpdatePaymentMode(info4);
            }
            this.ShowMsg("修改成功", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.radEnableWapAliPay.SelectedValue = masterSettings.EnableAliOHAliPay;
                PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("hishop.plugins.payment.ws_wappay.wswappayrequest");
                if (paymentMode != null)
                {
                    string xml = HiCryptographer.Decrypt(paymentMode.Settings);
                    XmlDocument document = new XmlDocument();
                    document.LoadXml(xml);
                    this.txtPartner.Text = document.GetElementsByTagName("Partner")[0].InnerText;
                    this.txtKey.Text = document.GetElementsByTagName("Key")[0].InnerText;
                    this.txtAccount.Text = document.GetElementsByTagName("Seller_account_name")[0].InnerText;
                }
                PaymentModeInfo info2 = SalesHelper.GetPaymentMode("hishop.plugins.payment.ws_apppay.wswappayrequest");
                if (info2 != null)
                {
                    string str2 = HiCryptographer.Decrypt(info2.Settings);
                    new XmlDocument().LoadXml(str2);
                }
            }
        }
    }
}

