using System;
using System.Data;
using System.IO;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Store;
namespace Hidistro.UI.Common.Controls
{
    [ParseChildren(true), PersistChildren(false)]
    public abstract class VWeiXinOAuthTemplatedWebControl : VshopTemplatedWebControl
    {
        static readonly bool isWxLogger = false; //微信日志开关

        protected VWeiXinOAuthTemplatedWebControl()
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);

            string currentRequestUrl = HttpContext.Current.Request.Url.ToString();
            currentRequestUrl = System.Text.RegularExpressions.Regex.Replace(currentRequestUrl, "[\f\n\r\t\v]", "");

                #region
                WeiXinOAuthAttribute oAuth2Attr = Attribute.GetCustomAttribute(this.GetType(), typeof(WeiXinOAuthAttribute)) as WeiXinOAuthAttribute;

                WxLogger("*****************请求进入会员中心*****************");

                WxLogger("********Request.Url****" + currentRequestUrl);
                WxLogger("********ReferralId****" + this.Page.Request.QueryString["ReferralId"]);

                //WxLogger("Cookies:" + Globals.GetCurrentMemberUserId());

                MemberInfo currentMember = null;

                currentMember = MemberProcessor.GetCurrentMember();
                WxLogger("调式Vshop-Member-COOKIE:" + Globals.GetCurrentMemberUserId().ToString());
                WxLogger("调式currentMember:" + ((currentMember==null)?"empty":"111"));

                

