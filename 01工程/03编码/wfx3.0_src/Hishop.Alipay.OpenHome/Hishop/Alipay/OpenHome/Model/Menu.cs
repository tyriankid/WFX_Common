namespace Hishop.Alipay.OpenHome.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class Menu : ModelBase
    {
        public IEnumerable<Button> button { get; set; }
    }
}

