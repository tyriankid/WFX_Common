namespace Hishop.Alipay.OpenHome.AlipayOHException
{
    using System;

    public class ResponseException : AlipayOpenHomeException
    {
        public ResponseException()
        {
        }

        public ResponseException(string message) : base(message)
        {
        }

        public ResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

