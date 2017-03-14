namespace Hidistro.Membership.Context
{
    using Hidistro.Core;
    using Hidistro.Core.Configuration;
    using Hidistro.Core.Enums;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Web;

    public sealed class HiContext
    {
        private HiConfiguration _config = null;
        private IUser _currentUser = null;
        private string _hostPath = null;
        private HttpContext _httpContext;
        private bool _isUrlReWritten = false;
        private NameValueCollection _queryString = null;
        private string _siteUrl = null;
        private Hidistro.Membership.Context.SiteSettings currentSettings;
        private const string dataKey = "Hishop_ContextStore";
        private string rolesCacheKey = null;
        private string verifyCodeKey = "VerifyCode";

        private HiContext(HttpContext context)
        {
            this._httpContext = context;
            this.Initialize(new NameValueCollection(context.Request.QueryString), context.Request.Url, context.Request.RawUrl, this.GetSiteUrl());
        }

        public bool CheckVerifyCode(string verifyCode)
        {
            if (HttpContext.Current.Request.Cookies[this.verifyCodeKey] == null)
            {
                this.RemoveVerifyCookie();
                return false;
            }
            bool flag = string.Compare(HiCryptographer.Decrypt(HttpContext.Current.Request.Cookies[this.verifyCodeKey].Value), verifyCode, true, CultureInfo.InvariantCulture) == 0;
            this.RemoveVerifyCookie();
            return flag;
        }

        public static HiContext Create(HttpContext context)
        {
            return Create(context, false);
        }

        public static HiContext Create(HttpContext context, UrlReWriterDelegate rewriter)
        {
            HiContext context2 = new HiContext(context);
            SaveContextToStore(context2);
            if (null != rewriter)
            {
                context2.IsUrlReWritten = rewriter(context);
            }
            return context2;
        }

        public static HiContext Create(HttpContext context, bool isReWritten)
        {
            HiContext context2 = new HiContext(context) {
                IsUrlReWritten = isReWritten
            };
            SaveContextToStore(context2);
            return context2;
        }

        public string CreateVerifyCode(int length)
        {
            string text = string.Empty;
            Random random = new Random();
            while (text.Length < length)
            {
                char ch;
                int num = random.Next();
                if ((num % 3) == 0)
                {
                    ch = (char) (0x61 + ((ushort) (num % 0x1a)));
                }
                else if ((num % 4) == 0)
                {
                    ch = (char) (0x41 + ((ushort) (num % 0x1a)));
                }
                else
                {
                    ch = (char) (0x30 + ((ushort) (num % 10)));
                }
                if (((((ch != '0') && (ch != 'o')) && ((ch != '1') && (ch != '7'))) && (((ch != 'l') && (ch != '9')) && (ch != 'g'))) && (ch != 'I'))
                {
                    text = text + ch.ToString();
                }
            }
            this.RemoveVerifyCookie();
            HttpCookie cookie = new HttpCookie(this.verifyCodeKey) {
                Value = HiCryptographer.Encrypt(text)
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
            return text;
        }

        public string GetAppshopSkinPath()
        {
            return (Globals.ApplicationPath + "/templates/appshop");
        }

        private string GetSiteUrl()
        {
            return this._httpContext.Request.Url.Host;
        }

        public string GetSkinPath()
        {
            return (Globals.ApplicationPath + "/Templates/master/" + this.SiteSettings.Theme).ToLower(CultureInfo.InvariantCulture);
        }

        public string GetStoragePath()
        {
            return "/Storage/master/";
        }

        public string GetWapshopSkinPath(ClientType clientType, string themeName = null)
        {
            string wapTheme = string.Empty;
            if (themeName != null)
            {
                wapTheme = themeName;
            }
            else
            {
                switch (clientType)
                {
                    case ClientType.VShop:
                        wapTheme = this.SiteSettings.VTheme;
                        goto Label_0069;

                    case ClientType.WAP:
                        wapTheme = this.SiteSettings.WapTheme;
                        goto Label_0069;

                    case ClientType.AliOH:
                        wapTheme = this.SiteSettings.AliOHTheme;
                        goto Label_0069;
                }
                wapTheme = this.SiteSettings.WapTheme;
            }
        Label_0069:
            return (Globals.ApplicationPath + "/templates/wapshop/" + wapTheme).ToLower(CultureInfo.InvariantCulture);
        }

        private void Initialize(NameValueCollection qs, Uri uri, string rawUrl, string siteUrl)
        {
            this._queryString = qs;
            this._siteUrl = siteUrl.ToLower();
            if (((this._queryString != null) && (this._queryString.Count > 0)) && !string.IsNullOrEmpty(this._queryString["ReferralUserId"]))
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["Site_ReferralUser"];
                if (cookie == null)
                {
                    cookie = new HttpCookie("Site_ReferralUser");
                }
                cookie.Value = this._queryString["ReferralUserId"];
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        private void RemoveVerifyCookie()
        {
            HttpContext.Current.Response.Cookies[this.verifyCodeKey].Value = null;
            HttpContext.Current.Response.Cookies[this.verifyCodeKey].Expires = new DateTime(0x777, 10, 12);
        }

        private static void SaveContextToStore(HiContext context)
        {
            context.Context.Items["Hishop_ContextStore"] = context;
        }

        public Hidistro.Core.Enums.ApplicationType ApplicationType
        {
            get
            {
                return this.Config.AppLocation.CurrentApplicationType;
            }
        }

        public HiConfiguration Config
        {
            get
            {
                if (this._config == null)
                {
                    this._config = HiConfiguration.GetConfig();
                }
                return this._config;
            }
        }

        public HttpContext Context
        {
            get
            {
                return this._httpContext;
            }
        }

        public static HiContext Current
        {
            get
            {
                HttpContext current = HttpContext.Current;
                HiContext context = current.Items["Hishop_ContextStore"] as HiContext;
                if (context == null)
                {
                    if (current == null)
                    {
                        throw new Exception("No HiContext exists in the Current Application. AutoCreate fails since HttpContext.Current is not accessible.");
                    }
                    context = new HiContext(current);
                    SaveContextToStore(context);
                }
                return context;
            }
        }

        public ClientType CurrentClientType
        {
            get
            {
                string str = HttpContext.Current.Request.Url.ToString();
                ClientType wAP = ClientType.WAP;
                if (str.ToLower().IndexOf("/vshop/") > -1)
                {
                    wAP = ClientType.VShop;
                }
                if (str.ToLower().IndexOf("/alioh/") > -1)
                {
                    wAP = ClientType.AliOH;
                }
                if (str.ToLower().IndexOf("AppShop") > -1)
                {
                    wAP = ClientType.App;
                }
                return wAP;
            }
        }

        public int DeliveryScopRegionId
        {
            get
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["User_DeliveryScopRegionId"];
                if ((cookie == null) || string.IsNullOrEmpty(cookie.Value))
                {
                    return 0;
                }
                int result = 0;
                int.TryParse(cookie.Value, out result);
                return result;
            }
            set
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["User_DeliveryScopRegionId"];
                if (cookie == null)
                {
                    cookie = new HttpCookie("User_DeliveryScopRegionId");
                }
                cookie.Value = value.ToString();
                cookie.Expires.AddYears(1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public string GetClientPath
        {
            get
            {
                string str = HttpContext.Current.Request.Url.ToString();
                string str2 = "WapShop";
                if (str.ToLower().IndexOf("/vshop/") > -1)
                {
                    str2 = "VShop";
                }
                if (str.ToLower().IndexOf("/alioh/") > -1)
                {
                    str2 = "AliOH";
                }
                return str2;
            }
        }

        public string HostPath
        {
            get
            {
                if (this._hostPath == null)
                {
                    Uri url = this.Context.Request.Url;
                    string str = (url.Port == 80) ? string.Empty : (":" + url.Port.ToString(CultureInfo.InvariantCulture));
                    this._hostPath = string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}", new object[] { url.Scheme, url.Host, str });
                }
                return this._hostPath;
            }
        }

        public bool IsUrlReWritten
        {
            get
            {
                return this._isUrlReWritten;
            }
            set
            {
                this._isUrlReWritten = value;
            }
        }

        public int ReferralUserId
        {
            get
            {
                if (string.Compare(Globals.DomainName, Current.SiteSettings.SiteUrl, true, CultureInfo.InvariantCulture) != 0)
                {
                    return 0;
                }
                HttpCookie cookie = HttpContext.Current.Request.Cookies["Site_ReferralUser"];
                if ((cookie == null) || string.IsNullOrEmpty(cookie.Value))
                {
                    return 0;
                }
                int result = 0;
                int.TryParse(cookie.Value, out result);
                return result;
            }
        }

        public string RolesCacheKey
        {
            get
            {
                return this.rolesCacheKey;
            }
            set
            {
                this.rolesCacheKey = value;
            }
        }

        public Hidistro.Membership.Context.SiteSettings SiteSettings
        {
            get
            {
                if (this.currentSettings == null)
                {
                    this.currentSettings = SettingsManager.GetMasterSettings(true);
                }
                return this.currentSettings;
            }
        }

        public string SiteUrl
        {
            get
            {
                return this._siteUrl;
            }
        }

        public IUser User
        {
            get
            {
                if (this._currentUser == null)
                {
                    this._currentUser = Users.GetUser();
                }
                return this._currentUser;
            }
            set
            {
                this._currentUser = value;
            }
        }
    }
}

