namespace Hishop.Alipay.OpenHome.Utility
{
    using System;

    internal class TimeHelper
    {
        public static double TransferToMilStartWith1970(DateTime dateTime)
        {
            DateTime time = new DateTime(0x7b2, 1, 1);
            TimeSpan span = (TimeSpan) (dateTime - time);
            return span.TotalMilliseconds;
        }
    }
}

