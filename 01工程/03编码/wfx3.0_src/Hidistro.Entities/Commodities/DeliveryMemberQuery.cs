namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class DeliveryMemberQuery : Pagination
    {
        public DateTime? EndDate { get; set; }

        [HtmlCoding]
        public string UserName { get; set; }

        public DateTime? StartDate { get; set; }

        public int DeliveryUserId { get; set; }

        public int DeliveryState { get; set; }

        public int StoreId { get; set; }

        public int Sex { get; set; }

        public int State { get; set; }
    }
}

