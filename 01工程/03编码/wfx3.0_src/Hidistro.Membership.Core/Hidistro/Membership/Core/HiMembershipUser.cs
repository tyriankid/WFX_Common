namespace Hidistro.Membership.Core
{
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.Security;

    public class HiMembershipUser
    {
        private string comment;
        private string email;
        private bool isApproved;
        private DateTime lastActivityDate;
        private DateTime lastLoginDate;

        public HiMembershipUser(bool isAnonymous, Hidistro.Membership.Core.Enums.UserRole userRole)
        {
            if (isAnonymous && (userRole != Hidistro.Membership.Core.Enums.UserRole.Anonymous))
            {
                throw new Exception(string.Format("Current user is Anonymous, But the user role is '{0}'", userRole.ToString()));
            }
            this.UserRole = userRole;
            this.IsAnonymous = userRole == Hidistro.Membership.Core.Enums.UserRole.Anonymous;
        }

        public HiMembershipUser(bool isAnonymous, Hidistro.Membership.Core.Enums.UserRole userRole, MembershipUser mu) : this(isAnonymous, userRole)
        {
            this.RefreshMembershipUser(mu);
        }

        public virtual bool ChangePassword(string password, string newPassword)
        {
            return this.Membership.ChangePassword(password, newPassword);
        }

        public bool ChangePasswordQuestionAndAnswer(string newQuestion, string newAnswer)
        {
            if (string.IsNullOrEmpty(newQuestion) || string.IsNullOrEmpty(newAnswer))
            {
                return false;
            }
            if ((newQuestion.Length > 0x100) || (newAnswer.Length > 0x80))
            {
                return false;
            }
            if (!string.IsNullOrEmpty(this.PasswordQuestion))
            {
                return false;
            }
            return MemberUserProvider.Instance().ChangePasswordQuestionAndAnswer(this.Username, newQuestion, newAnswer);
        }

        public virtual bool ChangePasswordQuestionAndAnswer(string oldAnswer, string newQuestion, string newAnswer)
        {
            if (string.IsNullOrEmpty(newQuestion) || string.IsNullOrEmpty(newAnswer))
            {
                return false;
            }
            if ((newQuestion.Length > 0x100) || (newAnswer.Length > 0x80))
            {
                return false;
            }
            return (this.ValidatePasswordAnswer(oldAnswer) && MemberUserProvider.Instance().ChangePasswordQuestionAndAnswer(this.Username, newQuestion, newAnswer));
        }

        public virtual bool ChangePasswordWithAnswer(string answer, string newPassword)
        {
            try
            {
                string str = this.ResetPassword(answer);
                if (string.IsNullOrEmpty(str))
                {
                    return false;
                }
                return this.ChangePassword(str, newPassword);
            }
            catch
            {
                return false;
            }
        }

        public void RefreshMembershipUser(MembershipUser mu)
        {
            if (mu == null)
            {
                throw new Exception("A null MembershipUser is not valid to instantiate a new User");
            }
            this.Membership = mu;
            this.Username = mu.UserName;
            this.UserId = (int) mu.ProviderUserKey;
            this.Comment = mu.Comment;
            this.LastLockoutDate = mu.LastLockoutDate;
            this.LastPasswordChangedDate = mu.LastPasswordChangedDate;
            this.LastLoginDate = mu.LastLoginDate;
            this.CreateDate = mu.CreationDate;
            this.IsLockedOut = mu.IsLockedOut;
            this.IsApproved = mu.IsApproved;
            this.PasswordQuestion = mu.PasswordQuestion;
            this.Email = mu.Email;
            this.LastActivityDate = mu.LastActivityDate;
        }

        public virtual string ResetPassword(string answer)
        {
            try
            {
                if (this.ValidatePasswordAnswer(answer))
                {
                    return this.Membership.ResetPassword();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public virtual bool ValidatePasswordAnswer(string answer)
        {
            return MemberUserProvider.Instance().ValidatePasswordAnswer(this.Username, answer);
        }

        public string AliOpenId { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Comment
        {
            get
            {
                return this.comment;
            }
            set
            {
                this.comment = value;
                if (this.Membership != null)
                {
                    this.Membership.Comment = value;
                }
            }
        }

        public DateTime CreateDate { get; private set; }

        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                this.email = value;
                if (this.Membership != null)
                {
                    this.Membership.Email = value;
                }
            }
        }

        public Hidistro.Membership.Core.Enums.Gender Gender { get; set; }

        public bool IsAnonymous { get; private set; }

        public bool IsApproved
        {
            get
            {
                return this.isApproved;
            }
            set
            {
                this.isApproved = value;
                if (this.Membership != null)
                {
                    this.Membership.IsApproved = value;
                }
            }
        }

        public bool IsLockedOut { get; private set; }

        public bool IsOpenBalance { get; set; }

        public DateTime LastActivityDate
        {
            get
            {
                return this.lastActivityDate;
            }
            set
            {
                this.lastActivityDate = value;
                if (this.Membership != null)
                {
                    this.Membership.LastActivityDate = value;
                }
            }
        }

        public DateTime LastLockoutDate { get; private set; }

        public DateTime LastLoginDate
        {
            get
            {
                return this.lastLoginDate;
            }
            set
            {
                this.lastLoginDate = value;
                if (this.Membership != null)
                {
                    this.Membership.LastLoginDate = value;
                }
            }
        }

        public DateTime LastPasswordChangedDate { get; private set; }

        public int LoginSource { get; set; }

        public MembershipUser Membership { get; private set; }

        public string MobilePIN { get; set; }

        public string OpenId { get; set; }

        public string Password { get; set; }

        public MembershipPasswordFormat PasswordFormat { get; set; }

        public string PasswordQuestion { get; private set; }

        public string QqOpenId { get; set; }

        public int RegSource { get; set; }

        public string SinaOpenId { get; set; }

        public string TaobaoOpenId { get; set; }

        public string TradePassword { get; set; }

        public MembershipPasswordFormat TradePasswordFormat { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public Hidistro.Membership.Core.Enums.UserRole UserRole { get; private set; }
    }
}

