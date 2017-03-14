namespace Hidistro.SalePromotion.Reduce
{
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Promotions;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class eggHalfPrice:ReduceInfo
    {
        public decimal getReduceAdjustPrice()
        {
            ////爽爽挝啡捆绑销售挝蛋蛋半价处理,订单中包含一杯咖啡,另一个挝蛋蛋半价
            //int coffeeCount = 0;
            //foreach (LineItemInfo infoCoffee in orderInfo.LineItems.Values)
            //{
            //    //获取订单中咖啡数量(蛋蛋需要半价的数量
            //    if (CategoryBrowser.GetCategory(infoCoffee.MainCategoryPath).Name.IndexOf("挝啡") > -1)
            //    {
            //        coffeeCount = infoCoffee.Quantity;
            //    }
            //}

            ////蛋蛋半价处理
            //foreach (ShoppingCartItemInfo infoEgg in shoppingCart.LineItems)
            //{
            //    if (infoEgg.Name == "挝蛋蛋" && coffeeCount > 0 && context.Request["EggHalfPrice"] == "on")
            //    {
            //        infoEgg.HalfPriceQuantity = coffeeCount;
            //    }
            //    else
            //    {
            //        continue;
            //    }
            //}
            return 0M;
        }
    }
}

