namespace Hishop.Alipay.OpenHome.Request
{
    using System;

    public class OauthTokenRequest : IRequest
    {
        private string token;

        public string GetBizContent()
        {
            throw new NotImplementedException();
        }

        public string GetMethodName()
        {
            return "alipay.system.oauth.token";
        }

        public void SetToken(string token)
        {
            this.token = token;
        }
    }
}

