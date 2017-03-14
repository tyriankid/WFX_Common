namespace Hishop.Alipay.OpenHome.Model
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    public class BasicResponse
    {
        public string Body { get; set; }

        [XmlElement("code")]
        public string ErrCode { get; set; }

        [XmlElement("msg")]
        public string ErrMsg { get; set; }

        public bool IsError
        {
            get
            {
                return !string.IsNullOrEmpty(this.SubErrCode);
            }
        }

        [XmlElement("sub_code")]
        public string SubErrCode { get; set; }

        [XmlElement("sub_msg")]
        public string SubErrMsg { get; set; }
    }
}

