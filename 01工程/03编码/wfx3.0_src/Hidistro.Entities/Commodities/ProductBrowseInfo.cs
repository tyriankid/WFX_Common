namespace Hidistro.Entities.Commodities
{
    using Hidistro.Entities.Comments;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.CompilerServices;

    public class ProductBrowseInfo
    {
        public string BrandName { get; set; }

        public string CategoryName { get; set; }

        public int ConsultationCount { get; set; }

        public DataTable DbAttribute { get; set; }

        public DataTable DBConsultations { get; set; }

        public DataTable DbCorrelatives { get; set; }

        public DataTable DBReviews { get; set; }

        public DataTable DbSKUs { get; set; }

        //public IList<ViewAttributeInfo> ListAttribute { get; set; }

        public IList<ProductConsultationInfo> ListConsultations { get; set; }

        public IList<ProductInfo> ListCorrelatives { get; set; }

        public IList<ProductReviewInfo> ListReviews { get; set; }

        public IList<SKUItem> ListSKUs { get; set; }

        public ProductInfo Product { get; set; }

        public int ReviewCount { get; set; }
    }
}

