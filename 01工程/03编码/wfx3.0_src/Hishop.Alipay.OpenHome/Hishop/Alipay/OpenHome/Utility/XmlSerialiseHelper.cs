namespace Hishop.Alipay.OpenHome.Utility
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    public class XmlSerialiseHelper
    {
        public static T Deserialize<T>(string xml)
        {
            StringReader textReader = new StringReader(xml);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T) serializer.Deserialize(textReader);
        }

        public static string Serialise<T>(T t)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter textWriter = new StreamWriter(stream, Encoding.GetEncoding("GBK"));
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            serializer.Serialize(textWriter, t, namespaces);
            string str = Encoding.GetEncoding("GBK").GetString(stream.ToArray()).Replace("\r", "").Replace("\n", "");
            while (str.Contains(" <"))
            {
                str = str.Replace(" <", "<");
            }
            return str.Substring(str.IndexOf("?>") + 2);
        }
    }
}

