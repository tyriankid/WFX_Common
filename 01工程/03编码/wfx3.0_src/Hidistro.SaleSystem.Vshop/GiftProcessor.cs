namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Orders;
    using Hidistro.SqlDal.Promotions;
    using Hidistro.SqlDal.Sales;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public static class GiftProcessor
    {
        /// <summary>
        /// add by hj 20150918 增加礼品
        /// </summary>
        /// <param name="giftId"></param>
        /// <param name="quantity"></param>
        /// <param name="promotype"></param>
        /// <returns></returns>
        public static bool AddGiftItem(int giftId, int quantity)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (quantity <= 0)
            {
                quantity = 1;
            }
            return new ShoppingCartDao().AddGiftItem(currentMember,giftId, quantity);
        }

        /// <summary>
        /// 20150921新增:通过giftID找到库存
        /// </summary>
        /// <param name="skuId"></param>
        public static int GetGiftStock(int giftId)
        {
            return new GiftDao().GetGiftDetails(giftId).Stock;
        }

        public static DataTable GetOrderGiftsThumbnailsUrl(string orderId)
        {
            return new OrderGiftDao().GetOrderGiftsThumbnailsUrl(orderId);
        }

        public static DataSet GetUserOrderGift(string orderId)
        {
            return new OrderDao().GetUserOrderGift(orderId);
        }
    }
}

