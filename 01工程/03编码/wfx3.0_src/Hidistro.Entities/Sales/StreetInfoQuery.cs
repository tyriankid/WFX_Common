namespace Hidistro.Entities.Sales
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class StreetInfoQuery : Pagination
    {
        public string streetName { get; set; }

        public string regionCode { get; set; }

        public int distributorId { get; set; }
        /// <summary>
        /// 是否用户绑定列表查询,若是,过滤掉已绑定的街道 ,若不是,查询所有
        /// </summary>
        public bool isUserBind { get; set; }
    }
}

