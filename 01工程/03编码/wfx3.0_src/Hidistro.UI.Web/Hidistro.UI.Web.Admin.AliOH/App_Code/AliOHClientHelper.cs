namespace Hidistro.UI.Web.App_Code
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    //using Hidistro.Membership.Context;
    using Hishop.Alipay.OpenHome;
    using System;

    public class AliOHClientHelper
    {
        public static AlipayOHClient Instance(string serverRootPath)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string aliOHServerUrl = masterSettings.AliOHServerUrl;
            string aliOHAppId = masterSettings.AliOHAppId;
            string priKey = serverRootPath + "/config/rsa_private_key.pem";
            string aliPubKey = serverRootPath + "/config/alipay_pubKey.pem";
            return new AlipayOHClient(aliOHServerUrl, aliOHAppId, aliPubKey, priKey, serverRootPath + "/config/rsa_public_key.pem", "UTF-8");
        }
    }
}

