namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core.Entities;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ProductBrowseQuery : Pagination
    {
        private IList<AttributeValueInfo> attributeValues;
        private Hidistro.Entities.Commodities.ProductSaleStatus productSaleStatus = Hidistro.Entities.Commodities.ProductSaleStatus.OnSale;

        public IList<AttributeValueInfo> AttributeValues
        {
            get
            {
                if (this.attributeValues == null)
                {
                    this.attributeValues = new List<AttributeValueInfo>();
                }
                return this.attributeValues;
            }
            set
            {
                this.attributeValues = value;
            }
        }

        public int? BrandId { get; set; }

        public int? CategoryId { get; set; }

        public bool IsPrecise { get; set; }

        public string Keywords { get; set; }

        public decimal? MaxSalePrice { get; set; }

        public decimal? MinSalePrice { get; set; }

        public string ProductCode { get; set; }

        public Hidistro.Entities.Commodities.ProductSaleStatus ProductSaleStatus
        {
            get
            {
                return this.productSaleStatus;
            }
            set
            {
                this.productSaleStatus = value;
            }
        }

        public string TagIds { get; set; }
    }
}

