namespace Hidistro.UI.Web.Admin.AliOH
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    //using Hidistro.Membership.Context;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.App_Code;
    //using Hidistro.UI.Web.App_Code;
    using System;
    using System.IO;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.AliohMobileAlipaySet)]
    public class WebForm1 : AdminPage
    {
        protected Button btnAdd;
        protected TextBox txtAppId;
        protected TextBox txtAppWelcome;
        protected TextBox txtPubKey;
        protected Literal txtUrl;

        protected void btnOK_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            masterSettings.AliOHAppId = this.txtAppId.Text;
            masterSettings.AliOHFollowRelay = this.txtAppWelcome.Text;
            SettingsManager.Save(masterSettings);
            this.ShowMsg("修改成功", true);
        }

        private string CreateRsaKey()
        {
            string keyDirectory = base.Server.MapPath("~/config");
            return RsaKeyHelper.CreateRSAKeyFile(base.Server.MapPath("~/config/RSAGenerator/Rsa.exe"), keyDirectory);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                string rSAKeyContent;
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                string path = base.Server.MapPath("~/config/rsa_public_key.pem");
                if (!File.Exists(path))
                {
                    rSAKeyContent = this.CreateRsaKey();
                    SettingsManager.Save(masterSettings);
                }
                else
                {
                    rSAKeyContent = RsaKeyHelper.GetRSAKeyContent(path, true);
                }
                this.txtAppId.Text = masterSettings.AliOHAppId;
                this.txtAppWelcome.Text = masterSettings.AliOHFollowRelay;
                this.txtUrl.Text = string.Format("http://{0}/api/alipay.ashx", base.Request.Url.Host);
                this.txtPubKey.Text = rSAKeyContent;
            }
        }
    }
}

