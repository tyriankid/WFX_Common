using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Hidistro.Core.Function
{
    /// <summary>
    /// 对象缓存类处理 add:jhb_20151026
    /// </summary>
    public class ApplicationCaching
    {
        /// <summary>
        /// 设置Cookie值
        /// </summary>
        public static void SetCookie(string cookieName,string cookieValue)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Value = cookieValue;
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 获取Cookie值
        /// </summary>
        public static string GetCookie(string cookieName)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            return cookie.Value;
        }


    }
}
