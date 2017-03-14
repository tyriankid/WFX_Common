namespace Hishop.Alipay.OpenHome.Handle
{
    using Hishop.Alipay.OpenHome;
    using System;

    internal interface IHandle
    {
        string Handle(string requestContent);

        string AliRsaPubKey { get; set; }

        AlipayOHClient client { get; set; }

        string LocalRsaPriKey { get; set; }

        string LocalRsaPubKey { get; set; }
    }
}

