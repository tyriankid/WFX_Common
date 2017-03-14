namespace Hishop.Alipay.OpenHome.Request
{
    using Hishop.Alipay.OpenHome.Model;
    using Hishop.Alipay.OpenHome.Utility;
    using System;
    using System.Runtime.InteropServices;

    public class MessagePushRequest : IRequest
    {
        private Message message;

        public MessagePushRequest(string appid, string toUserId, Articles articles, int articleCount, string agreementId = null, string msgType = "image-text")
        {
            Message message = new Message {
                AgreementId = agreementId,
                AppId = appid,
                Articles = articles,
                ArticleCount = articleCount,
                ToUserId = toUserId,
                CreateTime = TimeHelper.TransferToMilStartWith1970(DateTime.Now).ToString("F0"),
                MsgType = msgType
            };
            this.message = message;
        }

        public string GetBizContent()
        {
            return XmlSerialiseHelper.Serialise<Message>(this.message);
        }

        public string GetMethodName()
        {
            return "alipay.mobile.public.message.push";
        }
    }
}

