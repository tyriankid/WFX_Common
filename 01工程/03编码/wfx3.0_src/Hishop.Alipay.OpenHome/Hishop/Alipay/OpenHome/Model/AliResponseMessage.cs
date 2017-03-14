namespace Hishop.Alipay.OpenHome.Model
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class AliResponseMessage
    {
        public string code { get; set; }

        public string msg { get; set; }
    }
}

