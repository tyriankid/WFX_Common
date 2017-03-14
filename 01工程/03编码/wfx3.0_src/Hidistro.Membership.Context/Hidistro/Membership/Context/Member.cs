namespace Hidistro.Membership.Context
{
    using Hidistro.Core.Configuration;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web.Security;

    [HasSelfValidation]
    public class Member : IUser
    {
        public static  event EventHandler<UserEventArgs> DealPasswordChanged;

        public static  event EventHandler<UserEventArgs> FindPassword;

        public static  event EventHandler<EventArgs> Login;

        public static  event EventHandler<UserEventArgs> Logout;

        public static  event EventHandler<UserEventArgs> PasswordChanged;

        public static  event EventHandler<UserEventArgs> Register;

        public Member(Hidistro.Membership.Core.Enums.UserRole userRole)
        {
            if (userRole != Hidistro.Membership.Core.Enums.UserRole.Member)
            {
                throw new Exception("UserRole must be Member or Underling");
            }
            this.MembershipUser = new HiMembershipUser(false, userRole);
        }

        public Member(Hidistro.Membership.Core.Enums.UserRole userRole, HiMembershipUser membershipUser)
        {
            if (userRole != Hidistro.Membership.Core.Enums.UserRole.Member)
            {
                throw new Exception("UserRole must be Member or Underling");
            }
            this.MembershipUser = membershipUser;
        }

        public bool ChangePassword(string newPassword)
        {
            if ((this.UserRole == Hidistro.Membership.Core.Enums.UserRole.Member) && (HiContext.Current.User.UserRole == Hidistro.Membership.Core.Enums.UserRole.SiteManager))
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
            return this.GetMemberFactory().ChangeTradePassword(this.Username, newPassword);
        }

        public bool ChangeTradePassword(string oldPassword, string newPassword)
        {
            return this.GetMemberFactory().ChangeTradePassword(this.Username, oldPassword, newPassword);
        }

        [SelfValidation(Ruleset="ValMember")]
        public void CheckMemberEmail(ValidationResults results)
        {
            HiConfiguration config = HiConfiguration.GetConfig();
            if (string.IsNullOrEmpty(this.Email) || (this.Email.Length > 0x100))
            {
                results.AddResult(new ValidationResult("电子邮件不能为空且长度必须小于256个字符", this, "", "", null));
            }
            else if (!Regex.IsMatch(this.Email, config.EmailRegex))
            {
                results.AddResult(new ValidationResult("电子邮件的格式错误", this, "", "", null));
            }
            if (!(string.IsNullOrEmpty(this.QQ) || (((this.QQ.Length <= 20) && (this.QQ.Length >= 3)) && Regex.IsMatch(this.QQ, "^[0-9]*$"))))
            {
                results.AddResult(new ValidationResult("QQ号长度限制在3-20个字符之间，只能输入数字", this, "", "", null));
            }
            if (!(string.IsNullOrEmpty(this.Wangwang) || ((this.Wangwang.Length <= 20) && (this.Wangwang.Length >= 3))))
            {
                results.AddResult(new ValidationResult("旺旺长度限制在3-20个字符之间", this, "", "", null));
            }
            if (!(string.IsNullOrEmpty(this.CellPhone) || (((this.CellPhone.Length <= 20) && (this.CellPhone.Length >= 3)) && Regex.IsMatch(this.CellPhone, "^[0-9]*$"))))
            {
                results.AddResult(new ValidationResult("手机号码长度限制在3-20个字符之间,只能输入数字", this, "", "", null));
            }
            if (!(string.IsNullOrEmpty(this.TelPhone) || (((this.TelPhone.Length <= 20) && (this.TelPhone.Length >= 3)) && Regex.IsMatch(this.TelPhone, "^[0-9-]*$"))))
            {
                results.AddResult(new ValidationResult("电话号码长度限制在3-20个字符之间，只能输入数字和字符“-”", this, "", "", null));
            }
            if (!(string.IsNullOrEmpty(this.MSN) || (((this.MSN.Length <= 0x100) && (this.MSN.Length >= 1)) && Regex.IsMatch(this.MSN, config.UsernameRegex))))
            {
                results.AddResult(new ValidationResult("请输入正确的微信号码，长度在1-256个字符以内", this, "", "", null));
            }
        }

        private UserFactory GetMemberFactory()
        {
            if (this.UserRole != Hidistro.Membership.Core.Enums.UserRole.Member)
            {
                throw new Exception("UserRole must be Member or Underling");
            }
            return MemberFactory.Instance();
        }

        public IUserCookie GetUserCookie()
        {
            return new UserCookie(this);
        }

        public bool IsInRole(string roleName)
        {
            return ((this.UserRole == Hidistro.Membership.Core.Enums.UserRole.Member) && roleName.Equals(HiContext.Current.Config.RolesConfiguration.Member));
        }

        public void OnDealPasswordChanged(UserEventArgs args)
        {
            if (DealPasswordChanged != null)
            {
                DealPasswordChanged(this, args);
            }
        }

        public static void OnDealPasswordChanged(Member member, UserEventArgs args)
        {
            if (DealPasswordChanged != null)
            {
                DealPasswordChanged(member, args);
            }
        }

        public void OnFindPassword(UserEventArgs args)
        {
            if (FindPassword != null)
            {
                FindPassword(this, args);
            }
        }

        public static void OnFindPassword(Member member, UserEventArgs args)
        {
            if (FindPassword != null)
            {
                FindPassword(member, args);
            }
        }

        public void OnLogin()
        {
            if (Login != null)
            {
                Login(this, new EventArgs());
            }
        }

        public static void OnLogin(Member member)
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

        public static void OnPasswordChanged(Member member, UserEventArgs args)
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

        public static void OnRegister(Member member, UserEventArgs args)
        {
            if (Register != null)
            {
                Register(member, args);
            }
        }

        public bool OpenBalance(string tradePassword)
        {
            return this.GetMemberFactory().OpenBalance(this.UserId, tradePassword);
        }

        public string ResetPassword(string answer)
        {
            return this.MembershipUser.ResetPassword(answer);
        }

        public string ResetTradePassword(string username)
        {
            return this.GetMemberFactory().ResetTradePassword(username);
        }

        public bool ValidatePasswordAnswer(string answer)
        {
            return this.MembershipUser.ValidatePasswordAnswer(answer);
        }

        [StringLengthValidator(0, 100, Ruleset="ValMember", MessageTemplate="详细地址必须控制在100个字符以内")]
        public string Address { get; set; }

        public string AliOpenId
        {
            get
            {
                return this.MembershipUser.AliOpenId;
            }
            set
            {
                this.MembershipUser.AliOpenId = value;
            }
        }

        public decimal Balance { get; set; }

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

        public string CellPhone { get; set; }

        public bool CellPhoneVerification { get; set; }

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

        public DateTime CreateDate
        {
            get
            {
                return this.MembershipUser.CreateDate;
            }
        }

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

        public bool EmailVerification { get; set; }

        public decimal Expenditure { get; set; }

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

        public int GradeId { get; set; }

        public string IdentityCard { get; set; }

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

        public int LoginSource
        {
            get
            {
                return this.MembershipUser.LoginSource;
            }
            set
            {
                this.MembershipUser.LoginSource = value;
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

        public string MSN { get; set; }

        public string OpenId
        {
            get
            {
                return this.MembershipUser.OpenId;
            }
            set
            {
                this.MembershipUser.OpenId = value;
            }
        }

        public int OrderNumber { get; set; }

        public int? ParentUserId { get; set; }

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

        public int Points { get; set; }

        public string QQ { get; set; }

        public string QqOpenId
        {
            get
            {
                return this.MembershipUser.QqOpenId;
            }
            set
            {
                this.MembershipUser.QqOpenId = value;
            }
        }

        [StringLengthValidator(0, 20, Ruleset="ValMember", MessageTemplate="真实姓名必须控制在20个字符以内")]
        public string RealName { get; set; }

        public DateTime? ReferralAuditDate { get; set; }

        public string ReferralReason { get; set; }

        public DateTime? ReferralRequetsDate { get; set; }

        public int ReferralStatus { get; set; }

        public int? ReferralUserId { get; set; }

        public string RefusalReason { get; set; }

        public int RegionId { get; set; }

        public int RegSource
        {
            get
            {
                return this.MembershipUser.RegSource;
            }
            set
            {
                this.MembershipUser.RegSource = value;
            }
        }

        public decimal RequestBalance { get; set; }

        public string SessionId { get; set; }

        public string SinaOpenId
        {
            get
            {
                return this.MembershipUser.SinaOpenId;
            }
            set
            {
                this.MembershipUser.SinaOpenId = value;
            }
        }

        public string TaobaoOpenId
        {
            get
            {
                return this.MembershipUser.TaobaoOpenId;
            }
            set
            {
                this.MembershipUser.TaobaoOpenId = value;
            }
        }

        public string TelPhone { get; set; }

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

        public string VipCardNumber { get; set; }

        public string Wangwang { get; set; }

        public string WeChat { get; set; }

        public string Zipcode { get; set; }
    }
}

