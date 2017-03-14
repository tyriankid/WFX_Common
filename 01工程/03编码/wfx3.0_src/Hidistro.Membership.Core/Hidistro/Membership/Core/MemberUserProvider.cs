namespace Hidistro.Membership.Core
{
    using Hidistro.Core;
    using Hidistro.Membership.Core.Enums;
    using System;

    public abstract class MemberUserProvider
    {
        private static readonly MemberUserProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.Membership.Data.UserData,Hidistro.Membership.Data") as MemberUserProvider);

        protected MemberUserProvider()
        {
        }

        public abstract bool BindOpenId(string username, string openId, string openIdType);
        public abstract bool ChangePasswordQuestionAndAnswer(string username, string newQuestion, string newAnswer);
        public abstract CreateUserStatus CreateMembershipUser(HiMembershipUser userToCreate, string passwordQuestion, string passwordAnswer);
        public abstract AnonymousUser GetAnonymousUser();
        public abstract HiMembershipUser GetMembershipUser(int userId, string username, bool isOnline);
        public abstract int GetUserIdByAliPayOpenId(string openId);
        public abstract int GetUserIdByEmail(string email);
        public abstract int GetUserIdByOpenId(string openId);
        public abstract int GetUserIdByOpenId(string openId, string openIdType);
        public abstract int GetUserIdBySessionId(string sessionid);
        public abstract string GetUsernameWithOpenId(string openId, string openIdType);
        public static MemberUserProvider Instance()
        {
            return _defaultInstance;
        }

        public abstract bool IsExistEmal(string email);
        public abstract bool IsExistEmal(int userid, string email);
        public abstract bool IsExistUserName(string username);
        public abstract bool IsExistUserName(int userid, string username);
        public abstract bool UpdateMembershipUser(HiMembershipUser user);
        public abstract string UpdateSessionId(int userId);
        public abstract bool ValidatePasswordAnswer(string username, string answer);
    }
}

