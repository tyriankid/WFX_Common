namespace Hishop.Alipay.OpenHome.Model
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class AliUserTokenInfo
    {
        public string access_token { get; set; }

        public string alipay_user_id { get; set; }
    }
}