                if (currentMember != null)// || (this.Page.Session["userid"] == null || this.Page.Session["userid"].ToString() != currentMember.UserId.ToString())
                {
                    
                    //爽爽挝啡需求:店铺增加粉丝数
                    string storeId = HiCache.Get(string.Format("DataCache-sub-StoreId-{0}", currentMember.OpenId)) as string;
                    if (!string.IsNullOrEmpty(storeId))
                    {
                        ManagerHelper.addStoreFansCount(storeId.ToInt(), currentMember.UserId, 0);
                        HiCache.Remove(string.Format("DataCache-sub-StoreId-{0}", currentMember.OpenId));
                    }

                    /*爽爽挝啡的代理商系统跳转判断:如果aspnet_member表的topRegionId为0或者不为1,则表示未激活,跳转到非法页面*/
                    if (CustomConfigHelper.Instance.IsBuyerNeedsToBeActive)
                    {
                        if (currentMember.TopRegionId == 0 || currentMember.TopRegionId != 1)
                        {
                            GotoResourceNotFound("您尚未被授权登录后台采购系统");
                        }
                    }
                    WxLogger(string.Format("        状态信息：**用户“{0}”已登录，中止请求微信**", currentMember.UserName));

                    if (null != oAuth2Attr)
                    {
                        switch (oAuth2Attr.WeiXinOAuthPage)
                        {
                            case WeiXinOAuthPage.VLogin:
                            case WeiXinOAuthPage.VRegister:
                            case WeiXinOAuthPage.VUserLogin:
                                {
                                    string tUrl = (string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralId"]))
                                        ? "~/vshop/MemberCenter.aspx" : "~/vshop/MemberCenter.aspx?ReferralId=" + this.Page.Request.QueryString["ReferralId"];
                                    Page.Response.Redirect(tUrl, true);
                                    break;
                                }
                            case WeiXinOAuthPage.VMemberCenter:
                                {
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                    return;
                }

                //读取配置信息.
                if (masterSettings.IsValidationService) //是否启用微信登录
                {

                    string code = this.Page.Request.QueryString["code"];

                    if (!string.IsNullOrEmpty(code))
                    {

                        WxLogger("      状态信息：**从微信网关授权回来**");

                        WxLogger("      code：" + code);

                        #region 取到了code,说明用户同意了授权登录

                        string responseResult = this.GetResponseResult("https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + masterSettings.WeixinAppId + "&secret=" + masterSettings.WeixinAppSecret + "&code=" + code + "&grant_type=authorization_code");

                        WxLogger("      获取令牌：" + responseResult);

                        if (responseResult.Contains("access_token"))
                        {

                            JObject obj2 = JsonConvert.DeserializeObject(responseResult) as JObject;

                            string openId = obj2["openid"].ToString();//微信用户ＯＰＥＮＩＤ
                            if (1==1)
                            {

                                string wxUserInfoStr = this.GetResponseResult("https://api.weixin.qq.com/sns/userinfo?access_token=" + obj2["access_token"].ToString() + "&openid=" + obj2["openid"].ToString() + "&lang=zh_CN");

                                WxLogger("      用户信息：" + wxUserInfoStr);

                                if (wxUserInfoStr.Contains("nickname"))
                                {

                                    JObject wxUserInfo = JsonConvert.DeserializeObject(wxUserInfoStr) as JObject;

                                    if (this.SkipWinxinOpenId(Globals.UrlDecode(wxUserInfo["nickname"].ToString()), wxUserInfo["openid"].ToString(), wxUserInfo["headimgurl"].ToString(), Page.Request["state"], 0))
                                    {
                                        WxLogger("      状态信息：**微信绑定登录成功**" + currentRequestUrl);

                                        this.Page.Response.Redirect(currentRequestUrl);
                                    }
                                    else
                                    {
                                        WxLogger("      状态信息：**微信绑定登录失败**");

                                        this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/index.aspx");
                                    }

                                }
                                else
                                {

                                    WxLogger("      状态信息：**微信登录失败**");

                                    this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/index.aspx");

                                }


                            }
                            else
                            {
                                
                            }


                        }
                        else
                        {

                            WxLogger("      状态信息：**获取信息失败**");

                            this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/index.aspx");

                        }

                        #endregion


                    }
                    else if (!string.IsNullOrEmpty(this.Page.Request.QueryString["state"]))
                    {
                        this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/index.aspx");

                    }
                    else
                    {

                        #region 跳转到微信登录

                        WxLogger("      状态信息：**到微信网关授权**");

                        string state = "";

                        if (System.Web.HttpContext.Current.Request.Cookies["Vshop-ReferralId"] != null)
                        {
                            state = System.Web.HttpContext.Current.Request.Cookies.Get("Vshop-ReferralId").Value;
                        }

                        WxLogger("      state：" + state);

                        string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + masterSettings.WeixinAppId + "&redirect_uri=" + Globals.UrlEncode(currentRequestUrl) + "&response_type=code&scope=snsapi_userinfo&state=" + (string.IsNullOrWhiteSpace(state) ? "STATE" : state) + "#wechat_redirect";

                        /*url = (string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralId"]))
                                        ? url :url+ "&ReferralId=" + this.Page.Request.QueryString["ReferralId"];*/
                        /*url = (string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralId"]) || url.ToLower().IndexOf("referralid") > -1)
                                        ? url : url + "&ReferralId=" + this.Page.Request.QueryString["ReferralId"];*/
                        WxLogger("      状态信息：**测试**" + url);

                        //这里是微信入口
                        this.Page.Response.Redirect(url, true);

                        #endregion

                    }
                }
                else
                {

                    //if (!string.IsNullOrEmpty(masterSettings.WeixinLoginUrl))
                    //{
                    //    WxLogger("      状态信息：**跳转到通用登陆接口" + masterSettings.WeixinLoginUrl + "**");
                    //    this.Page.Response.Redirect(masterSettings.WeixinLoginUrl);
                    //}
                    //else
                    //{

                    WxLogger("      状态信息：**跳转到通用登陆接口" + masterSettings.WeixinLoginUrl + "**");

                    #region 加上尾巴

                    int ReferralUserId = (null == currentMember ? 0 : currentMember.ReferralUserId);// GetReferralUserId();

                    //跳转过来的URL
                    Uri urlReferrer = HttpContext.Current.Request.UrlReferrer;

                    string returnUrl = currentRequestUrl;

                    if (ReferralUserId > 0 && returnUrl.Contains("?"))
                    {
                        returnUrl += "&ReferralUserId=" + ReferralUserId.ToString();
                    }
                    //else
                    //{
                    //    returnUrl += "?ReferralUserId=" + ReferralUserId.ToString();
                    //}

                    #endregion

                    // this.Page.Response.Redirect("Login.aspx?returnUrl=" + Globals.UrlEncode(returnUrl));


                    if (null != oAuth2Attr)
                    {
                        switch (oAuth2Attr.WeiXinOAuthPage)
                        {
                            case WeiXinOAuthPage.VLogin:
                            case WeiXinOAuthPage.VMemberCenter:
                                {
                                    this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/UserLogin.aspx?returnUrl=" + Globals.UrlEncode(returnUrl));
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }

                }
                #endregion


            


            // }



        }


        
        /// <summary>        /// 判断用户有没有关注公众号        /// </summary>        /// <returns></returns>        public bool WxSubscribe(string openid)        {
            //开启微信才开始判断
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);            if (!masterSettings.IsValidationService)                return true;

            //获取access_token

            string responseResult = this.GetResponseResult("https://api.weixin.qq.com/cgi-bin/token?appid=" + masterSettings.WeixinAppId + "&secret=" + masterSettings.WeixinAppSecret + "&grant_type=client_credential");
            if (responseResult.Contains("access_token"))
            {
                JObject obj2 = JsonConvert.DeserializeObject(responseResult) as JObject;
                string wxUserInfoStr = this.GetResponseResult("https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + obj2["access_token"].ToString() + "&openid=" + openid + "&lang=zh_CN");
                if (wxUserInfoStr.Contains("subscribe"))
                {
                    JObject wxUserInfo = JsonConvert.DeserializeObject(wxUserInfoStr) as JObject;

                    if (Convert.ToInt32(wxUserInfo["subscribe"].ToString()) != 0)
                    {
                        return true;
                    }
                }
            }            return false;        }        /// <summary>

        /// <summary>
        /// 获取推荐人ID
        /// </summary>
        /// <returns></returns>
        string GetReferralUserId()
        {
            string ReferralUserId = "";

            try
            {
                //跳转过来的URL
                Uri urlReferrer = HttpContext.Current.Request.UrlReferrer;

                if (null != urlReferrer && !string.IsNullOrWhiteSpace(urlReferrer.Query))
                {
                    string querystr = "";
                    if (urlReferrer.Query.StartsWith("?")) querystr = urlReferrer.Query.Substring(1);

                    foreach (string item in querystr.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (item.Contains("ReferralUserId"))
                        {
                            ReferralUserId = item.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1];
                            break;
                        }
                    }
                }
            }
            catch { }

            return ReferralUserId;
        }

        private string GetResponseResult(string url)
        {
            using (HttpWebResponse response = (HttpWebResponse)WebRequest.Create(url).GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }


        bool SkipWinxinOpenId(string userName, string openId, string headimgurl, string state,int t)
        {

            WxLogger("      状态信息：**　进入　SkipWinxinOpenId　函数体 **");

            bool flag = false;
            try
            {
                string generateId = Globals.GetGenerateId();

                MemberInfo member = new MemberInfo();

                member.GradeId = MemberProcessor.GetDefaultMemberGrade();
                member.UserName = userName;
                member.RealName = userName;
                if (t == 0)
                {
                    member.OpenId = openId;
                    member.RegSource = 0;//微信注册
                }else if(t==1){
                    member.AliOpenId = openId;
                    member.RegSource = 1;//支付宝
                }
                member.CreateDate = DateTime.Now;
                member.SessionId = generateId;
                member.SessionEndTime = DateTime.Now.AddYears(10);
                member.Email = generateId + "@localhost.com";
                member.SessionId = generateId;
                member.Password = generateId;
                member.RealName = string.Empty;
                member.Address = string.Empty;
                member.UserHead = headimgurl;//用户头像
                #region 设置推荐人
                if (!string.IsNullOrWhiteSpace(state))
                {
                    int referralUserId = 0;
                    if (int.TryParse(state, out referralUserId))
                    {
                        member.ReferralUserId = referralUserId;
                    }
                }

                //System.IO.File.AppendAllText(HiContext.Current.Context.Request.MapPath("~/ReferralUserId.txt"), "用户名：" + userName + ";ReferralUserId＝" + member.ReferralUserId + ";openid=" + openId + Environment.NewLine);

                #endregion

                WxLogger(" 调式A **" + member.OpenId);

                if (MemberProcessor.CreateMember(member))
                {
                    //MemberProcessor.GetusernameMember(Globals.UrlDecode(userName));
                    //MemberProcessor.GetusernameMember(Globals.UrlDecode(obj3["nickname"].ToString()));
                    //System.IO.File.AppendAllText(Page.Request.MapPath("~/wx.txt"), "***用户创建成功了***" + Environment.NewLine);//获取到openid
                    WxLogger(" 调式b**" + member.OpenId);
                    IList<MemberInfo> memberLst = Hidistro.ControlPanel.Members.MemberHelper.GetMemdersByOpenIds("'" + openId + "'",t);

                    HttpCookie cookie = new HttpCookie("Vshop-Member");

                    cookie.Value = memberLst[0].UserId.ToString();

                    cookie.Expires = DateTime.Now.AddYears(10);

                    HttpContext.Current.Response.Cookies.Add(cookie);
                    WxLogger(" 调式c**" + Globals.GetCurrentMemberUserId().ToString());

                    /*
                    DistributorsInfo userIdDistributors = new DistributorsInfo();

                    userIdDistributors = DistributorsBrower.GetUserIdDistributors(member.UserId);

                    if ((userIdDistributors != null) && (userIdDistributors.UserId > 0))
                    {
                        HttpCookie cookie2 = new HttpCookie("Vshop-ReferralId")
                        {
                            Value = userIdDistributors.UserId.ToString(),
                            Expires = DateTime.Now.AddYears(1)
                        };
                        HttpContext.Current.Response.Cookies.Add(cookie2);
                    }*/

                    flag = true;

                }

            }
            catch (Exception ex)
            {

                WxLogger("      异常信息：** SkipWinxinOpenId()函数调用引发的异常：" + ex.Message + "**");

            }


            return flag;
        }



        /// <summary>
        /// 微信日志
        /// </summary>
        /// <param name="log"></param>
        void WxLogger(string log)
        {

            if (!isWxLogger) return;

            string logFile = Page.Request.MapPath("~/wx_login.txt");

            File.AppendAllText(logFile, string.Format("{0}:{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), log));

        }

        /// <summary>
        /// 判断用户有没有关注公众号
        /// </summary>
        /// <returns></returns>
        private bool checkWxEx(string openid)
        {
            WxLogger("      checkWx：");
            HttpCookie cookie = HttpContext.Current.Request.Cookies["wx_subscribe"];
            if ((cookie == null) || string.IsNullOrEmpty(cookie.Value))
            {
                //读取配置信息
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                WxLogger("      checkWx：" + masterSettings.GuidePageSet);
                if (masterSettings.IsValidationService && masterSettings.GuidePageSet != "") //是否启用微信登录
                {

                        #region 取到了code,说明用户同意了授权登录

                    string responseResult = this.GetResponseResult("https://api.weixin.qq.com/cgi-bin/token?appid=" + masterSettings.WeixinAppId + "&secret=" + masterSettings.WeixinAppSecret + "&grant_type=client_credential");
                        WxLogger("      responseResult：" + responseResult);
                        if (responseResult.Contains("access_token"))
                        {
                            WxLogger("      得到token：" );
                            JObject obj2 = JsonConvert.DeserializeObject(responseResult) as JObject;
                            //string openId = obj2["openid"].ToString();//微信用户ＯＰＥＮＩＤ
                            //WxLogger("      openId_openId：" + openId);
                            string wxUserInfoStr = this.GetResponseResult("https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + obj2["access_token"].ToString() + "&openid=" + openid + "&lang=zh_CN");
                            WxLogger("      wxUserInfoStr：" + wxUserInfoStr);
                            if (wxUserInfoStr.Contains("subscribe"))
                            {
                                WxLogger("      checkWx_openId：" + openid);
                                JObject wxUserInfo = JsonConvert.DeserializeObject(wxUserInfoStr) as JObject;
                                WxLogger("      subscribe：" + wxUserInfo["subscribe"]);
                                if (Convert.ToInt32(wxUserInfo["subscribe"]) != 0)
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/index.aspx");

                            }

                        }


                        #endregion

                }
                else
                {
                    return true;
                }
            }
            else
            {
                WxLogger("      cookie：" + cookie.Value);
                return true;
            }
            return false;
        }

    }
}

