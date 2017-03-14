﻿namespace Hishop.Alipay.OpenHome.AlipayOHException
{
    using System;

    public class RequestException : AlipayOpenHomeException
    {
        public RequestException()
        {
        }

        public RequestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

