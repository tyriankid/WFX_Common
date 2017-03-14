﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hishop.Weixin.MP.Api;
using Newtonsoft.Json;

namespace Hidistro.UI.Common.Controls
{
    public class WeixinSet : Literal
    {
        public string htmlAppID = string.Empty;
        public string htmlToken = string.Empty;
        public string htmlNonceStr = "QoN4FvGbxdTi7mnffL";
        public string htmlTimeStamp = string.Empty;
        public string htmlSignature = string.Empty;
        public string htmlstring1 = string.Empty;
        public string htmlTicket = string.Empty;
        protected override void Render(HtmlTextWriter writer)
        {
            base.Text = "";
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            this.htmlAppID = masterSettings.WeixinAppId;
            string weixinAppSecret = masterSettings.WeixinAppSecret;
            try
            {
                this.htmlToken = this.GetToken(this.htmlAppID, weixinAppSecret);
            }
            catch (Exception)
            {
            }
            this.htmlTimeStamp = WeixinSet.ConvertDateTimeInt(DateTime.Now).ToString();
            this.htmlSignature = this.GetSignature(this.htmlToken, this.htmlTimeStamp, this.htmlNonceStr, out this.htmlstring1);
            string token = TokenApi.GetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
            MenuApi.GetMenus(token);
            base.Text = string.Concat(new string[]
			{
				"<script>wx.config({ debug: false,appId: '",
				this.htmlAppID,
				"',timestamp: '",
				this.htmlTimeStamp,
				"', nonceStr: '",
				this.htmlNonceStr,
				"',signature: '",
				this.htmlSignature,
				"',jsApiList: ['checkJsApi','onMenuShareTimeline','onMenuShareAppMessage','onMenuShareQQ','onMenuShareWeibo','getLocation']});</script>"
			});
            base.Render(writer);
        }
        public string GetSignature(string token, string timestamp, string nonce, out string str)
        {
            string str2 = this.Page.Request.Url.ToString();
            string jsApi_ticket = this.GetJsApi_ticket(token);
            this.htmlTicket = jsApi_ticket;
            string text = "jsapi_ticket=" + jsApi_ticket;
            string text2 = "noncestr=" + nonce;
            string text3 = "timestamp=" + timestamp;
            string text4 = "url=" + str2;
            string[] value = new string[]
			{
				text,
				text2,
				text3,
				text4
			};
            str = string.Join("&", value);
            string text5 = str;
            text5 = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1");
            return text5.ToLower();
        }
        public string GetJsApi_ticket(string token)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", token);
            string value = this.DoGet(url);
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
            if (dictionary != null && dictionary.ContainsKey("ticket"))
            {
                return dictionary["ticket"];
            }
            return string.Empty;
        }
        public string GetToken(string appid, string secret)
        {
           string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret);
           string value = this.DoGet(url);
           if (string.IsNullOrEmpty(value))
           {
               return string.Empty;
           }
           Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
           if (dictionary != null && dictionary.ContainsKey("access_token"))
           {
               return dictionary["access_token"];
           }
           return string.Empty;
        }
        public string DoGet(string url)
        {
            HttpWebRequest webRequest = this.GetWebRequest(url, "GET");
            webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            HttpWebResponse rsp = (HttpWebResponse)webRequest.GetResponse();
            Encoding uTF = Encoding.UTF8;
            return this.GetResponseAsString(rsp, uTF);
        }
        public string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            Stream stream = null;
            StreamReader streamReader = null;
            string result;
            try
            {
                stream = rsp.GetResponseStream();
                streamReader = new StreamReader(stream, encoding);
                result = streamReader.ReadToEnd();
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }
                if (stream != null)
                {
                    stream.Close();
                }
                if (rsp != null)
                {
                    rsp.Close();
                }
            }
            return result;
        }
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
        public HttpWebRequest GetWebRequest(string url, string method)
        {
            int timeout = 100000;
            HttpWebRequest httpWebRequest;
            if (url.Contains("https"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.CheckValidationResult);
                httpWebRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            httpWebRequest.ServicePoint.Expect100Continue = false;
            httpWebRequest.Method = method;
            httpWebRequest.KeepAlive = true;
            httpWebRequest.UserAgent = "Hishop";
            httpWebRequest.Timeout = timeout;
            return httpWebRequest;
        }
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long ticks = long.Parse(timeStamp + "0000000");
            TimeSpan value = new TimeSpan(ticks);
            return dateTime.Add(value);
        }
        public static int ConvertDateTimeInt(DateTime time)
        {
            DateTime d = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(time - d).TotalSeconds;
        }
        static WeixinSet()
        {
        }
    }
}
