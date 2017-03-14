namespace Hidistro.Membership.Context
{
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Web.Security;

    public class StoreAdmin : IUser
    {
        public static  event EventHandler<UserEventArgs> DealPasswordChanged;

        public static  event EventHandler<EventArgs> Login;

        public static  event EventHandler<UserEventArgs> Logout;

        public static  event EventHandler<UserEventArgs> PasswordChanged;

        public static  event EventHandler<UserEventArgs> Register;

        public StoreAdmin()
        {
            Hidistro.Membership.Core.Enums.UserRole storeAdmin = Hidistro.Membership.Core.Enums.UserRole.StoreAdmin;
            this.MembershipUser = new HiMembershipUser(false, storeAdmin);
        }

        public StoreAdmin(Hidistro.Membership.Core.Enums.UserRole userRole, HiMembershipUser membershipUser)
        {
            if (userRole != Hidistro.Membership.Core.Enums.UserRole.StoreAdmin)
            {
                throw new Exception("UserRole must be Member or Underling");
            }
            this.MembershipUser = membershipUser;
        }

        public bool ChangePassword(string newPassword)
        {
            if (((this.UserRole == Hidistro.Membership.Core.Enums.UserRole.Member) || (this.UserRole == Hidistro.Membership.Core.Enums.UserRole.StoreAdmin)) && ((HiContext.Current.User.UserRole == Hidistro.Membership.Core.Enums.UserRole.StoreAdmin) || (HiContext.Current.User.UserRole == Hidistro.Membership.Core.Enums.UserRole.SiteManager)))
            {
                string password = this.MembershipUser.Membership.ResetPassword();
                if (this.MembershipUser.ChangePassword(password, newPassword))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ChangePassword(string oldPassword, string newPassword)
        {
            return this.MembershipUser.ChangePassword(oldPassword, newPassword);
        }

        public bool ChangePasswordQuestionAndAnswer(string newQuestion, string newAnswer)
        {
            return this.MembershipUser.ChangePasswordQuestionAndAnswer(newQuestion, newAnswer);
        }

        public bool ChangePasswordQuestionAndAnswer(string oldAnswer, string newQuestion, string newAnswer)
        {
            return this.MembershipUser.ChangePasswordQuestionAndAnswer(oldAnswer, newQuestion, newAnswer);
        }

        public bool ChangePasswordWithAnswer(string answer, string newPassword)
        {
            return this.MembershipUser.ChangePasswordWithAnswer(answer, newPassword);
        }

        public bool ChangePasswordWithoutAnswer(string newPassword)
        {
            string password = this.MembershipUser.Membership.ResetPassword();
            return this.MembershipUser.ChangePassword(password, newPassword);
        }

        public bool ChangeTradePassword(string newPassword)
        {
            return true;
        }

        public bool ChangeTradePassword(string oldPassword, string newPassword)
        {
            return true;
        }

        private UserFactory GetMemberFactory()
        {
            if (this.UserRole != Hidistro.Membership.Core.Enums.UserRole.StoreAdmin)
            {
                throw new Exception("UserRole must be Member or Underling");
            }
            return StoreAdminFactory.Instance();
        }

        public IUserCookie GetUserCookie()
        {
            return new UserCookie(this);
        }

        public bool IsInRole(string roleName)
        {
            return ((this.UserRole == Hidistro.Membership.Core.Enums.UserRole.StoreAdmin) && roleName.Equals(HiContext.Current.Config.RolesConfiguration.StoreAdmin));
        }

        public void OnDealPasswordChanged(UserEventArgs args)
        {
            if (DealPasswordChanged != null)
            {
                DealPasswordChanged(this, args);
            }
        }

        public void OnLogin()
        {
            if (Login != null)
            {
                Login(this, new EventArgs());
            }
        }

        public static void OnLogin(StoreAdmin member)
        {
            if (Login != null)
            {
                Login(member, new EventArgs());
            }
        }

        public static void OnLogout(UserEventArgs args)
        {
            if (Logout != null)
            {
                Logout(null, args);
            }
        }

        public void OnPasswordChanged(UserEventArgs args)
        {
            if (PasswordChanged != null)
            {
                PasswordChanged(this, args);
            }
        }

        public static void OnPasswordChanged(StoreAdmin member, UserEventArgs args)
        {
            if (PasswordChanged != null)
            {
                PasswordChanged(member, args);
            }
        }

        public void OnRegister(UserEventArgs args)
        {
            if (Register != null)
            {
                Register(this, args);
            }
        }

        public static void OnRegister(StoreAdmin member, UserEventArgs args)
        {
            if (Register != null)
            {
                Register(member, args);
            }
        }

        public string ResetPassword(string answer)
        {
            return this.MembershipUser.ResetPassword(answer);
        }

        public bool ValidatePasswordAnswer(string answer)
        {
            return this.MembershipUser.ValidatePasswordAnswer(answer);
        }

        public string Address { get; set; }

        public string AliOpenId { get; set; }

        public DateTime? BirthDate
        {
            get
            {
                return this.MembershipUser.BirthDate;
            }
            set
            {
                this.MembershipUser.BirthDate = value;
            }
        }

        public bool CloseStatus { get; set; }

        public string Comment
        {
            get
            {
                return this.MembershipUser.Comment;
            }
            set
            {
                this.MembershipUser.Comment = value;
            }
        }

        public string ContactMan { get; set; }

        public DateTime CreateDate
        {
            get
            {
                return this.MembershipUser.CreateDate;
            }
        }

        public IDictionary<int, string> DeliveryScopList { get; set; }

        public string Email
        {
            get
            {
                return this.MembershipUser.Email;
            }
            set
            {
                this.MembershipUser.Email = value;
            }
        }

        public Hidistro.Membership.Core.Enums.Gender Gender
        {
            get
            {
                return this.MembershipUser.Gender;
            }
            set
            {
                this.MembershipUser.Gender = value;
            }
        }

        public bool IsAnonymous
        {
            get
            {
                return this.MembershipUser.IsAnonymous;
            }
        }

        public bool IsApproved
        {
            get
            {
                return this.MembershipUser.IsApproved;
            }
            set
            {
                this.MembershipUser.IsApproved = value;
            }
        }

        public bool IsLockedOut
        {
            get
            {
                return this.MembershipUser.IsLockedOut;
            }
        }

        public bool IsOpenBalance
        {
            get
            {
                return this.MembershipUser.IsOpenBalance;
            }
            set
            {
                this.MembershipUser.IsOpenBalance = value;
            }
        }

        public DateTime LastActivityDate
        {
            get
            {
                return this.MembershipUser.LastActivityDate;
            }
            set
            {
                this.MembershipUser.LastActivityDate = value;
            }
        }

        public DateTime LastLockoutDate
        {
            get
            {
                return this.MembershipUser.LastLockoutDate;
            }
        }

        public DateTime LastLoginDate
        {
            get
            {
                return this.MembershipUser.LastLoginDate;
            }
        }

        public DateTime LastPasswordChangedDate
        {
            get
            {
                return this.MembershipUser.LastPasswordChangedDate;
            }
        }

        public HiMembershipUser MembershipUser { get; private set; }

        public string MobilePIN
        {
            get
            {
                return this.MembershipUser.MobilePIN;
            }
            set
            {
                this.MembershipUser.MobilePIN = value;
            }
        }

        public string OpenId { get; set; }

        public string Password
        {
            get
            {
                return this.MembershipUser.Password;
            }
            set
            {
                this.MembershipUser.Password = value;
            }
        }

        public MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return this.MembershipUser.PasswordFormat;
            }
            set
            {
                this.MembershipUser.PasswordFormat = value;
            }
        }

        public string PasswordQuestion
        {
            get
            {
                return this.MembershipUser.PasswordQuestion;
            }
        }

        public string Picture { get; set; }

        public int RegionId { get; set; }

        public int State { get; set; }

        public int StoreId { get; set; }

        public string StoreName { get; set; }

        public string Tel { get; set; }

        public int TopRegionId { get; set; }

        public string TradePassword
        {
            get
            {
                return this.MembershipUser.TradePassword;
            }
            set
            {
                this.MembershipUser.TradePassword = value;
            }
        }

        public MembershipPasswordFormat TradePasswordFormat
        {
            get
            {
                return this.MembershipUser.TradePasswordFormat;
            }
            set
            {
                this.MembershipUser.TradePasswordFormat = value;
            }
        }

        public int UserId
        {
            get
            {
                return this.MembershipUser.UserId;
            }
            set
            {
                this.MembershipUser.UserId = value;
            }
        }

        public string Username
        {
            get
            {
                return this.MembershipUser.Username;
            }
            set
            {
                this.MembershipUser.Username = value;
            }
        }

        public Hidistro.Membership.Core.Enums.UserRole UserRole
        {
            get
            {
                return this.MembershipUser.UserRole;
            }
        }

        public string ZipCode { get; set; }
    }
}

