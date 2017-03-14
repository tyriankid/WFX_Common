namespace Hishop.Alipay.OpenHome.Model
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    [Serializable, XmlRoot("XML")]
    internal class AliRequstWhenFollow : AliRequest
    {
        [XmlElement("UserInfo")]
        public Hishop.Alipay.OpenHome.Model.UserInfo UserInfo { get; set; }
    }
}

