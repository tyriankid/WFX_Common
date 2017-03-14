namespace Hidistro.Membership.Context
{
    using Hidistro.Core;
    using Hidistro.Membership.Core;
    using System;

    public abstract class BizActorProvider
    {
        private static readonly BizActorProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.Membership.Data.BizActorData,Hidistro.Membership.Data") as BizActorProvider);

        protected BizActorProvider()
        {
        }

        public abstract bool ChangeMemberTradePassword(string username, string oldPassword, string newPassword);
        public abstract bool CreateManager(SiteManager manager);
        public abstract bool CreateMember(Member member);
        public abstract bool CreateStoreAdmin(StoreAdmin admin);
        public abstract SiteManager GetManager(HiMembershipUser membershipUser);
        public abstract Member GetMember(HiMembershipUser membershipUser);
        public abstract StoreAdmin GetStoreAdmin(HiMembershipUser membershipUser);
        public static BizActorProvider Instance()
        {
            return _defaultInstance;
        }

        public abstract bool UpdateMember(Member member);
        public abstract bool UpdateStoreAdmin(StoreAdmin admin);
        public abstract bool ValidMemberTradePassword(string username, string password);
    }
}

