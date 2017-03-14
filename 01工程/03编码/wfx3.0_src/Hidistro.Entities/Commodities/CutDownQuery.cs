namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class CutDownQuery : Pagination
    {
        public int CutDownId { get; set; }

        public string ProductName { get; set; }

        public int State { get; set; }
    }
}

