namespace Hidistro.Entities.VShop
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class WhoKnowMeQuery : Pagination
    {
        public Guid WhowKnowMeId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}

