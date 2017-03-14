namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class ProductInfo
    {
        private SKUItem defaultSku;
        private Dictionary<string, SKUItem> skus;

        public DateTime AddedDate { get; set; }

        public string BrandId { get; set; }

        public int CategoryId { get; set; }

        public decimal CostPrice
        {
            get
            {
                return this.DefaultSku.CostPrice;
            }
        }

        public SKUItem DefaultSku
        {
            get
            {
                return (this.defaultSku ?? (this.defaultSku = this.Skus.Values.First<SKUItem>()));
            }
        }

        public string Description { get; set; }

        public int DisplaySequence { get; set; }

        public string ExtendCategoryPath { get; set; }

        public bool HasSKU { get; set; }

        public string ImageUrl1 { get; set; }

        public string ImageUrl2 { get; set; }

        public string ImageUrl3 { get; set; }

        public string ImageUrl4 { get; set; }

        public string ImageUrl5 { get; set; }

        public bool IsfreeShipping { get; set; }

        public string MainCategoryPath { get; set; }

        public decimal? MarketPrice { get; set; }

        public decimal MaxSalePrice
        {
            get
            {
                decimal[] maxSalePrice = new decimal[1];
                foreach (SKUItem item in from sku in this.Skus.Values
                    where sku.SalePrice > maxSalePrice[0]
                    select sku)
                {
                    maxSalePrice[0] = item.SalePrice;
                }
                return maxSalePrice[0];
            }
        }

        public decimal MinSalePrice
        {
            get
            {
                decimal[] minSalePrice = new decimal[] { 79228162514264337593543950335M };
                foreach (SKUItem item in from sku in this.Skus.Values
                    where sku.SalePrice < minSalePrice[0]
                    select sku)
                {
                    minSalePrice[0] = item.SalePrice;
                }
                return minSalePrice[0];
            }
        }
        public decimal MinCostPrice
        {
            get
            {
                decimal[] minCostPrice = new decimal[] { 79228162514264337593543950335M };
                foreach (SKUItem item in from sku in this.Skus.Values
                                         where sku.CostPrice < minCostPrice[0]
                                         select sku)
                {
                    minCostPrice[0] = item.CostPrice;
                }
                return minCostPrice[0];
            }
        }

        public string ProductCode { get; set; }

        public int ProductId { get; set; }

        [HtmlCoding]
        public string ProductName { get; set; }

        public int SaleCounts { get; set; }

        public decimal SalePrice { get; set; }

        public ProductSaleStatus SaleStatus { get; set; }

        [HtmlCoding]
        public string ShortDescription { get; set; }

        public int ShowSaleCounts { get; set; }

        public string SKU
        {
            get
            {
                return this.DefaultSku.SKU;
            }
        }

        public string SkuId
        {
            get
            {
                return this.DefaultSku.SkuId;
            }
        }

        public Dictionary<string, SKUItem> Skus
        {
            get
            {
                return (this.skus ?? (this.skus = new Dictionary<string, SKUItem>()));
            }
        }

        public int Stock
        {
            get
            {
                return this.Skus.Values.Sum<SKUItem>(((Func<SKUItem, int>) (sku => sku.Stock)));
            }
        }

        public long TaobaoProductId { get; set; }

        public string ThumbnailUrl100 { get; set; }

        public string ThumbnailUrl160 { get; set; }

        public string ThumbnailUrl180 { get; set; }

        public string ThumbnailUrl220 { get; set; }

        public string ThumbnailUrl310 { get; set; }

        public string ThumbnailUrl40 { get; set; }

        public string ThumbnailUrl410 { get; set; }

        public string ThumbnailUrl60 { get; set; }

        public int? TypeId { get; set; }

        public string Unit { get; set; }

        public int VistiCounts { get; set; }

        public decimal Weight
        {
            get
            {
                return this.DefaultSku.Weight;
            }
        }

        public int Range { get; set; }

        public int StoreId { get; set; }

        public int ReviewState { get; set; }

        /// <summary>
        /// 产品Top枚举
        /// </summary>
        public enum ProductTop {
            /// <summary>
            /// 上新
            /// </summary>
            New,
            /// <summary>
            /// 热门
            /// </summary>
            Hot,
            /// <summary>
            /// 优惠
            /// </summary>
            Discount,
            /// <summary>
            /// 最喜欢
            /// </summary>
            MostLike,
            /// <summary>
            /// 活动商品
            /// </summary>
            Activity,
            /// <summary>
            /// 分类
            /// </summary>
            Category,
            /// <summary>
            /// 无
            /// </summary>
            No,
        }

        /// <summary>
        /// 产品限定范围枚举
        /// </summary>
        public enum ProductRanage
        {
            /// <summary>
            /// 正常显示店铺已上架的商品
            /// </summary>
            NormalSelect,
            /// <summary>
            /// 正常显示店铺未上架的商品
            /// </summary>
            NormalUnSelect,
            /// <summary>
            /// 显示所有出售状态的商品
            /// </summary>
            All,
            /// <summary>
            /// 根据上架范围显示已上架的商品
            /// </summary>
            RangeSelect,
            /// <summary>
            /// 根据上架范围显示未上架的商品
            /// </summary>
            RangeUnSelect,
        }


    }
}

