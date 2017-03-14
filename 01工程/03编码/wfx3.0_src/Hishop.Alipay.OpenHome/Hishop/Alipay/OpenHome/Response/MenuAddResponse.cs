﻿namespace Hishop.Alipay.OpenHome.Response
{
    using Hishop.Alipay.OpenHome.Model;
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class MenuAddResponse : AliResponse, IAliResponseStatus
    {
        public AliResponseMessage alipay_mobile_public_menu_add_response { get; set; }

        public string Code
        {
            get
            {
                return this.alipay_mobile_public_menu_add_response.code;
            }
        }

        public string Message
        {
            get
            {
                return this.alipay_mobile_public_menu_add_response.msg;
            }
        }
    }
}

