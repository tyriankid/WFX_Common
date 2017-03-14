using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Hishop.Weixin.MP.Api
{
    /// <summary>
    /// 带参数的二维码处理  add:jhb_20151206
    /// 参考文档：微信公众平台开发者文档 http://mp.weixin.qq.com/wiki/18/28fc21e7ed87bec960651f0ce873ef8a.html
    /// </summary>
    public class TicketAPI
    {
        private const string TEMP_JSON_DATA = @"{""expire_seconds"":1800, ""action_name"": ""QR_SCENE"", ""action_info"": {""scene"": {""scene_id"":""{0}""}}}";//只能接受整形
        private const string FIXED_JSON_DATA = @"{""action_name"": ""QR_LIMIT_SCENE"", ""action_info"": {""scene"": {""scene_id"":""{0}""}}}";//只能接受整形
        private const string FIXED_JSON_DATA2 = @"{""action_name"": ""QR_LIMIT_STR_SCENE"", ""action_info"": {""scene"": {""scene_str"":""{0}""}}}";//接受字符串

        /// <summary>
        /// 创建二维码ticket
        /// </summary>
        public static string CreateTicket(string WeixinAppId, string WeixinAppSecret, string ticketID, bool isTemp = true)
        {
            string token = TokenApi.GetToken(WeixinAppId, WeixinAppSecret);
            JObject obj2 = JsonConvert.DeserializeObject(token) as JObject;
            if (obj2 != null && obj2.ToString().Contains("access_token"))
            {
                token = obj2["access_token"].ToString();
            }
            string url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + token;
            //WxLogger(ticketID);
            string json = (isTemp) ? TEMP_JSON_DATA : FIXED_JSON_DATA2;
            json = json.Replace("{0}", ticketID);
            //WxLogger(json);
            string strTICKET = new Hishop.Weixin.MP.Util.WebUtils().DoPost(url, json);
            JObject obj3 = JsonConvert.DeserializeObject(strTICKET) as JObject;
            return obj3["ticket"].ToString();
        }

        /// <summary>
        /// 获取二维码
        /// </summary>
        public static string GetTicketImage(string WeixinAppId, string WeixinAppSecret, string ticketID, bool isTemp = true)
        {
            string TICKET = CreateTicket(WeixinAppId, WeixinAppSecret, ticketID, isTemp);
            return GetTicketImage(TICKET, ticketID);
        }

        /// <summary>
        /// 获取二维码
        /// </summary>
        public static string GetTicketImage(string TICKET, string ticketID)
        {
            string strpath = string.Empty;
            string savepath = string.Empty;
            string stUrl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + HttpContext.Current.Server.UrlEncode(TICKET);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(stUrl);
            req.Method = "GET";
            using (WebResponse wr = req.GetResponse())
            {
                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();
                strpath = myResponse.ResponseUri.ToString();
                WebClient mywebclient = new WebClient();
                //savepath = HttpContext.Current.Server.MapPath("~/Storage/temp") + "\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + (new Random()).Next().ToString().Substring(0, 4) + "." + myResponse.ContentType.Split('/')[1].ToString();
                savepath = HttpContext.Current.Server.MapPath("~/Storage/TicketImage") + "\\" + ticketID + "." + myResponse.ContentType.Split('/')[1].ToString();
                if (!Directory.Exists(Path.GetDirectoryName(savepath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(savepath));
                }
                try
                {
                    mywebclient.DownloadFile(strpath, savepath);
                }
                catch (Exception ex)
                {
                    savepath = ex.ToString();
                }
            }
            return strpath.ToString();
        }

        /// <summary>
        /// 微信日志(临时)
        /// </summary>
        static void WxLogger(string log)
        {
            string logFile = @"D:\Project\WebSite\WFX_Yihui\wx_.txt";
            System.IO.File.AppendAllText(logFile, string.Format("{0}:{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), log));
        }

    }
}
