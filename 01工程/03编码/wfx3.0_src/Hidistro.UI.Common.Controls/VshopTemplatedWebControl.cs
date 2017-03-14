using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;

namespace Hidistro.UI.Common.Controls
{
    [ParseChildren(true), PersistChildren(false)]
    public abstract class VshopTemplatedWebControl : TemplatedWebControl
    {

        protected int referralId;
        private string RefrrealKey = "Vshop-ReferralId";
        private string skinName;

        protected VshopTemplatedWebControl()
        {

        }


        private string ControlText()
        {
            if (!this.SkinFileExists)
            {
                return null;
            }
            StringBuilder builder = new StringBuilder(File.ReadAllText(this.Page.Request.MapPath(this.SkinPath), Encoding.UTF8));
            if (builder.Length == 0)
            {
                return null;
            }
            builder.Replace("<%", "").Replace("%>", "");
            string vshopSkinPath = Globals.GetVshopSkinPath(null);
            builder.Replace("/images/", vshopSkinPath + "/images/");
            builder.Replace("/script/", vshopSkinPath + "/script/");
            builder.Replace("/style/", vshopSkinPath + "/style/");
            builder.Replace("/utility/", Globals.ApplicationPath + "/utility/");
            builder.Insert(0, "<%@ Register TagPrefix=\"UI\" Namespace=\"ASPNET.WebControls\" Assembly=\"ASPNET.WebControls\" %>" + Environment.NewLine);
            builder.Insert(0, "<%@ Register TagPrefix=\"Kindeditor\" Namespace=\"kindeditor.Net\" Assembly=\"kindeditor.Net\" %>" + Environment.NewLine);
            builder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.Common.Validator\" Assembly=\"Hidistro.UI.Common.Validator\" %>" + Environment.NewLine);
            builder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.Common.Controls\" Assembly=\"Hidistro.UI.Common.Controls\" %>" + Environment.NewLine);
            builder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.SaleSystem.Tags\" Assembly=\"Hidistro.UI.SaleSystem.Tags\" %>" + Environment.NewLine);
            builder.Insert(0, "<%@ Control Language=\"C#\" %>" + Environment.NewLine);
            MatchCollection matchs = Regex.Matches(builder.ToString(), "href(\\s+)?=(\\s+)?\"url:(?<UrlName>.*?)(\\((?<Param>.*?)\\))?\"", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            for (int i = matchs.Count - 1; i >= 0; i--)
            {
                int startIndex = matchs[i].Groups["UrlName"].Index - 4;
                int length = matchs[i].Groups["UrlName"].Length + 4;
                if (matchs[i].Groups["Param"].Length > 0)
                {
                    length += matchs[i].Groups["Param"].Length + 2;
                }
                builder.Remove(startIndex, length);
                builder.Insert(startIndex, Globals.GetSiteUrls().UrlData.FormatUrl(matchs[i].Groups["UrlName"].Value.Trim(), new object[] { matchs[i].Groups["Param"].Value }));
            }
            return builder.ToString();
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralId"]) && int.TryParse(this.Page.Request.QueryString["ReferralId"], out this.referralId))
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-Member"];
                if (((cookie != null) && !string.IsNullOrEmpty(cookie.Value)) && (this.referralId != Convert.ToInt32(cookie.Value)))
                {
                    DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(Convert.ToInt16(cookie.Value));
                    if ((currentDistributors != null) && (currentDistributors.UserId > 0))
                    {
                        cookie.Value = null;
                        cookie.Expires = DateTime.Now.AddYears(-1);
                        HttpContext.Current.Response.Cookies.Set(cookie);
                    }
                }
                HttpCookie cookie2 = new HttpCookie(this.RefrrealKey)
                {
                    Value = null,
                    Expires = DateTime.Now.AddYears(-1)
                };
                HttpContext.Current.Response.Cookies.Set(cookie2);
                cookie2.Value = this.referralId.ToString();
                cookie2.Expires = DateTime.Now.AddYears(1);
                HttpContext.Current.Response.Cookies.Add(cookie2);
            }

            //关注推送处理
            int iVisitDistributorID = 0;
            HttpCookie cookie3 = HttpContext.Current.Request.Cookies[this.RefrrealKey];
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember != null && !string.IsNullOrEmpty(currentMember.OpenId))
            {
                //WriteLog("测试1：" + currentMember.OpenId + "_" + currentMember.UserId + "_" + currentMember.UserName);
                string VisitDistributorID = HiCache.Get(string.Format("DataCache-VisitDistributor-{0}", currentMember.OpenId)) as string;
                //WriteLog("当前推送码：" + VisitDistributorID);
                if (!string.IsNullOrEmpty(VisitDistributorID) && DistributorsBrower.GetDistributorInfo(currentMember.UserId) == null)
                {
                    //WriteLog("进入推送：" + VisitDistributorID);
                    int.TryParse(VisitDistributorID, out iVisitDistributorID);
                    if (cookie3 == null)
                    {
                        cookie3 = new HttpCookie(this.RefrrealKey);
                    }
                    cookie3.Value = VisitDistributorID;
                    cookie3.Expires = DateTime.Now.AddYears(1);
                    HttpContext.Current.Response.Cookies.Add(cookie3);
                    HiCache.Remove(string.Format("DataCache-VisitDistributor-{0}", currentMember.OpenId));
                    if (iVisitDistributorID == 0)//增加对总店的处理
                    {
                        cookie3.Value = null;
                        cookie3.Expires = DateTime.Now.AddYears(-1);
                        HttpContext.Current.Response.Cookies.Set(cookie3);
                    }
                }
            }

            //HttpCookie cookie3 = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
            if ((cookie3 != null) && !string.IsNullOrEmpty(cookie3.Value))
            {
                //当前登录者有店铺时，ReferralId为自己
                DistributorsInfo info2 = DistributorsBrower.GetCurrentDistributors(Convert.ToInt16(cookie3.Value));
                if ((info2 == null) || (info2.UserId <= 0))
                {
                    //WxLogger("清除访问店铺COOKIE");
                    cookie3.Value = null;
                    cookie3.Expires = DateTime.Now.AddYears(-1);
                    HttpContext.Current.Response.Cookies.Set(cookie3);
                    HttpCookie cookie4 = HttpContext.Current.Request.Cookies["Vshop-Member"];
                    if (cookie4 != null)
                    {
                        cookie4.Value = null;
                        cookie4.Expires = DateTime.Now.AddYears(-1);
                        HttpContext.Current.Response.Cookies.Set(cookie4);
                    }
                    HttpContext.Current.Response.Redirect("/Vshop/UserLogin.aspx");
                }
            }

            //访问店铺为总店时( Cookies["Vshop-ReferralId"]为空 ) , 判断自己是否有店铺
            if (currentMember != null && !string.IsNullOrEmpty(currentMember.OpenId)
                && DistributorsBrower.GetDistributorInfo(currentMember.UserId) != null)
            {
                if (cookie3 == null) cookie3 = new HttpCookie(this.RefrrealKey);
                cookie3.Value = currentMember.UserId.ToString();
                cookie3.Expires = DateTime.Now.AddYears(1);
                HttpContext.Current.Response.Cookies.Add(cookie3);
            }

            if (!this.LoadHtmlThemedControl())
            {
                throw new SkinNotFoundException(this.SkinPath);
            }
            this.AttachChildControls();
        }

        /// <summary>
        /// 微信日志
        /// </summary>
        /// <param name="log"></param>
        void WxLogger(string log)
        {
            string logFile = @"D:\Project\WebSite\WFX_Yihui\wx_.txt";
            File.AppendAllText(logFile, string.Format("{0}:{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), log));
        }
        

        private string GenericReloadUrl(NameValueCollection queryStrings)
        {
            if ((queryStrings == null) || (queryStrings.Count == 0))
            {
                return this.Page.Request.Url.AbsolutePath;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(this.Page.Request.Url.AbsolutePath).Append("?");
            foreach (string str2 in queryStrings.Keys)
            {
                if (queryStrings[str2] != null)
                {
                    string str = queryStrings[str2].Trim();
                    if (!string.IsNullOrEmpty(str) && (str.Length > 0))
                    {
                        builder.Append(str2).Append("=").Append(this.Page.Server.UrlEncode(str)).Append("&");
                    }
                }
            }
            queryStrings.Clear();
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        public string getUrl()
        {
            string str = HttpContext.Current.Request.Url.PathAndQuery.ToString();
            int startIndex = str.LastIndexOf("/") + 1;
            int length = (str.IndexOf(".aspx") - str.LastIndexOf("/")) - 1;
            return str.Substring(startIndex, length);
        }

        protected void GotoResourceNotFound(string errorMsg = "")
        {
            this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + errorMsg);
        }

        protected bool LoadHtmlThemedControl()
        {
            string str = this.ControlText();
            if (!string.IsNullOrEmpty(str))
            {
                Control child = this.Page.ParseControl(str);
                child.ID = "_";
                this.Controls.Add(child);
                return true;
            }
            return false;
        }

        public void RegisterShareScript(string ImgUrl, string lineLink, string descContent, string shareTitle)
        {
            string weixinAppId = SettingsManager.GetMasterSettings(true).WeixinAppId;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<script language=\"javascript\" type=\"text/javascript\">");
            builder.AppendLine("var wxShareImgUrl = '" + ImgUrl + "';");
            builder.AppendLine("var wxSharelineLink = '" + lineLink + "';");
            builder.AppendLine("var wxSharedescContent = '" + (string.IsNullOrEmpty(descContent) ? "" : descContent.Replace("'", "")) + "';");
            builder.AppendLine("var wxShareshareTitle = '" + shareTitle + "';");
            builder.AppendLine("var wxappid = '" + weixinAppId + "';");
            builder.AppendLine("var wxShareImgWidth = '220';");
            builder.AppendLine("var wxShareImgHeight = '220';");
            builder.AppendLine("function weixinShareTimeline(){");
            builder.AppendLine("WeixinJSBridge.invoke('shareTimeline',{");
            builder.AppendLine("\"img_url\":wxShareImgUrl,");
            builder.AppendLine("\"img_width\":wxShareImgWidth,");
            builder.AppendLine("\"img_height\": wxShareImgHeight,");
            builder.AppendLine("\"link\": wxSharelineLink,");
            builder.AppendLine("\"desc\": wxSharedescContent,");
            builder.AppendLine("\"title\": wxShareshareTitle");
            builder.AppendLine("});");
            builder.AppendLine("}");
            builder.AppendLine("function wxshareFriend() {");
            builder.AppendLine("WeixinJSBridge.invoke('sendAppMessage', {");
            builder.AppendLine("\"appid\": wxappid,");
            builder.AppendLine("\"img_url\": wxShareImgUrl,");
            builder.AppendLine("\"img_width\": wxShareImgWidth,");
            builder.AppendLine("\"img_height\": wxShareImgHeight,");
            builder.AppendLine("\"link\": wxSharelineLink,");
            builder.AppendLine("\"desc\": wxSharedescContent,");
            builder.AppendLine("\"title\": wxShareshareTitle");
            builder.AppendLine("}, function (res) {");
            builder.AppendLine("})");
            builder.AppendLine("}");
            builder.AppendLine("document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {");
            builder.AppendLine("WeixinJSBridge.on('menu:share:appmessage', function (argv) {");
            builder.AppendLine("wxshareFriend();");
            builder.AppendLine("});");
            builder.AppendLine("WeixinJSBridge.on('menu:share:timeline', function (argv) {");
            builder.AppendLine("weixinShareTimeline();");
            builder.AppendLine("});");
            builder.AppendLine("},false);");
            builder.AppendLine("</script>");
            HttpContext.Current.Response.Write(builder.ToString());
        }

        public void ReloadPage(NameValueCollection queryStrings)
        {
            this.Page.Response.Redirect(this.GenericReloadUrl(queryStrings));
        }

        public void ReloadPage(NameValueCollection queryStrings, bool endResponse)
        {
            this.Page.Response.Redirect(this.GenericReloadUrl(queryStrings), endResponse);
        }

        private bool SkinFileExists
        {
            get
            {
                return !string.IsNullOrEmpty(this.SkinName);
            }
        }

        public virtual string SkinName
        {
            get
            {
                return this.skinName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.ToLower(CultureInfo.InvariantCulture);
                    if (value.EndsWith(".html"))
                    {
                        this.skinName = value;
                    }
                }
            }
        }

        protected virtual string SkinPath
        {
            get
            {
                string vshopSkinPath = Globals.GetVshopSkinPath(null);
                if (this.SkinName.StartsWith(vshopSkinPath))
                {
                    return this.SkinName;
                }
                if (this.SkinName.StartsWith("/"))
                {
                    return (vshopSkinPath + this.SkinName);
                }
                return (vshopSkinPath + "/" + this.SkinName);
            }
        }
        
    }
}

