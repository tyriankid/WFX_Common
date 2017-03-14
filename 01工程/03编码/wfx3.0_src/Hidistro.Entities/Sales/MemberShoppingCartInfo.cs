namespace Hidistro.Entities.Sales
{
    using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

    public class MemberShoppingCartInfo
    {
        public MemberShoppingCartInfo(MemberInfo currentMember, List<ShoppingCartInfo> carts)
        {
            CurrentMember = currentMember;
            Carts = carts;
        }
        public MemberInfo CurrentMember { get; set; }
        public List<ShoppingCartInfo> Carts { get; set; }

    }
}

