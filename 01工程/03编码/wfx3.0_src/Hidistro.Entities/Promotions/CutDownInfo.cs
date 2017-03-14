namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class CutDownInfo
    {
        private IList<CutDownDetailInfo> cutDownDetails;

        [HtmlCoding]
        public string Content { get; set; }//描述

        public int Count { get; set; }//商品最大购买数量(库存)

        public DateTime EndDate { get; set; }

        public DateTime StartDate { get; set; }

        public IList<CutDownDetailInfo> CutDownDetails
        {
            get
            {
                if (this.cutDownDetails == null)
                {
                    this.cutDownDetails = new List<CutDownDetailInfo>();
                }
                return this.cutDownDetails;
            }
        }

        public int ProdcutQuantity { get; set; }
        public int CutDownId { get; set; }//砍价id

        public int MaxCount { get; set; }//最多砍价次数

        public int ProductId { get; set; }//商品id

        public int SoldCount { get; set; }//活动产品售出订单数量

        public decimal PerCutPrice { get; set; }//每次砍价减少价格

        public decimal FirstPrice { get; set; }//初始价格

        public decimal CurrentPrice { get; set; }//商品当前价格

        public int CurrentCutCount { get; set; }//当前商品被砍价次数

        public decimal MinPrice { get; set; }//最低价

        public CutDownStatus Status { get; set; }
    }
}

