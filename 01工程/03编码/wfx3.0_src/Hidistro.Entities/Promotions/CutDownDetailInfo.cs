namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class CutDownDetailInfo
    {
        public int CutDownDetailId { get; set; }

        public int CutDownId { get; set; }

        public int MemberId { get; set; }

        public DateTime CutTime { get; set; }

        public decimal CutDownPrice { get; set; }
    }
}

