namespace Hishop.Alipay.OpenHome.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class Button
    {
        public string actionParam { get; set; }

        public string actionType { get; set; }

        public string authType
        {
            get
            {
                return "loginAuth";
            }
        }

        public string name { get; set; }

        public IEnumerable<Button> subButton { get; set; }
    }
}

