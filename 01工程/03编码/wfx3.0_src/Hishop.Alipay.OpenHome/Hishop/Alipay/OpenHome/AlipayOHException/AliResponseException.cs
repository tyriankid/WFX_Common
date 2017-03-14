namespace Hishop.Alipay.OpenHome.AlipayOHException
{
    using System;
    using System.Runtime.CompilerServices;

    public class AliResponseException : AlipayOpenHomeException
    {
        public AliResponseException()
        {
        }

        public AliResponseException(string code, string message) : base(message)
        {
            this.ResponseCode = code;
        }

        public AliResponseException(string code, string message, Exception innerException) : base(message, innerException)
        {
            this.ResponseCode = code;
        }

        public string ResponseCode { get; set; }
    }
}

