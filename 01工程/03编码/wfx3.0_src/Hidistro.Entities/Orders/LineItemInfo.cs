namespace Hidistro.Entities.Orders
{
    using System;
    using System.Runtime.CompilerServices;

    public class LineItemInfo
    {
        public decimal GetSubTotal()
        {
            return (this.ItemAdjustedPrice * (this.Quantity - this.GiveQuantity) - (this.HalfPriceQuantity * this.ItemAdjustedPrice) / 2);
        }

        public decimal GetSubShowTotal()
        {
            return (this.ItemAdjustedPrice * (this.Quantity));
        }

        /// <summary>
        /// Add: JHB,20150817，计算利润
        /// </summary>
        /// <returns></returns>
        public decimal GetSubTotalProfit()
        {
            return ((this.ItemAdjustedPrice - this.ItemCostPrice) * (this.Quantity - this.GiveQuantity) - (this.HalfPriceQuantity*this.ItemAdjustedPrice)/2);
        }

        /// <summary>
        /// Add:HJ,20151023, 根据成本价计算
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
        public decimal GetSubTotalByCostPrice()
        {
            return (this.ItemCostPrice * (this.Quantity));
        }

        /// <summary>
        /// Add:jh,20151112, 根据特殊出货折扣比例计算
        /// </summary>
        /// <returns></returns>
        public decimal GetSubTotalBySpecialRate(decimal rate)
        {
            return ((this.ItemAdjustedPrice - this.ItemAdjustedPrice * rate) * (this.Quantity - this.GiveQuantity) - (this.HalfPriceQuantity * this.ItemAdjustedPrice) / 2);
        }


        public decimal ItemAdjustedCommssion { get; set; }

        public decimal ItemAdjustedPrice { get; set; }

        public decimal ItemCostPrice { get; set; }

        public string ItemDescription { get; set; }

        public decimal ItemListPrice { get; set; }

        public decimal ItemsCommission { get; set; }

        public decimal ItemWeight { get; set; }

        public string MainCategoryPath { get; set; }

        public OrderStatus OrderItemsStatus { get; set; }

        public int ProductId { get; set; }

        public int PromotionId { get; set; }

        public string PromotionName { get; set; }

        public int Quantity { get; set; }

        public int GiveQuantity { get; set; }

        public int HalfPriceQuantity { get; set; }

        public decimal SecondItemsCommission { get; set; }

        public int ShipmentQuantity { get; set; }

        public string SKU { get; set; }

        public string SKUContent { get; set; }

        public string SkuId { get; set; }

        public decimal ThirdItemsCommission { get; set; }

        public LineItemStatus Status { get; set; }

        public string StatusText
        {
            get
            {
                return EnumDescription.GetEnumDescription(this.Status);
            }
        }

        public string ThumbnailsUrl { get; set; }
    }
}

