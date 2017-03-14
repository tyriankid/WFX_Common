namespace Hidistro.Entities.Orders
{
    using Hidistro.Core.Entities;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class OrderQuery : Pagination
    {
        public DateTime? EndDate { get; set; }

        public int? GroupBuyId { get; set; }

        public int Quantity { get; set; }

        public int GiveQuantity { get; set; }

        public int? IsPrinted { get; set; }

        public string OrderId { get; set; }

        public int? PaymentType { get; set; }

        public string PayTypeName { get; set; }

        public string ProductName { get; set; }

        public int? RegionId { get; set; }

        public string ShipId { get; set; }

        public int? ShippingModeId { get; set; }

        public string ShipTo { get; set; }

        public DateTime? StartDate { get; set; }

        public OrderStatus Status { get; set; }

        public OrderType? Type { get; set; }

        public int? UserId { get; set; }

        public string UserName { get; set; }

        public string StoreName { get; set; }

        public List<string> customKeyword { get; set; }

        public string ExpressCompanyName{ get; set; }
        public enum OrderType
        {
            GroupBuy = 2,
            NormalProduct = 1
        }

        public int? OrderAgent { get; set; }

        public int? AgentUserId { get; set; }

        public string RealName { get; set; }

        public string modeName { get; set; }

        public int? referralUserId { get; set; }

        public string ChannelName { get; set; }

        public string Sender { get; set; }
        public int? selectAgentId { get; set; }
        public int ClientUserId { get; set; }
        public string StoreType { get; set; }
    }
}

