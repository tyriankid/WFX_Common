using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Hidistro.Core.Entities
{
    public class ImportOfProductsQuery : Pagination
    {
        public int CommodityID { get; set; }

        public string CommoditySource{ get; set; }

        public string CommodityCode { get; set; }

        public int? CategoryId { get; set; }

        public string MaiCategoryPath { get; set; }

        public int? BrandId { get; set; }

        public int? TypeId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
