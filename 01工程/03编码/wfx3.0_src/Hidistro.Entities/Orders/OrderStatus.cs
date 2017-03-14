namespace Hidistro.Entities.Orders
{
    using System;

    public enum OrderStatus
    {
        All = 0,
        ApplyForRefund = 6,
        ApplyForReplacement = 8,
        ApplyForReturns = 7,
        BuyerAlreadyPaid = 2,
        Closed = 4,
        Finished = 5,
        History = 0x63,
        Refunded = 9,
        Returned = 10,
        SellerAlreadySent = 3,
        Today = 11,
        WaitBuyerPay = 1,
        Delete =88,
        //xx
        DeliveryingForReturns = 13,
        GetGoodsForReturns = 14,
        MerchantsAgreedFoReturns = 12,
        MerchantsAgreedForReplace = 15,
        MerchantsDeliveryForReplace = 0x11,
        RefundRefused = 0x12,
        Replaced = 11,
        ReplaceRefused = 20,
        ReturnRefused = 0x13,
        UserDeliveryForReplace = 0x10
    }
}

