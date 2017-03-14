namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class CountDownInfo
    {
        public string Content { get; set; }

        public int CountDownId { get; set; }

        public decimal CountDownPrice { get; set; }

        public int DisplaySequence { get; set; }

        public DateTime EndDate { get; set; }

        public int MaxCount { get; set; }

        public int ProductId { get; set; }

        public DateTime StartDate { get; set; }
    }
}

