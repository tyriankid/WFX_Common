namespace Hishop.Alipay.OpenHome.Model
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    [XmlRoot("XML")]
    public class AliRequest
    {
        [XmlElement("AccountNo")]
        public string AccountNo { get; set; }

        [XmlElement("ActionParam")]
        public string ActionParam { get; set; }

        [XmlElement("AgreementId")]
        public string AgreementId { get; set; }

        [XmlElement("AppId")]
        public string AppId { get; set; }

        [XmlElement("CreateTime")]
        public long CreateTime { get; set; }

        [XmlElement("EventType")]
        public string EventType { get; set; }

        [XmlElement("FromUserId")]
        public string FromUserId { get; set; }

        [XmlElement("MsgType")]
        public string MsgType { get; set; }
    }
}

