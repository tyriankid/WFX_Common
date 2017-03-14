namespace Hishop.Alipay.OpenHome.Handle
{
    using Aop.Api.Util;
    using Hishop.Alipay.OpenHome;
    using Hishop.Alipay.OpenHome.Utility;
    using System;
    using System.Runtime.CompilerServices;

    internal class VerifyGateWayHandle : IHandle
    {
        public string Handle(string requestContent)
        {
            return AlipaySignature.encryptAndSign(string.Format("<success>true</success><biz_content>{0}</biz_content>", RsaFileHelper.GetRSAKeyContent(this.LocalRsaPubKey, true)), this.AliRsaPubKey, this.LocalRsaPriKey, "UTF-8", false, true, true);
        }

        public string AliRsaPubKey { get; set; }

        public AlipayOHClient client { get; set; }

        public string LocalRsaPriKey { get; set; }

        public string LocalRsaPubKey { get; set; }
    }
}

