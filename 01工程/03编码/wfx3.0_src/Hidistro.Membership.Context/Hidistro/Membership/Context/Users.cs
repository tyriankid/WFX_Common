namespace Hidistro.Membership.Context
{
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Security;

    public static class Users
    {
        public static void ClearUserCache(IUser user)
        {
            Hashtable hashtable = UserCache();
            hashtable[UserKey(user.UserId.ToString(CultureInfo.InvariantCulture))] = null;
            hashtable[UserKey(user.Username)] = null;
        }

        public static CreateUserStatus CreateUser(IUser user, string role)
        {
            return CreateUser(user, new string[] { role });
        }

        public static CreateUserStatus CreateUser(IUser user, string[] roles)
        {
            CreateUserStatus unknownFailure = UserHelper.Create(user.MembershipUser, roles);
            if ((unknownFailure == CreateUserStatus.Created) && !UserFactory.Create(user.UserRole).Create(user))
            {
                HiMembership.Delete(user.Username);
                unknownFailure = CreateUserStatus.UnknownFailure;
            }
            return unknownFailure;
        }

        public static void DeleteStoreAdminCookie()
        {
            if (HiContext.Current.Context != null)
            {
                HttpCookie cookie = HiContext.Current.Context.Request.Cookies["Store-Admin"];
                if (null != cookie)
                {
                    SetCookieDomain(cookie);
                    cookie.Expires = DateTime.Now.AddDays(-1.0);
                    HiContext.Current.Context.Response.Cookies.Add(cookie);
                }
            }
        }

        public static IUser FindUserByUsername(string username)
        {
            return GetUser(0, username, true, false);
        }

        public static AnonymousUser GetAnonymousUser()
        {
            AnonymousUser anonymousUser = HiCache.Get("DataCache-AnonymousUser") as AnonymousUser;
            if (anonymousUser == null)
            {
                anonymousUser = MemberUserProvider.Instance().GetAnonymousUser();
                if (((anonymousUser != null) && (anonymousUser.Username != null)) && (anonymousUser.UserId > 0))
                {
                    HiCache.Insert("DataCache-AnonymousUser", anonymousUser, 120);
                }
            }
            return anonymousUser;
        }

        public static IUser GetContexUser()
        {
            IUser user = GetUser(0, GetLoggedOnUsername(), true, true);
            if (!user.IsAnonymous && (HiContext.Current.ApplicationType == ApplicationType.Unknown))
            {
                return GetAnonymousUser();
            }
            return user;
        }

        public static string GetLoggedOnUsername()
        {
            Exception exception;
            HttpContext current = HttpContext.Current;
            HttpCookie cookie = HiContext.Current.Context.Request.Cookies["Vshop-Member"];
            if (cookie != null)
            {
                try
                {
                    return Globals.UrlDecode(HiCryptographer.Decrypt(cookie.Value));
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    return string.Empty;
                }
            }
            cookie = HiContext.Current.Context.Request.Cookies["Store-Admin"];
            if (cookie != null)
            {
                try
                {
                    return Globals.UrlDecode(HiCryptographer.Decrypt(cookie.Value));
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                    return string.Empty;
                }
            }
            if (!(current.User.Identity.IsAuthenticated && !string.IsNullOrEmpty(current.User.Identity.Name)))
            {
                return "Anonymous";
            }
            return current.User.Identity.Name;
        }

        public static IUser GetUser()
        {
            IUser user = GetUser(0, GetLoggedOnUsername(), true, true);
            if (!user.IsAnonymous)
            {
                ApplicationType applicationType = HiContext.Current.ApplicationType;
                if (applicationType == ApplicationType.Unknown)
                {
                    return GetAnonymousUser();
                }
                if (((applicationType == ApplicationType.Admin) && (user.UserRole != UserRole.SiteManager)) && (user.UserRole != UserRole.StoreAdmin))
                {
                    return GetAnonymousUser();
                }
                if (((applicationType == ApplicationType.Member) || (applicationType == ApplicationType.Common)) && (user.UserRole != UserRole.Member))
                {
                    return GetAnonymousUser();
                }
            }
            return user;
        }

        public static IUser GetUser(int userId)
        {
            return GetUser(userId, null, true, false);
        }

        public static IUser GetUser(int userId, bool isCacheable)
        {
            return GetUser(userId, null, isCacheable, false);
        }

        public static IUser GetUser(int userId, string username, bool isCacheable, bool userIsOnline)
        {
            if (((userId == 0) && !string.IsNullOrEmpty(username)) && username.Equals("Anonymous", StringComparison.CurrentCultureIgnoreCase))
            {
                return GetAnonymousUser();
            }
            Hashtable hashtable = UserCache();
            string str = (userId > 0) ? UserKey(userId.ToString(CultureInfo.InvariantCulture)) : UserKey(username);
            IUser user = null;
            if (isCacheable)
            {
                user = hashtable[str] as IUser;
                if (user != null)
                {
                    return user;
                }
            }
            HiMembershipUser membershipUser = UserHelper.GetMembershipUser(userId, username, userIsOnline);
            if ((membershipUser == null) || membershipUser.Username.Equals("Anonymous", StringComparison.CurrentCultureIgnoreCase))
            {
                return GetAnonymousUser();
            }
            user = UserFactory.Create(membershipUser.UserRole).GetUser(membershipUser);
            if (isCacheable)
            {
                hashtable[UserKey(user.Username)] = user;
                hashtable[UserKey(user.UserId.ToString(CultureInfo.InvariantCulture))] = user;
            }
            return user;
        }

        public static IUser GetUserByAliPayOpenId(string openId)
        {
            return GetUser(UserHelper.GetUserIdByAliPayOpenId(openId));
        }

        public static IUser GetUserByOpenId(string openId)
        {
            return GetUser(UserHelper.GetUserIdByOpenId(openId));
        }

        public static IUser GetUserBySessionId(string sessionId)
        {
            return GetUser(UserHelper.GetUserIdBySessionId(sessionId));
        }

        private static void SetCookieDomain(HttpCookie cookie)
        {
            Regex regex = new Regex(@"[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (regex.IsMatch(HiContext.Current.Context.Request.Url.Host) && HiContext.Current.Context.Request.Url.Host.ToLower().EndsWith(HiContext.Current.SiteSettings.SiteUrl.ToLower()))
            {
                cookie.Path = "/";
                cookie.Domain = HiContext.Current.SiteSettings.SiteUrl;
            }
        }

        public static void SetCurrentUser(IUser user, int days, bool AutoLogin = false)
        {
            HttpCookie authCookie = FormsAuthentication.GetAuthCookie(user.Username, false);
            IUserCookie userCookie = user.GetUserCookie();
            userCookie.WriteCookie(authCookie, days, AutoLogin);
            userCookie.WriteUserCookie(user.Username, days);
        }

        public static void SetStoreAdmin(IUser user, int days, bool AutoLogin = false)
        {
            HttpCookie authCookie = FormsAuthentication.GetAuthCookie(user.Username, false);
            IUserCookie userCookie = user.GetUserCookie();
            userCookie.WriteCookie(authCookie, days, AutoLogin);
            userCookie.WriteStoreCookie(user.Username, days);
        }

        public static bool UpdateUser(IUser user)
        {
            if (null == user)
            {
                return false;
            }
            bool flag = UserHelper.UpdateUser(user.MembershipUser);
            if (flag)
            {
                flag = UserFactory.Create(user.UserRole).UpdateUser(user);
                HiContext current = HiContext.Current;
                if (current.User.UserId == user.UserId)
                {
                    current.User = user;
                }
            }
            ClearUserCache(user);
            return flag;
        }

        private static Hashtable UserCache()
        {
            Hashtable hashtable = HiCache.Get("DataCache-UserLookuptable") as Hashtable;
            if (hashtable == null)
            {
                hashtable = new Hashtable();
                HiCache.Insert("DataCache-UserLookuptable", hashtable, 120);
            }
            return hashtable;
        }

        private static string UserKey(object key)
        {
            return string.Format(CultureInfo.InvariantCulture, "User-{0}", new object[] { key }).ToLower(CultureInfo.InvariantCulture);
        }

        public static LoginUserStatus ValidateUser(IUser user)
        {
            return UserHelper.ValidateUser(user.MembershipUser);
        }

        public static bool ValidTradePassword(IUser user)
        {
            return UserFactory.Create(user.UserRole).ValidTradePassword(user.Username, user.TradePassword);
        }
    }
}

