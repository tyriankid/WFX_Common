namespace Hishop.Alipay.OpenHome.Model
{
    using Hishop.Alipay.OpenHome.Utility;
    using System;
    using System.Xml.Serialization;

    [XmlRoot("Item")]
    public class Item
    {
        private string description;
        private string imageUrl;
        private string title;
        private string url;

        [XmlElement("Desc", typeof(CData))]
        public CData Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = (string) value;
            }
        }

        [XmlElement("ImageUrl", typeof(CData))]
        public CData ImageUrl
        {
            get
            {
                return this.imageUrl;
            }
            set
            {
                this.imageUrl = (string) value;
            }
        }

        [XmlElement("Title", typeof(CData))]
        public CData Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = (string) value;
            }
        }

        [XmlElement("Url", typeof(CData))]
        public CData Url
        {
            get
            {
                return this.url;
            }
            set
            {
                this.url = (string) value;
            }
        }
    }
}

