namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class GiftQuery
    {
        public GiftQuery()
        {
            this.Page = new Pagination();
        }

        public bool IsOnline { get; set; }

        public bool IsPromotion { get; set; }

        public string Name { get; set; }

        public Pagination Page { get; set; }
    }
}

