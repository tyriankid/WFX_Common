using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using Hidistro.Core.Entities;
using Hidistro.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hidistro.UI.Common.Controls
{
    /// <summary>
    ///Access_token 的摘要说明
    /// </summary>
    public class Access_token
    {
        string _access_token;
        string _expires_in="7200";

        /// <summary>
        /// 获取到的凭证 
        /// </summary>
        public string access_token
        {
            get { return _access_token; }
            set { _access_token = value; }
        }

        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public string expires_in
        {
            get { return _expires_in; }
            set { _expires_in = value; }
        }

        /*
        access_token是变化的,有自己的周期,官方解释为:"有效期为7200秒",这就要 求我们把获得的access_token存入一个物理文件或者Application中,请求到过期后修改这些内容,需要用的时候读出来.
有些人可能想到了，如果过期我就在获得一个就好了,不用物理文件和Application也可以达到同样的效果,但是需要注意了微信平台对每天获得
access_token的次数也作了限制，一个用户出发多次，如果用户多，那肯定就超出了。所以我们还是按照以上的思路实现这些功能:
在此之前我们已经了解了获得access_token的方法(连接),现在只需要保证它的随时更新就好了.
    */

        /// <summary>
        /// 从服务器获取access_token
        /// </summary>
        private static Access_token GetAccess_tokenByServer(Access_Type type= Access_Type.weixin)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            string strUrl = "";
            switch (type)
            {
                case Access_Type.weixin:
                    //strUrl = "https://api.vdian.com/token?grant_type=client_credential&appkey=" + masterSettings.WeixinAppId + "&secret=" + masterSettings.WeixinAppSecret;
                    strUrl = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", masterSettings.WeixinAppId, masterSettings.WeixinAppSecret); 
                    break;
                case Access_Type.weidian:
                    //strUrl = "https://api.vdian.com/token?grant_type=client_credential&appkey=" + masterSettings.WeiDianAppId + "&secret=" + masterSettings.WeixinAppSecret;
                    break;
            }
            
            Access_token mode = new Access_token();
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strUrl);
            req.Method = "GET";
            using (WebResponse wr = req.GetResponse())
            {
                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();
                //在这里对Access_token 赋值
                //Access_token token = new Access_token();
                //token = JsonHelper.ParseFromJson<Access_token>(content);
                //token = JsonConvert.DeserializeObject<Access_token>(content);
                JObject obj2 = JsonConvert.DeserializeObject(content) as JObject;

                if (obj2 != null && obj2.ToString().Contains("result"))
                {
                    obj2 = JsonConvert.DeserializeObject(obj2["result"].ToString()) as JObject;
                    mode.access_token = obj2["access_token"].ToString();
                    mode.expires_in = obj2["expire_in"].ToString();
                }
                else if (obj2 != null && type == Access_Type.weixin)
                {
                    obj2 = JsonConvert.DeserializeObject(obj2.ToString()) as JObject;
                    mode.access_token = obj2["access_token"].ToString();
                    mode.expires_in = obj2["expires_in"].ToString();
                }
                
            }
            return mode;
        }

        /// <summary>
        /// 获取接口token
        /// </summary>
        /// <param name="type">接口类型</param>
        /// <param name="isForce">是否强刷</param>
        public static string GetAccess_token(Access_Type type = Access_Type.weixin, bool isForce = false)
        {

            string Token = string.Empty;
            DateTime YouXRQ;
            // 读取XML文件中的数据，并显示出来 ，注意文件路径
            string filepath = System.Web.HttpContext.Current.Server.MapPath("~/config/APIConfig.xml");
            StreamReader str = new StreamReader(filepath, System.Text.Encoding.UTF8);
            XmlDocument xml = new XmlDocument();
            xml.Load(str);
            str.Close();
            str.Dispose();
            Token = xml.SelectSingleNode("xml").SelectSingleNode(type.ToString()+"_Access_Token").InnerText;
            YouXRQ = Convert.ToDateTime(xml.SelectSingleNode("xml").SelectSingleNode(type+"_Access_YouXRQ").InnerText);

            if (isForce || DateTime.Now > YouXRQ)
            {
                DateTime _youxrq = DateTime.Now;
                Access_token mode = GetAccess_tokenByServer(type);
                xml.SelectSingleNode("xml").SelectSingleNode(type.ToString() + "_Access_Token").InnerText = mode.access_token;
                _youxrq = _youxrq.AddSeconds(int.Parse(mode.expires_in));
                xml.SelectSingleNode("xml").SelectSingleNode(type.ToString() + "_Access_YouXRQ").InnerText = _youxrq.ToString();
                xml.Save(filepath);
                Token = mode.access_token;
            }
            return Token;
        }

        /// <summary>
        /// 接口类型
        /// </summary>
        public enum Access_Type
        { 
            weixin=0,
            weidian=1,
        }

    }


}
