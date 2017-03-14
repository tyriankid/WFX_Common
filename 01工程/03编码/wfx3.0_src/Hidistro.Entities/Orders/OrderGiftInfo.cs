using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Orders
{
    public class OrderGiftInfo
    {
        public decimal CostPrice { get; set; }

        public int costPoint { get; set; }

        public int GiftId { get; set; }

        public string GiftName { get; set; }

        public string OrderId { get; set; }

        public int PromoteType { get; set; }

        public int Quantity { get; set; }

        public string ThumbnailsUrl { get; set; }
    }
}
