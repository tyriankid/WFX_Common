namespace Hidistro.Membership.Context
{
    using System;
    using System.Runtime.CompilerServices;

    public class UserEventArgs : EventArgs
    {
        public UserEventArgs(string username, string password, string dealPassword)
        {
            this.Username = username;
            this.Password = password;
            this.DealPassword = dealPassword;
        }

        public string DealPassword { get; private set; }

        public string Password { get; private set; }

        public string Username { get; private set; }
    }
}

