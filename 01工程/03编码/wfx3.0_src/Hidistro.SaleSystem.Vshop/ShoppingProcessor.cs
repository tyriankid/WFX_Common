namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.SqlDal;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Members;
    using Hidistro.SqlDal.Orders;
    using Hidistro.SqlDal.Promotions;
    using Hidistro.SqlDal.Sales;
    using Hidistro.SqlDal.VShop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;

    public static class ShoppingProcessor
    {
        private static object createOrderLocker = new object();

        public static decimal CalcFreight(int regionId, decimal totalWeight, ShippingModeInfo shippingModeInfo)
        {
            decimal price = 0M;
            int topRegionId = RegionHelper.GetTopRegionId(regionId);
            decimal num3 = totalWeight;
            int num4 = 1;
            if ((num3 > shippingModeInfo.Weight) && (shippingModeInfo.AddWeight.HasValue && (shippingModeInfo.AddWeight.Value > 0M)))
            {
                decimal num6 = num3 - shippingModeInfo.Weight;
                if ((num6 % shippingModeInfo.AddWeight) == 0M)
                {
                    num4 = Convert.ToInt32(Math.Truncate((decimal)((num3 - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value)));
                }
                else
                {
                    num4 = Convert.ToInt32(Math.Truncate((decimal)((num3 - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value))) + 1;
                }
            }
            if ((shippingModeInfo.ModeGroup == null) || (shippingModeInfo.ModeGroup.Count == 0))
            {
                if ((num3 > shippingModeInfo.Weight) && shippingModeInfo.AddPrice.HasValue)
                {
                    return ((num4 * shippingModeInfo.AddPrice.Value) + shippingModeInfo.Price);
                }
                return shippingModeInfo.Price;
            }
            int? nullable = null;
            foreach (ShippingModeGroupInfo info in shippingModeInfo.ModeGroup)
            {
                foreach (ShippingRegionInfo info2 in info.ModeRegions)
                {
                    if (topRegionId == info2.RegionId)
                    {
                        nullable = new int?(info2.GroupId);
                        break;
                    }
                }
                if (nullable.HasValue)
                {
                    if (num3 > shippingModeInfo.Weight)
                    {
                        price = (num4 * info.AddPrice) + info.Price;
                    }
                    else
                    {
                        price = info.Price;
                    }
                    break;
                }
            }
            if (nullable.HasValue)
            {
                return price;
            }
            if ((num3 > shippingModeInfo.Weight) && shippingModeInfo.AddPrice.HasValue)
            {
                return ((num4 * shippingModeInfo.AddPrice.Value) + shippingModeInfo.Price);
            }
            return shippingModeInfo.Price;
        }

        public static decimal CalcPayCharge(decimal cartMoney, PaymentModeInfo paymentModeInfo)
        {
            if (!paymentModeInfo.IsPercent)
            {
                return paymentModeInfo.Charge;
            }
            return (cartMoney * (paymentModeInfo.Charge / 100M));
        }

        private static void checkCanGroupBuy(int quantity, int groupBuyId)
        {
            GroupBuyInfo groupBuy = GroupBuyBrowser.GetGroupBuy(groupBuyId);
            if (groupBuy.Status != GroupBuyStatus.UnderWay)
            {
                throw new OrderException("当前团购状态不允许购买");
            }
            if ((groupBuy.StartDate > DateTime.Now) || (groupBuy.EndDate < DateTime.Now))
            {
                throw new OrderException("当前不在团购时间范围内");
            }
            int num = groupBuy.MaxCount - groupBuy.SoldCount;
            if (quantity > num)
            {
                throw new OrderException("剩余可购买团购数量不够");
            }
        }

        public static OrderInfo ConvertShoppingCartToOrder(ShoppingCartInfo shoppingCart, bool isCountDown, bool isSignBuy,bool isAgent)
        {
            if (shoppingCart.LineItems.Count == 0 && shoppingCart.LineGifts.Count==0)
            {
                return null;
            }
            OrderInfo info = new OrderInfo
            {
                Points = shoppingCart.GetPoint(),
                ReducedPromotionId = shoppingCart.ReducedPromotionId,
                ReducedPromotionName = shoppingCart.ReducedPromotionName,
                ReducedPromotionAmount = shoppingCart.ReducedPromotionAmount,
                IsReduced = shoppingCart.IsReduced,
                SentTimesPointPromotionId = shoppingCart.SentTimesPointPromotionId,
                SentTimesPointPromotionName = shoppingCart.SentTimesPointPromotionName,
                IsSendTimesPoint = shoppingCart.IsSendTimesPoint,
                TimesPoint = shoppingCart.TimesPoint,
                FreightFreePromotionId = shoppingCart.FreightFreePromotionId,
                FreightFreePromotionName = shoppingCart.FreightFreePromotionName,
                IsFreightFree = shoppingCart.IsFreightFree
            };
            string str = string.Empty;
            if (shoppingCart.LineItems.Count > 0)
            {
                foreach (ShoppingCartItemInfo info2 in shoppingCart.LineItems)
                {
                    str = str + string.Format("'{0}',", info2.SkuId);
                }
            }
            if (shoppingCart.LineItems.Count > 0)
            {
                foreach (ShoppingCartItemInfo info2 in shoppingCart.LineItems)
                {
                    decimal costprice = new SkuDao().GetSkuItem(info2.SkuId).CostPrice;
                    LineItemInfo info3 = new LineItemInfo
                    {
                        SkuId = info2.SkuId,
                        ProductId = info2.ProductId,
                        SKU = info2.SKU,
                        Quantity = info2.Quantity,
                        ShipmentQuantity = info2.ShippQuantity,
                        ItemCostPrice = costprice,
                        ItemListPrice = isAgent ? costprice : info2.MemberPrice,
                        ItemAdjustedPrice = isAgent ? costprice : info2.AdjustedPrice,
                        ItemDescription = info2.Name,
                        ThumbnailsUrl = info2.ThumbnailUrl40,
                        ItemWeight = info2.Weight,
                        SKUContent = info2.SkuContent,
                        PromotionId = info2.PromotionId,
                        PromotionName = info2.PromotionName,
                        MainCategoryPath = info2.MainCategoryPath,
                        GiveQuantity = info2.GiveQuantity,
                        HalfPriceQuantity = info2.HalfPriceQuantity
                    };
                    info.LineItems.Add(info3.SkuId, info3);
                }
            }
            //如果有礼品在购物车内,增加礼品到订单
            if (shoppingCart.LineGifts.Count > 0)
            {
                foreach (ShoppingCartGiftInfo info4 in shoppingCart.LineGifts)
                {
                    OrderGiftInfo item = new OrderGiftInfo
                    {
                        GiftId = info4.GiftId,
                        GiftName = info4.Name,
                        Quantity = info4.Quantity,
                        ThumbnailsUrl = info4.ThumbnailUrl100,
                        CostPrice = info4.CostPrice,
                        costPoint = info4.NeedPoint*info4.Quantity
                    };
                    info.Gifts.Add(item);
                }
            }
            //如果当前订单内商品数量为零的话,则订单状态为已付款
            if ((info.GetTotal() == 0M) && (info.LineItems.Count == 0))
            {
                info.OrderStatus = OrderStatus.BuyerAlreadyPaid;
            }
            info.Tax = 0.00M;
            info.InvoiceTitle = "";
            return info;
        }

        /// <summary>
        /// add@20150921 by jh
        /// 提交订单时,扣除积分
        /// </summary>
        /// <param name="needPoint"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static bool CutNeedPoint(int needPoint, string orderId)
        {
            MemberInfo user = MemberProcessor.GetCurrentMember();
            PointDetailInfo point = new PointDetailInfo
            {
                OrderId = orderId,
                UserId = user.UserId,
                TradeDate = DateTime.Now,
                TradeType = PointTradeType.Change,
                Reduced = new int?(needPoint),
                Points = user.Points - needPoint
            };
            if ((point.Points > 0x7fffffff) || (point.Points < 0))
            {
                point.Points = 0;
            }
            if (new PointDetailDao().AddPointDetail(point))
            {
                return true;
            }
            return false;
        }

        public static bool CreatOrder(OrderInfo orderInfo)
        {
            bool flag = false;
            Database database = DatabaseFactory.CreateDatabase();
            int quantity = orderInfo.LineItems.Sum<KeyValuePair<string, LineItemInfo>>((Func<KeyValuePair<string, LineItemInfo>, int>)(item => item.Value.Quantity));
            lock (createOrderLocker)
            {
                if (orderInfo.GroupBuyId > 0)
                {
                    checkCanGroupBuy(quantity, orderInfo.GroupBuyId);
                }
                using (DbConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    DbTransaction dbTran = connection.BeginTransaction();

                    try
                    {
                        WriteLog(orderInfo == null ? "orderinfo空" : "不为空");
                        if (string.IsNullOrEmpty(orderInfo.OrderId))
                        {
                            orderInfo.OrderId = GenerateOrderIdByOrder(Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.OrderIdSortNumCount,dbTran);
                        }
                        
                        if (!new OrderDao().CreatOrder(orderInfo, dbTran))
                        {
                            dbTran.Rollback();
                            return false;
                        }
                        if ((orderInfo.LineItems.Count > 0) && !new LineItemDao().AddOrderLineItems(orderInfo.OrderId, orderInfo.LineItems.Values, dbTran))
                        {
                            dbTran.Rollback();
                            return false;
                        }
                        //如果有礼品,添加到礼品表
                        //add@20150921 by hj
                        if (orderInfo.Gifts.Count > 0)
                        {
                            OrderGiftDao dao = new OrderGiftDao();
                            foreach (OrderGiftInfo info in orderInfo.Gifts)
                            {
                                if (!dao.AddOrderGift(orderInfo.OrderId, info, 0, dbTran))
                                {
                                    dbTran.Rollback();
                                    return false;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(orderInfo.CouponCode) && !new CouponDao().AddCouponUseRecord(orderInfo, dbTran))
                        {
                            dbTran.Rollback();
                            return false;
                        }
                        //代金券功能目前不使用,redpagerId字段废除,用迪蔓的新规则:选择服务天使门店id代替
                        /*
                        if ((orderInfo.RedPagerID > 0) && !new UserRedPagerDao().AddUserRedPagerRecord(orderInfo, dbTran))
                        {
                            dbTran.Rollback();
                            return false;
                        }
                         */ 
                        if (orderInfo.GroupBuyId > 0)
                        {
                            GroupBuyDao dao = new GroupBuyDao();
                            GroupBuyInfo groupBuy = dao.GetGroupBuy(orderInfo.GroupBuyId,dbTran);
                            groupBuy.SoldCount += quantity;
                            dao.UpdateGroupBuy(groupBuy, dbTran);
                            dao.RefreshGroupBuyFinishState(orderInfo.GroupBuyId, dbTran);
                        }
                        dbTran.Commit();
                        flag = true;
                    }
                    catch(Exception ex)
                    {
                        WriteLog(ex.Message);
                        dbTran.Rollback();
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return flag;
        }
        public static void WriteLog(string log)
        {
            System.IO.StreamWriter writer = System.IO.File.AppendText(System.Web.HttpContext.Current.Server.MapPath("~/wx_.txt"));
            writer.WriteLine(System.DateTime.Now);
            writer.WriteLine(log);
            writer.Flush();
            writer.Close();
        }
        private static string GenerateOrderIdByOrder(int rightOrderLength = 0,DbTransaction dbTran = null)
        {
            //订单id由前八位日期,后七位随机数组成
            string str = string.Empty;
            System.Random random = new System.Random();
            for (int i = 0; i < 7 - rightOrderLength; i++)
            {
                int num = random.Next();
                str += ((char)(48 + (ushort)(num % 10))).ToString();
            }

            if (rightOrderLength > 0)
            {
                int userid = 0;
                ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
                if (currentManager != null)
                {
                    userid = currentManager.UserId;
                }

                string maxOrderIDNext = Hidistro.ControlPanel.Sales.OrderHelper.GetTodayOrderNum(dbTran, userid);
                if (string.IsNullOrEmpty(maxOrderIDNext))
                {
                    maxOrderIDNext = "1";
                }
                else
                {
                    maxOrderIDNext = maxOrderIDNext.Substring(maxOrderIDNext.Length - rightOrderLength, rightOrderLength);
                    maxOrderIDNext = (int.Parse(maxOrderIDNext) + 1).ToString();
                    if (maxOrderIDNext.Length > rightOrderLength)
                    {
                        maxOrderIDNext = "1";
                    }
                }
                string leftNumStr = maxOrderIDNext.PadLeft(rightOrderLength, '0');
                str += leftNumStr;
            }
            return System.DateTime.Now.ToString("yyyyMMdd") + str;
        }

        /// <summary>
        /// 计算代理商订单ID
        /// </summary>
        /// <param name="userId">后台用户Id</param>
        /// <returns>订单ID</returns>
        private static string GenerateOrderId(int userId)
        {
            string str = string.Empty;
            System.Random random = new System.Random();
            for (int i = 0; i < 7; i++)
            {
                int num = random.Next();
                str += ((char)(48 + (ushort)(num % 10))).ToString();
            }
            return System.DateTime.Now.ToString("yyyyMMdd") + str + "_" + userId.ToString();
        }

        public static DataTable GetCoupon(decimal orderAmount,string CouponIds)
        {
            return new CouponDao().GetCoupon(orderAmount, CouponIds);
        }

        public static CouponInfo GetCoupon(string couponCode)
        {
            return new CouponDao().GetCouponDetails(couponCode);
        }

        public static OrderInfo GetOrderInfo(string orderId)
        {
            return new OrderDao().GetOrderInfo(orderId);
        }


        public static DataTable GetOrderReturnTable(int userid, string ReturnsId)
        {
            return new RefundDao().GetOrderReturnTable(userid, ReturnsId);
        }

        public static PaymentModeInfo GetPaymentMode(int modeId)
        {
            return new PaymentModeDao().GetPaymentMode(modeId);
        }

        public static IList<PaymentModeInfo> GetPaymentModes()
        {
            return new PaymentModeDao().GetPaymentModes();
        }

        public static SKUItem GetProductAndSku(MemberInfo member, int productId, string options, bool isMemberPrice=true)
        {
            return new SkuDao().GetProductAndSku(member, productId, options, isMemberPrice);
        }

        public static bool GetReturnMes(int userid, string OrderId, int ProductId, int HandleStatus)
        {
            return new RefundDao().GetReturnMes(userid, OrderId, ProductId, HandleStatus);
        }

        public static ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
        {
            return new ShippingModeDao().GetShippingMode(modeId, includeDetail);
        }

        public static IList<ShippingModeInfo> GetShippingModes()
        {
            return new ShippingModeDao().GetShippingModes();
        }

        public static bool InsertCalculationCommission(string orderid)
        {
            OrderInfo orderInfo = GetOrderInfo(orderid);
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(orderInfo.ReferralUserId);
            bool flag = false;
            if (userIdDistributors != null)
            {
                Dictionary<string, LineItemInfo> lineItems = orderInfo.LineItems;
                LineItemInfo info3 = new LineItemInfo();
                DataView defaultView = CategoryBrowser.GetAllCategories().DefaultView;
                string str2 = null;
                string str3 = null;
                string str4 = null;
                decimal subTotal = 0M;
                foreach (KeyValuePair<string, LineItemInfo> pair in lineItems)
                {
                    string key = pair.Key;
                    info3 = pair.Value;
                    DataTable productCategories = ProductBrowser.GetProductCategories(info3.ProductId);
                    if ((productCategories.Rows.Count > 0) && (productCategories.Rows[0][0].ToString() != "0"))
                    {
                        defaultView.RowFilter = " CategoryId=" + productCategories.Rows[0][0];
                        str2 = defaultView[0]["FirstCommission"].ToString();
                        str3 = defaultView[0]["SecondCommission"].ToString();
                        str4 = defaultView[0]["ThirdCommission"].ToString();
                        if ((!string.IsNullOrEmpty(str2) && !string.IsNullOrEmpty(str3)) && !string.IsNullOrEmpty(str4))
                        {
                            ArrayList referralBlanceList = new ArrayList();
                            ArrayList userIdList = new ArrayList();
                            ArrayList ordersTotalList = new ArrayList();
                            subTotal = info3.GetSubTotal();
                            if (string.IsNullOrEmpty(userIdDistributors.ReferralPath))
                            {
                                referralBlanceList.Add((decimal.Parse(str4) / 100M) * info3.GetSubTotal());
                                userIdList.Add(orderInfo.ReferralUserId);
                                ordersTotalList.Add(subTotal);
                            }
                            else
                            {
                                string[] strArray = userIdDistributors.ReferralPath.Split(new char[] { '|' });
                                if (strArray.Length == 1)
                                {
                                    referralBlanceList.Add((decimal.Parse(str3) / 100M) * info3.GetSubTotal());
                                    userIdList.Add(strArray[0]);
                                    ordersTotalList.Add(subTotal);
                                    referralBlanceList.Add((decimal.Parse(str4) / 100M) * info3.GetSubTotal());
                                    userIdList.Add(orderInfo.ReferralUserId);
                                    ordersTotalList.Add(subTotal);
                                }
                                if (strArray.Length == 2)
                                {
                                    referralBlanceList.Add((decimal.Parse(str2) / 100M) * info3.GetSubTotal());
                                    userIdList.Add(strArray[0]);
                                    ordersTotalList.Add(subTotal);
                                    referralBlanceList.Add((decimal.Parse(str3) / 100M) * info3.GetSubTotal());
                                    userIdList.Add(strArray[1]);
                                    ordersTotalList.Add(subTotal);
                                    referralBlanceList.Add((decimal.Parse(str4) / 100M) * info3.GetSubTotal());
                                    userIdList.Add(orderInfo.ReferralUserId);
                                    ordersTotalList.Add(subTotal);
                                }
                            }
                            flag = InsertCalculationCommission(userIdList, referralBlanceList, orderInfo.OrderId, ordersTotalList, orderInfo.UserId.ToString());
                        }
                    }
                }
            }
            return flag;
        }

        public static bool InsertCalculationCommission(ArrayList UserIdList, ArrayList ReferralBlanceList, string orderid, ArrayList OrdersTotalList, string userid)
        {
            return new OrderDao().InsertCalculationCommission(UserIdList, ReferralBlanceList, orderid, OrdersTotalList, userid);
        }

        public static bool InsertOrderRefund(RefundInfo refundInfo)
        {
            return new RefundDao().InsertOrderRefund(refundInfo);
        }

        public static bool UpdateAdjustCommssions(string orderId, string skuId, decimal commssionmoney, decimal adjustcommssion)
        {
            bool flag = false;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    OrderInfo orderInfo = GetOrderInfo(orderId);
                    if (orderId == null)
                    {
                        return false;
                    }
                    int userId = DistributorsBrower.GetCurrentDistributors().UserId;
                    if ((orderInfo.ReferralUserId != userId) || (orderInfo.OrderStatus != OrderStatus.WaitBuyerPay))
                    {
                        return false;
                    }
                    LineItemInfo lineItem = orderInfo.LineItems[skuId];
                    if ((lineItem == null) || (lineItem.ItemsCommission < adjustcommssion))
                    {
                        return false;
                    }
                    lineItem.ItemAdjustedCommssion = adjustcommssion;
                    if (!new LineItemDao().UpdateLineItem(orderId, lineItem, dbTran))
                    {
                        dbTran.Rollback();
                    }
                    if (!new OrderDao().UpdateOrder(orderInfo, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    dbTran.Commit();
                    flag = true;
                }
                catch (Exception)
                {
                    dbTran.Rollback();
                }
                finally
                {
                    connection.Close();
                }
                return flag;
            }
        }

        public static bool isActing()
        {
            return new OrderDao().isActing();
        }

        public static bool UpdateOrderGoodStatu(string orderid, string skuid, int OrderItemsStatus)
        {
            return new RefundDao().UpdateOrderGoodStatu(orderid, skuid, OrderItemsStatus);
        }

        public static CouponInfo UseCoupon(decimal orderAmount, string claimCode)
        {
            if (!string.IsNullOrEmpty(claimCode))
            {
                CouponInfo coupon = GetCoupon(claimCode);
                if (coupon != null)
                {
                    decimal? amount;
                    if (coupon.Amount.HasValue)
                    {
                        amount = coupon.Amount;
                        if (!((amount.GetValueOrDefault() > 0M) && amount.HasValue) || (orderAmount < coupon.Amount.Value))
                        {
                        }
                    }
                    if ((coupon.Amount.HasValue && (!(((amount = coupon.Amount).GetValueOrDefault() == 0M) && amount.HasValue) || (orderAmount <= coupon.DiscountValue))))
                    {
                        return coupon;
                    }
                    if (coupon.Amount == 0M && orderAmount >= coupon.DiscountValue)
                    {
                        return coupon;
                    }
                }
            }
            return null;
        }

        public static int CountDownOrderCount(int productid)
        {
            return new LineItemDao().CountDownOrderCount(productid);
        }

        public static bool IsOrderRemind(int UserID, int Time)
        {
            return new OrderDao().IsOrderRemind(UserID,Time);
        }


    }
}

