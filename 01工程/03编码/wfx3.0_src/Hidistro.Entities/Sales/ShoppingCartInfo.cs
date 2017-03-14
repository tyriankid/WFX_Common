namespace Hidistro.Entities.Sales
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ShoppingCartInfo
    {
        private bool isSendGift;
        private IList<ShoppingCartItemInfo> lineItems;
        private IList<ShoppingCartGiftInfo> lineGifts;
        private decimal timesPoint = 1M;

        public decimal GetAmount()
        {
            decimal num = 0M;
            foreach (ShoppingCartItemInfo info in this.lineItems)
            {
                num += info.SubTotal;
            }
            return num;
        }

        public IList<ShoppingCartGiftInfo> LineGifts
        {
            get
            {
                if (this.lineGifts == null)
                {
                    this.lineGifts = new List<ShoppingCartGiftInfo>();
                }
                return this.lineGifts;
            }
        }

        public int GetPoint()
        {
            int num = 0;
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if (((this.GetTotal() * this.TimesPoint) / masterSettings.PointsRate) > 2147483647M)
            {
                return 0x7fffffff;
            }
            if (masterSettings.PointsRate != 0M)
            {
                num = (int) Math.Round((decimal) ((this.GetTotal() * this.TimesPoint) / masterSettings.PointsRate), 0);
            }
            return num;
        }

        /// <summary>
        /// 获取总共需要的积分
        /// </summary>
        /// <returns></returns>
        public int GetTotalNeedPoint()
        {
            int num = 0;
            if ((this.LineGifts != null) && (this.LineGifts.Count != 0))
            {
                foreach (ShoppingCartGiftInfo info in this.LineGifts)
                {
                    num += info.SubPointTotal;
                }
            }
            return num;
        }

        public int GetPoint(decimal money)
        {
            int num = 0;
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if (((money * this.TimesPoint) / masterSettings.PointsRate) > 2147483647M)
            {
                return 0x7fffffff;
            }
            if (masterSettings.PointsRate != 0M)
            {
                num = (int) Math.Round((decimal) ((money * this.TimesPoint) / masterSettings.PointsRate), 0);
            }
            return num;
        }

        public int GetQuantity()
        {
            int num = 0;
            foreach (ShoppingCartItemInfo info in this.lineItems)
            {
                num += info.Quantity;
            }
            return num;
        }
        public int GetGiftQuantity()
        {
            int num = 0;
            foreach (ShoppingCartGiftInfo info in this.lineGifts)
            {
                num += info.Quantity;
            }
            return num;
        }

        public decimal GetTotal()
        {
            return (this.GetAmount() - this.ReducedPromotionAmount);
        }

        public int CategoryId { get; set; }

        public int FreightFreePromotionId { get; set; }

        public string FreightFreePromotionName { get; set; }

        public bool IsFreightFree { get; set; }

        public bool IsReduced { get; set; }

        public bool IsSendGift
        {
            get
            {
                foreach (ShoppingCartItemInfo info in this.lineItems)
                {
                    if (info.IsSendGift)
                    {
                        return true;
                    }
                }
                return this.isSendGift;
            }
            set
            {
                this.isSendGift = value;
            }
        }

        public bool IsSendTimesPoint { get; set; }

        public IList<ShoppingCartItemInfo> LineItems
        {
            get
            {
                if (this.lineItems == null)
                {
                    this.lineItems = new List<ShoppingCartItemInfo>();
                }
                return this.lineItems;
            }
        }

        public decimal ReducedPromotionAmount { get; set; }

        public int ReducedPromotionId { get; set; }

        public string ReducedPromotionName { get; set; }

        public int SendGiftPromotionId { get; set; }

        public string SendGiftPromotionName { get; set; }

        public int SentTimesPointPromotionId { get; set; }

        public string SentTimesPointPromotionName { get; set; }

        public decimal TimesPoint
        {
            get
            {
                return this.timesPoint;
            }
            set
            {
                this.timesPoint = value;
            }
        }

        public decimal TotalWeight
        {
            get
            {
                decimal num = 0M;
                foreach (ShoppingCartItemInfo info in this.lineItems)
                {
                    num += info.GetSubWeight();
                }
                return num;
            }
        }

        public decimal Weight
        {
            get
            {
                decimal num = 0M;
                foreach (ShoppingCartItemInfo info in this.lineItems)
                {
                    if (!info.IsfreeShipping)
                    {
                        num += info.GetSubWeight();
                    }
                }
                return num;
            }
        }
    }
}

