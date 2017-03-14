namespace Hishop.Alipay.OpenHome.Model
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public abstract class AliResponse
    {
        protected AliResponse()
        {
        }

        public ErrorResponse error_response { get; set; }

        public string sign { get; set; }
    }
}

