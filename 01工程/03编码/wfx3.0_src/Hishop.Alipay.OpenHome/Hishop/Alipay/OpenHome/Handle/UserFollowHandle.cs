namespace Hishop.Alipay.OpenHome.Handle
{
    using Aop.Api.Util;
    using Hishop.Alipay.OpenHome;
    using System;
    using System.Runtime.CompilerServices;

    internal class UserFollowHandle : IHandle
    {
        public string Handle(string requestContent)
        {
            return AlipaySignature.encryptAndSign(this.client.FireUserFollowEvent(), this.AliRsaPubKey, this.LocalRsaPriKey, "UTF-8", false, true, true);
        }

        public string AliRsaPubKey { get; set; }

        public AlipayOHClient client { get; set; }

        public string LocalRsaPriKey { get; set; }

        public string LocalRsaPubKey { get; set; }
    }
}

