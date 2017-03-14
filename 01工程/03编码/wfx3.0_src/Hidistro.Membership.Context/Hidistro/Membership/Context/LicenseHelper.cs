namespace Hidistro.Membership.Context
{
    using Hidistro.Core;
    using System;
    using System.IO;
    using System.Web;
    using System.Web.Caching;

    public static class LicenseHelper
    {
        public const string CachePublicKey = "Hishop_PublicKey";

        public static string GetPublicKey()
        {
            string str = HiCache.Get("Hishop_PublicKey") as string;
            if (string.IsNullOrEmpty(str))
            {
                string path = null;
                HttpContext current = HttpContext.Current;
                if (current != null)
                {
                    path = current.Request.MapPath("~/config/publickey.xml");
                }
                else
                {
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "/config/publickey.xml");
                }
                str = File.ReadAllText(path);
                HiCache.Max("Hishop_PublicKey", str, new CacheDependency(path));
            }
            return str;
        }

        public static string GetSiteHash()
        {
            return SettingsManager.GetMasterSettings(false).CheckCode;
        }
    }
}

