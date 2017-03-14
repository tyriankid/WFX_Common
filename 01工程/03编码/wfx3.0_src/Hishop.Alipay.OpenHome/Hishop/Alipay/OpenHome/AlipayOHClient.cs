namespace Hishop.Alipay.OpenHome
{
    using Aop.Api;
    using Aop.Api.Request;
    using Aop.Api.Response;
    using Aop.Api.Util;
    using Hishop.Alipay.OpenHome.AlipayOHException;
    using Hishop.Alipay.OpenHome.Handle;
    using Hishop.Alipay.OpenHome.Model;
    using Hishop.Alipay.OpenHome.Request;
    using Hishop.Alipay.OpenHome.Utility;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    public class AlipayOHClient
    {
        private const string ACCESS_TOKEN = "auth_token";
        private string aliPubKey;
        private const string APP_ID = "app_id";
        private string appId;
        private const string BIZ_CONTENT = "biz_content";
        private string charset;
        private const string CHARSET = "charset";
        private const string CONTENT = "biz_content";
        private HttpContext context;
        private string dateTimeFormat;
        private string format;
        private const string FORMAT = "format";
        private const string METHOD = "method";
        private string privateKey;
        private const string PROD_CODE = "prod_code";
        private string pubKey;
        public AliRequest request;
        private string serverUrl;
        private const string SERVICE = "service";
        private const string SIGN = "sign";
        private const string SIGN_TYPE = "sign_type";
        private string signType;
        private const string SING = "sign";
        private const string SING_TYPE = "sign_type";
        private const string TERMINAL_INFO = "terminal_info";
        private const string TERMINAL_TYPE = "terminal_type";
        private const string TIMESTAMP = "timestamp";
        private string version;
        private const string VERSION = "version";
        private WebUtils webUtils;

        public event Hishop.Alipay.OpenHome.OnUserFollow OnUserFollow;

        public AlipayOHClient(string aliPubKey, string priKey, string pubKey, string charset = "UTF-8")
        {
            this.signType = "RSA";
            this.dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            this.webUtils = new WebUtils();
            this.privateKey = priKey;
            this.charset = charset;
            this.pubKey = pubKey;
            this.aliPubKey = aliPubKey;
        }

        public AlipayOHClient(string url, string appId, string aliPubKey, string priKey, string pubKey, string charset = "UTF-8")
        {
            this.signType = "RSA";
            this.dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            this.webUtils = new WebUtils();
            this.serverUrl = url;
            this.appId = appId;
            this.privateKey = priKey;
            this.charset = charset;
            this.pubKey = pubKey;
            this.aliPubKey = aliPubKey;
        }

        public T Execute<T>(IRequest request) where T: AliResponse, IAliResponseStatus
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", request.GetMethodName());
            parameters.Add("app_id", this.appId);
            parameters.Add("timestamp", DateTime.Now.ToString(this.dateTimeFormat));
            parameters.Add("sign_type", this.signType);
            parameters.Add("charset", this.charset);
            parameters.Add("biz_content", request.GetBizContent());
            parameters.Add("sign", AlipaySignature.RSASign(parameters, this.privateKey, this.charset));
            T local = JsonConvert.DeserializeObject<T>(this.webUtils.DoPost(this.serverUrl, parameters, this.charset));
            if ((local.error_response != null) && local.error_response.IsError)
            {
                throw new AliResponseException(local.error_response.code, local.error_response.sub_msg);
            }
            if (local.Code != "200")
            {
                throw new AliResponseException(local.Code, local.Message);
            }
            return local;
        }

        internal string FireUserFollowEvent()
        {
            return this.OnUserFollow(this, new EventArgs());
        }

        public AlipayUserUserinfoShareResponse GetAliUserInfo(string accessToken)
        {
            AlipayUserUserinfoShareRequest request = new AlipayUserUserinfoShareRequest {
                AuthToken = accessToken
            };
            IAopClient client = new DefaultAopClient(this.serverUrl, this.appId, this.privateKey);
            return client.Execute<AlipayUserUserinfoShareResponse>(request);
        }

        public void HandleAliOHResponse(HttpContext context)
        {
            this.context = context;
            string str = context.Request["sign"];
            string xml = context.Request["biz_content"];
            string str3 = context.Request["sign_type"];
            string str4 = context.Request["service"];
            this.request = XmlSerialiseHelper.Deserialize<AliRequest>(xml);
            IHandle handle = null;
            string eventType = this.request.EventType;
            if (eventType != null)
            {
                if (!(eventType == "verifygw"))
                {
                    if (eventType == "follow")
                    {
                        handle = new UserFollowHandle();
                    }
                }
                else
                {
                    handle = new VerifyGateWayHandle();
                }
            }
            if (handle != null)
            {
                handle.client = this;
                handle.LocalRsaPriKey = this.privateKey;
                handle.LocalRsaPubKey = this.pubKey;
                handle.AliRsaPubKey = this.aliPubKey;
                string s = handle.Handle(xml);
                context.Response.AddHeader("Content-Type", "application/xml");
                context.Response.Write(s);
            }
        }

        public AlipaySystemOauthTokenResponse OauthTokenRequest(string authCode)
        {
            AlipaySystemOauthTokenRequest request = new AlipaySystemOauthTokenRequest {
                GrantType = AlipaySystemOauthTokenRequest.AllGrantType.authorization_code,
                Code = authCode
            };
            AlipaySystemOauthTokenResponse response = null;
            try
            {
                IAopClient client = new DefaultAopClient(this.serverUrl, this.appId, this.privateKey);
                response = client.Execute<AlipaySystemOauthTokenResponse>(request);
            }
            catch (AopException)
            {
            }
            return response;
        }

        private class EventType
        {
            public const string Follow = "follow";
            public const string Verifygw = "verifygw";
        }
    }
}

