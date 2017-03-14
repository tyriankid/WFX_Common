namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class CutDownDetailsQuery : Pagination
    {
        public int DetailId { get; set; }
        public int CutDownId { get; set; }

        public int MemberId { get; set; }

        public string CutTime { get; set; }

        public decimal CutDownPrice { get; set; }
    }
}

