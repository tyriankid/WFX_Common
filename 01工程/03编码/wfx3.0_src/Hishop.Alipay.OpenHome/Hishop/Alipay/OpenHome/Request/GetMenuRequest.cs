namespace Hishop.Alipay.OpenHome.Request
{
    using System;

    public class GetMenuRequest : IRequest
    {
        public string GetBizContent()
        {
            return null;
        }

        public string GetMethodName()
        {
            return "alipay.mobile.public.menu.get";
        }
    }
}

