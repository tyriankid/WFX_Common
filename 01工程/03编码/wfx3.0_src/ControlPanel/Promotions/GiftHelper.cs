namespace Hidistro.ControlPanel.Promotions
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Promotions;
    using Hidistro.SqlDal.Promotions;
    using System;

    public static class GiftHelper
    {
        public static GiftActionStatus AddGift(GiftInfo gift)
        {
            Globals.EntityCoding(gift, true);
            return new GiftDao().CreateUpdateDeleteGift(gift, DataProviderAction.Create);
        }

        public static bool DeleteGift(int giftId)
        {
            GiftInfo gift = new GiftInfo {
                GiftId = giftId
            };
            return (new GiftDao().CreateUpdateDeleteGift(gift, DataProviderAction.Delete) == GiftActionStatus.Success);
        }

        public static GiftInfo GetGiftDetails(int giftId)
        {
            return new GiftDao().GetGiftDetails(giftId);
        }

        public static DbQueryResult GetGifts(GiftQuery query)
        {
            return new GiftDao().GetGifts(query);
        }

        public static GiftActionStatus UpdateGift(GiftInfo gift)
        {
            Globals.EntityCoding(gift, true);
            return new GiftDao().CreateUpdateDeleteGift(gift, DataProviderAction.Update);
        }

        public static bool UpdateIsDownLoad(int giftId)
        {
            return new GiftDao().UpdateIsDownLoad(giftId);
        }
    }
}

