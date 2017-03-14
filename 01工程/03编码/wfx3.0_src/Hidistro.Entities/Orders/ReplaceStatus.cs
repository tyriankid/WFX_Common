namespace Hidistro.Entities.Orders
{
    using System;
    using System.ComponentModel;

    public enum ReplaceStatus
    {
        [Description("申请换货中")]
        Applied = 0,
        [Description("待用户发货")]
        MerchantsAgreed = 3,
        [Description("商家已发货")]
        MerchantsDelivery = 6,
        [Description("已拒绝")]
        Refused = 2,
        [Description("已完成")]
        Replaced = 1,
        [Description("用户已发货")]
        UserDelivery = 4
    }
}

