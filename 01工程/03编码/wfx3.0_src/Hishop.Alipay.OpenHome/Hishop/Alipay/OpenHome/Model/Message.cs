namespace Hishop.Alipay.OpenHome.Model
{
    using Hishop.Alipay.OpenHome.Utility;
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    [XmlRoot("xml")]
    public class Message
    {
        private string agreementId;
        private string appId;
        private string msgType;
        private string toUserId;

        [XmlElement("AgreementId", typeof(CData))]
        public CData AgreementId
        {
            get
            {
                return this.agreementId;
            }
            set
            {
                this.agreementId = (string) value;
            }
        }

        [XmlElement("AppId", typeof(CData))]
        public CData AppId
        {
            get
            {
                return this.appId;
            }
            set
            {
                this.appId = (string) value;
            }
        }

        [XmlElement("ArticleCount")]
        public int ArticleCount { get; set; }

        [XmlElement("Articles")]
        public Hishop.Alipay.OpenHome.Model.Articles Articles { get; set; }

        [XmlElement("CreateTime")]
        public string CreateTime { get; set; }

        [XmlElement("MsgType", typeof(CData))]
        public CData MsgType
        {
            get
            {
                return this.msgType;
            }
            set
            {
                this.msgType = (string) value;
            }
        }

        [XmlElement("ToUserId", typeof(CData))]
        public CData ToUserId
        {
            get
            {
                return this.toUserId;
            }
            set
            {
                this.toUserId = (string) value;
            }
        }
    }
}

