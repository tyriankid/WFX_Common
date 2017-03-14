namespace Hishop.Alipay.OpenHome.Model
{
    using System;

    public interface IAliResponseStatus
    {
        string Code { get; }

        string Message { get; }
    }
}

