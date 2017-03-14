namespace Hidistro.Membership.Core
{
    using System;
    using System.Runtime.InteropServices;
    using System.Web;

    public interface IUserCookie
    {
        void DeleteCookie(HttpCookie cookie);
        void WriteCookie(HttpCookie cookie, int days, bool autoLogin);
        void WriteStoreCookie(string UserName, int days = 0);
        void WriteUserCookie(string UserName, int days = 0);
    }
}

