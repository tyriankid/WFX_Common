using Hidistro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Hidistro.SqlDal.Promotions
{
    public class OrderGiftQuery
    {
        public OrderGiftQuery()
        {
            this.Page = new Pagination();
        }

        public string OrderId { get; set; }

        public Pagination Page { get; set; }
    }
}
