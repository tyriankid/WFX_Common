namespace Hidistro.Entities.Orders
{
    using System;
    using System.Runtime.CompilerServices;

    public class ReplaceInfo
    {
        public string AdminRemark { get; set; }

        public string AdminShipAddress { get; set; }

        public DateTime ApplyForTime { get; set; }

        public string Comments { get; set; }

        public string ExpressCompanyAbb { get; set; }

        public string ExpressCompanyName { get; set; }

        public ReplaceStatus HandleStatus { get; set; }

        public DateTime HandleTime { get; set; }

        public string OrderId { get; set; }

        public int Quantity { get; set; }

        public int ReplaceId { get; set; }

        public string ReplaceReason { get; set; }

        public string ShipOrderNumber { get; set; }

        public string ShopName { get; set; }

        public string SkuId { get; set; }

        public int? StoreId { get; set; }

        public string UserCredentials { get; set; }

        public string UserExpressCompanyAbb { get; set; }

        public string UserExpressCompanyName { get; set; }

        public string UserShipOrderNumber { get; set; }
    }
}

