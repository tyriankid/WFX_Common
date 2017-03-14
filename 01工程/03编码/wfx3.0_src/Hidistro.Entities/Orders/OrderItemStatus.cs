namespace Hidistro.Entities.Orders
{
    using System;
    using System.ComponentModel;

    public enum OrderItemStatus
    {
        [Description("所有商品售后中")]
        AllInAfterSales = 5,
        [Description("已退款/退货完成")]
        AllRefunedOrReturned = 6,
        [Description("正在退款")]
        HasRefund = 1,
        [Description("正在退款/退货/换货")]
        HasRefundOrReturnOrReplace = 4,
        [Description("正在换货")]
        HasReplace = 3,
        [Description("正在退货")]
        HasReturn = 2,
        [Description("正常状态")]
        Nomarl = 0
    }
}

