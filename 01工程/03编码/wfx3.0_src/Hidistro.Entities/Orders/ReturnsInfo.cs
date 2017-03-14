namespace Hidistro.Entities.Orders
{
    using System;
    using System.Runtime.CompilerServices;

    public class ReturnsInfo
    {
        public string AdminRemark { get; set; }

        public string AdminShipAddress { get; set; }

        public DateTime ApplyForTime { get; set; }

        public string Comments { get; set; }

        public string ExpressCompanyAbb { get; set; }

        public string ExpressCompanyName { get; set; }

        //public ReturnsStatus HandleStatus { get; set; }

        public DateTime HandleTime { get; set; }

        public string Operator { get; set; }

        public string OrderId { get; set; }

        public int Quantity { get; set; }

        public string RefundGateWay { get; set; }

        public decimal RefundMoney { get; set; }

        public string RefundOrderId { get; set; }

        //public RefundTypes RefundType { get; set; }

        public string ReturnReason { get; set; }

        public int ReturnsId { get; set; }

        public string ShipOrderNumber { get; set; }

        public string ShopName { get; set; }

        public string SkuId { get; set; }

        public int? StoreId { get; set; }

        public string UserCredentials { get; set; }
    }
}

