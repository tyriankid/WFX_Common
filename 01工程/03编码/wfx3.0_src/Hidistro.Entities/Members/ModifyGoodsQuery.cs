namespace Hidistro.Entities.Members
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ModifyGoodsQuery 
    {
        public int CommodityID{ get; set; }

        public string CommoditySource { get; set; }

        public string CommodityCode { get; set; }
    }
}

