namespace Hidistro.SaleSystem.Vshop
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System;
    using System.IO;
    using System.Text;
    using System.Runtime.Serialization.Json;

    public class JsonHelper
    {
        /// <summary>  
        /// 生成Json格式  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="obj"></param>  
        /// <returns></returns>  
        public static string GetJson<T>(T obj)
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                json.WriteObject(stream, obj);
                string szJson = Encoding.UTF8.GetString(stream.ToArray()); return szJson;
            }
        }
        /// <summary>  
        /// 获取Json的Model  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="szJson"></param>  
        /// <returns></returns>  
        public static T ParseFromJson<T>(string szJson)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(ms);
            }
        }

        /// <summary>
        /// Json数据转数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonText"></param>
        /// <returns></returns>
        public static List<T> JsonToList<T>(string jsonText)
        {
            List<T> list = new List<T>();

            DataContractJsonSerializer _Json = new DataContractJsonSerializer(list.GetType());
            byte[] _Using = System.Text.Encoding.UTF8.GetBytes(jsonText);
            System.IO.MemoryStream _MemoryStream = new System.IO.MemoryStream(_Using);
            _MemoryStream.Position = 0;

            return (List<T>)_Json.ReadObject(_MemoryStream);
        }
    }
}

