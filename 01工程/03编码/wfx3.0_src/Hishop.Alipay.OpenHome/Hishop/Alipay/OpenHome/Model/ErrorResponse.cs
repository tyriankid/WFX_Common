namespace Hishop.Alipay.OpenHome.Model
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class ErrorResponse
    {
        public string code { get; set; }

        public bool IsError
        {
            get
            {
                return !string.IsNullOrEmpty(this.code);
            }
        }

        public string msg { get; set; }

        public string sub_code { get; set; }

        public string sub_msg { get; set; }
    }
}

