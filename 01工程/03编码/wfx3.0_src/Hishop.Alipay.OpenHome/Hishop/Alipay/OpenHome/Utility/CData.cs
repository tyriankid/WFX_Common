namespace Hishop.Alipay.OpenHome.Utility
{
    using System;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public class CData : IXmlSerializable
    {
        private string m_Value;

        public CData()
        {
        }

        public CData(string p_Value)
        {
            this.m_Value = p_Value;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public static implicit operator string(CData element)
        {
            return ((element == null) ? null : element.m_Value);
        }

        public static implicit operator CData(string text)
        {
            return new CData(text);
        }

        public void ReadXml(XmlReader reader)
        {
            this.m_Value = reader.ReadElementContentAsString();
        }

        public override string ToString()
        {
            return this.m_Value;
        }

        public void WriteXml(XmlWriter writer)
        {
            if (!string.IsNullOrEmpty(this.m_Value))
            {
                writer.WriteCData(this.m_Value);
            }
            else
            {
                writer.WriteString(this.m_Value);
            }
        }

        public string Value
        {
            get
            {
                return this.m_Value;
            }
        }
    }
}

