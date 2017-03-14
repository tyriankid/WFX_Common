namespace Hidistro.Membership.Context
{
    using Hidistro.Membership.Core;
    using System;

    internal class StoreAdminFactory : UserFactory
    {
        private static readonly StoreAdminFactory _defaultInstance = new StoreAdminFactory();
        private BizActorProvider provider;

        static StoreAdminFactory()
        {
            _defaultInstance.provider = BizActorProvider.Instance();
        }

        public override bool ChangeTradePassword(string username, string newPassword)
        {
            return true;
        }

        public override bool ChangeTradePassword(string username, string oldPassword, string newPassword)
        {
            return true;
        }

        public override bool Create(IUser userToCreate)
        {
            try
            {
                return this.provider.CreateStoreAdmin(userToCreate as StoreAdmin);
            }
            catch
            {
                return false;
            }
        }

        public override IUser GetUser(HiMembershipUser membershipUser)
        {
            return this.provider.GetStoreAdmin(membershipUser);
        }

        public static StoreAdminFactory Instance()
        {
            return _defaultInstance;
        }

        public override bool OpenBalance(int userId, string tradePassword)
        {
            return true;
        }

        public override string ResetTradePassword(string username)
        {
            return "000000";
        }

        public override bool UpdateUser(IUser user)
        {
            StoreAdmin admin = user as StoreAdmin;
            return this.provider.UpdateStoreAdmin(admin);
        }

        public override bool ValidTradePassword(string username, string password)
        {
            return true;
        }
    }
}

