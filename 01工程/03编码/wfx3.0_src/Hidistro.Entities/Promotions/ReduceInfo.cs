namespace Hidistro.Entities.Promotions
{
    using Hidistro.Entities.Orders;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ReduceInfo
    {
        /// <summary>
        /// 商品原价
        /// </summary>
        public decimal ItemOriginalPrice { get; set; }

        /// <summary>
        /// 商品活动后价格
        /// </summary>
        public decimal ItemAdjustPrice { get; set; }

        /// <summary>
        /// 活动名
        /// </summary>
        public string ReduceName { get; set; }

        /// <summary>
        /// 活动描述
        /// </summary>
        public string ReduceDescription { get; set; }

        /// <summary>
        /// 订单信息
        /// </summary>
        public OrderInfo orderInfo { get; set; }
    }
}

