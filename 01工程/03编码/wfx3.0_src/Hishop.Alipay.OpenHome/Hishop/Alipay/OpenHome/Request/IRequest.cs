namespace Hishop.Alipay.OpenHome.Request
{
    using System;

    public interface IRequest
    {
        string GetBizContent();
        string GetMethodName();
    }
}

