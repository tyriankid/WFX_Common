namespace Hishop.Alipay.OpenHome.Response
{
    using Hishop.Alipay.OpenHome.Model;
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class MessagePushResponse : AliResponse, IAliResponseStatus
    {
        public AliResponseMessage alipay_mobile_public_message_push_response { get; set; }

        public string Code { get; set; }

        public string Message { get; set; }

        public string sign { get; set; }
    }
}

