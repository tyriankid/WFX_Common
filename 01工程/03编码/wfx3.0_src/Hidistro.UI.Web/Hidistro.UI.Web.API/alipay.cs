namespace Hidistro.UI.Web.API
{
    using Hidistro.Membership.Context;
    using Hishop.Alipay.OpenHome;
    using Hishop.Alipay.OpenHome.Model;
    using Hishop.Alipay.OpenHome.Request;
    using Hishop.Alipay.OpenHome.Response;
    using System;
    using System.Web;

    public class alipay : IHttpHandler
    {
        private string alipayPubKeyFile;
        private string logfile;
        private string priKeyFile;
        private string pubKeyFile;

        private string client_OnUserFollow(object sender, EventArgs e)
        {
            try
            {
                AlipayOHClient client = (AlipayOHClient) sender;
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                Articles articles2 = new Articles();
                Item item = new Item {
                    Description = masterSettings.AliOHFollowRelay,
                    Title = string.IsNullOrWhiteSpace(masterSettings.AliOHFollowRelayTitle) ? "欢迎您的关注！" : masterSettings.AliOHFollowRelayTitle
                };
                articles2.Item = item;
                Articles articles = articles2;
                IRequest request = new MessagePushRequest(client.request.AppId, client.request.FromUserId, articles, 1, null, "image-text");
                new AlipayOHClient(masterSettings.AliOHServerUrl, client.request.AppId, this.alipayPubKeyFile, this.priKeyFile, this.pubKeyFile, "UTF-8").Execute<MessagePushResponse>(request);
            }
            catch (Exception)
            {
            }
            return "";
        }

        public void ProcessRequest(HttpContext context)
        {
            string text1 = context.Request["EventType"];
            this.priKeyFile = context.Server.MapPath("~/config/rsa_private_key.pem");
            this.alipayPubKeyFile = context.Server.MapPath("~/config/alipay_pubKey.pem");
            this.pubKeyFile = context.Server.MapPath("~/config/rsa_public_key.pem");
            this.logfile = context.Server.MapPath("~/a.log");
            AlipayOHClient client = new AlipayOHClient(this.alipayPubKeyFile, this.priKeyFile, this.pubKeyFile, "UTF-8");
            client.OnUserFollow += new OnUserFollow(this.client_OnUserFollow);
            client.HandleAliOHResponse(context);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

