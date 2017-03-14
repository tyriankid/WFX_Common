﻿namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductQuery : Pagination
    {
        public int? BrandId { get; set; }

        public int? CategoryId { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? IsIncludeHomeProduct { get; set; }

        public bool? IsIncludePromotionProduct { get; set; }

        public int? IsMakeTaobao { get; set; }

        [HtmlCoding]
        public string Keywords { get; set; }

        public string MaiCategoryPath { get; set; }

        public decimal? MaxSalePrice { get; set; }

        public decimal? MinSalePrice { get; set; }

        [HtmlCoding]
        public string ProductCode { get; set; }

        public ProductSaleStatus SaleStatus { get; set; }

        public DateTime? StartDate { get; set; }

        public int? TagId { get; set; }

        public int? TopicId { get; set; }

        public int? TypeId { get; set; }

        public string ProductIds { get; set; }

        public int ProductId { get; set; }

        public int StoreId { get; set; }

        public int ReviewState { get; set; }

        public string SkuIds { get; set; }

        public string CommoditySource { get; set; }

        public string CommodityCode { get; set; }

        public string ProductName { get; set; }
    }
}

