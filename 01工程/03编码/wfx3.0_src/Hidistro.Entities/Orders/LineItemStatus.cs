namespace Hidistro.Entities.Orders
{
    using System;
    using System.ComponentModel;

    public enum LineItemStatus
    {
        [Description("退货用户已发货")]
        DeliveryForReturn = 0x16,
        [Description("退货平台已收货")]
        GetGoodsForReturn = 0x17,
        [Description("换货待用户发货")]
        MerchantsAgreedForReplace = 0x1f,
        [Description("退货待用户发货")]
        MerchantsAgreedForReturn = 0x15,
        [Description("换货平台已发货")]
        MerchantsDeliveryForRepalce = 0x21,
        [Description("正常状态")]
        Normal = 0,
        [Description("申请退款中")]
        RefundApplied = 10,
        [Description("退款已完成")]
        Refunded = 11,
        [Description("退款被拒绝")]
        RefundRefused = 12,
        [Description("申请换货中")]
        ReplaceApplied = 30,
        [Description("换货完成")]
        Replaced = 0x22,
        [Description("拒绝换货")]
        ReplaceRefused = 0x23,
        [Description("申请退货中")]
        ReturnApplied = 20,
        [Description("退货完成")]
        Returned = 0x18,
        [Description("退货被拒绝")]
        ReturnsRefused = 0x19,
        [Description("换货用户已发货")]
        UserDeliveryForReplace = 0x20
    }
}

