namespace Hidistro.SaleSystem.Member
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    //using Hidistro.SaleSystem.Shopping;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.SqlDal;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Members;
    using Hidistro.SqlDal.Orders;
    using Hidistro.SqlDal.Promotions;
    using Hidistro.SqlDal.Sales;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    //using System.Transactions;

    public static class TradeHelper
    {
        public static int AddClaimCodeToUser(string claimCode, int userId)
        {
            return 0;// new CouponDao().AddClaimCodeToUser(claimCode, userId, MemberProcessor.GetCurrentMember().UserName);
        }

        //public static bool AgreedReplace(int ReplaceId, string AdminRemark, string OrderId, string skuId, string AgreedReplace)
        //{
        //    if (new ReplaceDao().AgreedReplace(ReplaceId, AdminRemark, OrderId, skuId, AgreedReplace))
        //    {
        //        UpdateOrderItemStatus(OrderId);
        //        return true;
        //    }
        //    return false;
        //}

        //public static bool ApplyForRefund(string orderId, string remark, int refundType, string skuId, int quantity, string GateWay, string GateWayOrderId, string ShopName, decimal RefundMoney, int StoreId, string refundReason, string UserCredentials)
        //{
        //    if (new RefundDao().ApplyForRefund(orderId, remark, refundType, skuId, quantity, GateWay, GateWayOrderId, ShopName, RefundMoney, StoreId, refundReason, UserCredentials))
        //    {
        //        UpdateOrderItemStatus(orderId);
        //        return true;
        //    }
        //    return false;
        //}

        //public static bool ApplyForReplace(string orderId, string remark, string skuId, int quantity, string shopName, int StoreId, string ReplaceReason, string UserCredentials)
        //{
        //    if (new ReplaceDao().ApplyForReplace(orderId, remark, skuId, quantity, shopName, StoreId, ReplaceReason, UserCredentials))
        //    {
        //        UpdateOrderItemStatus(orderId);
        //        return true;
        //    }
        //    return false;
        //}

        //public static bool ApplyForReturn(string orderId, string remark, int refundType, string SkuId, int Quantity, string GateWay, string GateWayOrderId, string ShopName, decimal RefundMoney, int StoreId, string ReturnReason, string UserCredentials)
        //{
        //    if (new ReturnDao().ApplyForReturn(orderId, remark, refundType, SkuId, Quantity, GateWay, GateWayOrderId, ShopName, RefundMoney, StoreId, ReturnReason, UserCredentials))
        //    {
        //        UpdateOrderItemStatus(orderId);
        //        return true;
        //    }
        //    return false;
        //}

        public static bool CanRefund(OrderInfo order, string skuId = "")
        {
            if ((order != null) && (order.OrderStatus == OrderStatus.BuyerAlreadyPaid))
            {
                if (string.IsNullOrEmpty(skuId))
                {
                    if (order.ItemStatus != OrderItemStatus.Nomarl)
                    {
                        return false;
                    }
                    return true;
                }
                if (order.LineItems.ContainsKey(skuId))
                {
                    LineItemInfo info = order.LineItems[skuId];
                    return ((((info.Status == LineItemStatus.Normal) || (info.Status == LineItemStatus.RefundRefused)) || ((info.Status == LineItemStatus.Replaced) || (info.Status == LineItemStatus.ReplaceRefused))) || (info.Status == LineItemStatus.ReturnsRefused));
                }
            }
            return false;
        }

        public static bool CanRefund(string orderId, string skuId = "")
        {
            return CanRefund(new OrderDao().GetOrderInfo(orderId), skuId);
        }

        public static bool CanReplace(OrderInfo order, string SkuId = "")
        {
            if ((order != null) && (((order.OrderStatus == OrderStatus.Finished) || (order.OrderStatus == OrderStatus.SellerAlreadySent)) || (order.OrderStatus == OrderStatus.Replaced)))
            {
                if (string.IsNullOrEmpty(SkuId))
                {
                    if (order.ItemStatus != OrderItemStatus.Nomarl)
                    {
                        return false;
                    }
                    return true;
                }
                if (order.LineItems.ContainsKey(SkuId))
                {
                    LineItemInfo info = order.LineItems[SkuId];
                    return ((((info.Status == LineItemStatus.Normal) || (info.Status == LineItemStatus.RefundRefused)) || ((info.Status == LineItemStatus.Replaced) || (info.Status == LineItemStatus.ReplaceRefused))) || (info.Status == LineItemStatus.ReturnsRefused));
                }
            }
            return false;
        }

        public static bool CanReplace(string orderId, string SkuId = "")
        {
            return CanReplace(GetOrderInfo(orderId), SkuId);
        }

        public static bool CanReturn(OrderInfo order, string SkuId = "")
        {
            if ((order != null) && (((order.OrderStatus == OrderStatus.SellerAlreadySent) || (order.OrderStatus == OrderStatus.Finished)) || (order.OrderStatus == OrderStatus.Replaced)))
            {
                if (string.IsNullOrEmpty(SkuId))
                {
                    if (order.ItemStatus != OrderItemStatus.Nomarl)
                    {
                        return false;
                    }
                    return true;
                }
                if (order.LineItems.ContainsKey(SkuId))
                {
                    LineItemInfo info = order.LineItems[SkuId];
                    return ((((info.Status == LineItemStatus.Normal) || (info.Status == LineItemStatus.RefundRefused)) || ((info.Status == LineItemStatus.ReplaceRefused) || (info.Status == LineItemStatus.Replaced))) || (info.Status == LineItemStatus.ReturnsRefused));
                }
            }
            return false;
        }

        public static bool CanReturn(string orderId, string SkuId = "")
        {
            return CanReturn(new OrderDao().GetOrderInfo(orderId), SkuId);
        }

        public static bool CheckOrderStock(OrderInfo order, out string productinfo)
        {
            //bool flag = true;
            productinfo = "";
            //if (order.GroupBuyId > 0)
            //{
            //    GroupBuyInfo groupBuy = GetGroupBuy(order.GroupBuyId);
            //    if (((groupBuy == null) || (groupBuy.StartDate > DateTime.Now)) || (groupBuy.EndDate < DateTime.Now))
            //    {
            //        productinfo = "团购已结束或者团购数量已达到限制数量";
            //        return false;
            //    }
            //    if (groupBuy.SoldCount > groupBuy.MaxCount)
            //    {
            //        productinfo = "团购已结束或者团购数量已达到限制数量";
            //        return false;
            //    }
            //    return true;
            //}
            //if (order.CountDownBuyId > 0)
            //{
            //    CountDownInfo countDownBuy = GetCountDownBuy(order.CountDownBuyId);
            //    if (((countDownBuy == null) || (countDownBuy.StartDate > DateTime.Now)) || (countDownBuy.EndDate < DateTime.Now))
            //    {
            //        productinfo = "抢购已结束或者团购数量已达到限制数量";
            //        return false;
            //    }
            //    if (ShoppingProcessor.CountDownOrderCount(countDownBuy.ProductId) > countDownBuy.MaxCount)
            //    {
            //        productinfo = "抢购已结束或者团购数量已达到限制数量";
            //        return false;
            //    }
            //    return true;
            //}
            //if (order.BundlingID > 0)
            //{
            //    if (GetBundling(order.BundlingID) == null)
            //    {
            //        productinfo = "捆绑销售信息不存在";
            //        return false;
            //    }
            //}
            //else
            //{
            //    return true;
            //}
            //foreach (LineItemInfo info4 in order.LineItems.Values)
            //{
            //    if (ShoppingCartProcessor.GetSkuStock(info4.SkuId) < info4.Quantity)
            //    {
            //        productinfo = productinfo + ((productinfo == "") ? "" : "、") + info4.SKUContent;
            //        flag = false;
            //    }
            //}
            return false;//flag;
        }

        public static bool CheckShoppingStock(ShoppingCartInfo shoppingcart, out string productinfo)
        {
            bool flag = true;
            productinfo = "";
            foreach (ShoppingCartItemInfo info in shoppingcart.LineItems)
            {
                if (ShoppingCartProcessor.GetSkuStock(info.SkuId) < info.Quantity)
                {
                    string str = productinfo;
                    productinfo = str + ((productinfo == "") ? "" : "、") + info.Name + " " + info.SkuContent;
                    flag = false;
                }
            }
            return flag;
        }

        public static bool CloseOrder(string orderId)
        {
            OrderDao dao = new OrderDao();
            OrderInfo orderInfo = dao.GetOrderInfo(orderId);
            if (orderInfo.CheckAction(OrderActions.SELLER_CLOSE) && (orderInfo.ItemStatus == OrderItemStatus.Nomarl))
            {
                orderInfo.OrderStatus = OrderStatus.Closed;
                return dao.UpdateOrder(orderInfo, null);
            }
            return false;
        }

        public static bool ConfirmOrderFinish(OrderInfo order)
        {
            bool flag = false;
            if (order.CheckAction(OrderActions.BUYER_CONFIRM_GOODS) && (order.ItemStatus == OrderItemStatus.Nomarl))
            {
                order.OrderStatus = OrderStatus.Finished;
                order.FinishDate = DateTime.Now;
                flag = new OrderDao().UpdateOrder(order, null);
            }
            return flag;
        }

        public static bool ExitCouponClaimCode(string claimCode)
        {
            return true;//new CouponDao().ExitCouponClaimCode(claimCode);
        }

        //public static bool FinishGetGoodsForReturn(int ReturnsId, string AdminRemark, string OrderId, string skuId = "")
        //{
        //    if (new ReturnDao().FinishGetGoods(ReturnsId, AdminRemark, OrderId, skuId))
        //    {
        //        UpdateOrderItemStatus(OrderId);
        //        return true;
        //    }
        //    return false;
        //}

        //public static bool FinishReplace(int ReplaceId, string AdminRemark, string OrderId, string SkuId = "")
        //{
        //    if (new ReplaceDao().FinishReplace(ReplaceId, AdminRemark, OrderId, SkuId))
        //    {
        //        UpdateOrderItemStatus(OrderId);
        //        return true;
        //    }
        //    return false;
        //}

        //public static bool FinishReturn(int ReturnsId, string AdminRemark, string OrderId, string skuId = "")
        //{
        //    if (new ReturnDao().FinishReturn(ReturnsId, AdminRemark, OrderId, skuId))
        //    {
        //        UpdateOrderItemStatus(OrderId);
        //        return true;
        //    }
        //    return false;
        //}

        //public static BundlingInfo GetBundling(int BundID)
        //{
        //    return new BundlingDao().GetBundlingInfo(BundID);
        //}

        //public static BundlingInfo GetBundling(int BundID, int buyAmount, out string msg)
        //{
        //    msg = "";
        //    BundlingInfo bundlingInfo = new BundlingDao().GetBundlingInfo(BundID);
        //    if (bundlingInfo == null)
        //    {
        //        msg = "捆绑销售不存在!";
        //    }
        //    return bundlingInfo;
        //}

        //public static DataTable GetChangeCoupons()
        //{
        //    return new CouponDao().GetChangeCoupons();
        //}

        //public static CountDownInfo GetCountDownBuy(int CountDownId)
        //{
        //    return new CountDownDao().GetCountDownInfo(CountDownId);
        //}

        //public static CountDownInfo GetCountDownInfo(int countDownID, int buyAmount, out string msg)
        //{
        //    msg = "";
        //    CountDownInfo countDownInfo = new CountDownDao().GetCountDownInfo(countDownID);
        //    if (countDownInfo == null)
        //    {
        //        msg = "抢购信息不存在！";
        //        return null;
        //    }
        //    if (((countDownInfo.MaxCount < buyAmount) || (countDownInfo.StartDate > DateTime.Now)) || (countDownInfo.EndDate < DateTime.Now))
        //    {
        //        msg = "抢购还没有开始或者已经结束,或者数量超过了抢购数量";
        //        return null;
        //    }
        //    return countDownInfo;
        //}

        public static GroupBuyInfo GetGroupBuy(int groupBuyId)
        {
            return new GroupBuyDao().GetGroupBuy(groupBuyId);
        }

        public static GroupBuyInfo GetGroupBuyInfo(int groupbuyID, int buyAmount, out string msg)
        {
            msg = "";
            GroupBuyInfo groupBuy = new GroupBuyDao().GetGroupBuy(groupbuyID);
            if (groupBuy == null)
            {
                msg = "团购信息不存在！";
                return null;
            }
            if ((((groupBuy.MaxCount < buyAmount) || (groupBuy.StartDate > DateTime.Now)) || (groupBuy.EndDate < DateTime.Now)) || (groupBuy.Status != GroupBuyStatus.UnderWay))
            {
                msg = "团购还没有开始或者已经结束,或者数量超过了限制数量";
                return null;
            }
            return groupBuy;
        }

        public static int GetMaxQuantity(OrderInfo order, string skuId)
        {
            if (string.IsNullOrEmpty(skuId))
            {
                return 1;//order.GetAllQuantity();(%)
            }
            if (order.LineItems.ContainsKey(skuId))
            {
                return order.LineItems[skuId].ShipmentQuantity;
            }
            return 0;
        }

        public static decimal GetMaxRefundAmount(OrderInfo order, string skuId)
        {
            if (string.IsNullOrEmpty(skuId))
            {
                return 1;//order.GetPayTotal();(%)
            }
            if (order.LineItems.ContainsKey(skuId))
            {
                return 2;//order.GetCanRefundAmount(skuId);(%)
            }
            return 0M;
        }

        public static int GetOrderCount(int groupBuyId)
        {
            return new GroupBuyDao().GetOrderCount(groupBuyId);
        }

        public static DataTable GetOrderGiftsThumbnailsUrl(string orderId)
        {
            return new DataTable();//OrderDao().GetOrderGiftsThumbnailsUrl(orderId);(%)
        }

        public static OrderInfo GetOrderInfo(string orderId)
        {
            return new OrderDao().GetOrderInfo(orderId);
        }

        public static DataTable GetOrderItemThumbnailsUrl(string orderId)
        {
            return new DataTable();//OrderDao().GetOrderItemThumbnailsUrl(orderId);(%)
        }

        private static decimal GetOrderReferralDeduct(OrderInfo order)
        {
            decimal num = 0M;
            ProductDao dao = new ProductDao();
            foreach (LineItemInfo info in order.LineItems.Values)
            {
                num += 1;//(dao.GetProductReferralDeduct(info.ProductId) * info.GetSubTotal()) / 100M;(%)
            }
            return num;
        }

        private static decimal GetOrderSubMemberDeduct(OrderInfo order)
        {
            decimal num = 0M;
            ProductDao dao = new ProductDao();
            foreach (LineItemInfo info in order.LineItems.Values)
            {
                num += 1; //(dao.GetProductSubMemberDeduct(info.ProductId) * info.GetSubTotal()) / 100M;(%)
            }
            return num;
        }

        private static decimal GetOrderSubReferralDeduct(OrderInfo order)
        {
            decimal num = 0M;
            ProductDao dao = new ProductDao();
            foreach (LineItemInfo info in order.LineItems.Values)
            {
                num += 1;//(dao.GetProductSubReferralDeduct(info.ProductId) * info.GetSubTotal()) / 100M;(%)
            }
            return num;
        }

        public static PaymentModeInfo GetPaymentMode(int modeId)
        {
            return new PaymentModeDao().GetPaymentMode(modeId);
        }

        public static PaymentModeInfo GetPaymentMode(string gateway)
        {
            return new PaymentModeDao().GetPaymentMode(gateway);
        }

        public static IList<PaymentModeInfo> GetPaymentModes(PayApplicationType payApplicationType)
        {
            return new List<PaymentModeInfo>(); //PaymentModeDao().GetPaymentModes(payApplicationType);
        }

        //public static CountDownInfo GetProductCountDownInfo(int ProductID, int buyAmount, out string msg)
        //{
        //    msg = "";
        //    CountDownInfo countDownByProductId = new CountDownDao().GetCountDownByProductId(ProductID);
        //    if (countDownByProductId == null)
        //    {
        //        msg = "抢购信息不存在！";
        //        return null;
        //    }
        //    if (((countDownByProductId.MaxCount < buyAmount) || (countDownByProductId.StartDate > DateTime.Now)) || (countDownByProductId.EndDate < DateTime.Now))
        //    {
        //        msg = "抢购还没有开始或者已经结束,或者数量超过了抢购数量";
        //        return null;
        //    }
        //    return countDownByProductId;
        //}

        //public static GroupBuyInfo GetProductGroupBuyInfo(int ProductID, int buyAmount, out string msg)
        //{
        //    msg = "";
        //    GroupBuyInfo productGroupBuyInfo = new GroupBuyDao().GetProductGroupBuyInfo(ProductID);
        //    if (productGroupBuyInfo == null)
        //    {
        //        productGroupBuyInfo = new GroupBuyDao().GetGroupBuy(ProductID);
        //    }
        //    if (productGroupBuyInfo == null)
        //    {
        //        msg = "团购信息不存在！";
        //        return null;
        //    }
        //    if ((((productGroupBuyInfo.MaxCount < buyAmount) || (productGroupBuyInfo.StartDate > DateTime.Now)) || (productGroupBuyInfo.EndDate < DateTime.Now)) || (productGroupBuyInfo.Status != GroupBuyStatus.UnderWay))
        //    {
        //        msg = "团购还没有开始或者已经结束,或者数量超过了限制数量";
        //        return null;
        //    }
        //    return productGroupBuyInfo;
        //}

        //public static DbQueryResult GetRefundApplys(RefundApplyQuery query)
        //{
        //    return new RefundDao().GetRefundApplys(query);
        //}

        //public static DataTable GetRefundApplysTable(int refundId)
        //{
        //    return new RefundDao().GetRefundApplysTable(refundId);
        //}

        //public static RefundInfo GetRefundInfo(int refundId)
        //{
        //    return new RefundDao().GetRefundInfo(refundId);
        //}

        //public static RefundInfo GetRefundInfo(string RefundOrderId)
        //{
        //    return new RefundDao().GetRefundInfo(RefundOrderId, "");
        //}

        //public static RefundInfo GetRefundInfo(string OrderId, string SkuId = "")
        //{
        //    return new RefundDao().GetRefundInfo(OrderId, SkuId);
        //}

        //public static decimal GetRefundMoney(OrderInfo order, out decimal refundMoney)
        //{
        //    return new ReturnDao().GetRefundMoney(order, out refundMoney, "");
        //}

        //public static DbQueryResult GetReplaceApplys(ReplaceApplyQuery query)
        //{
        //    return new ReplaceDao().GetReplaceApplys(query);
        //}

        //public static DataTable GetReplaceApplysTable(int replaceId)
        //{
        //    return new ReplaceDao().GetReplaceApplysTable(replaceId, 0);
        //}

        //public static ReplaceInfo GetReplaceInfo(int replaceId)
        //{
        //    return new ReplaceDao().GetReplaceInfo(replaceId);
        //}

        //public static ReplaceInfo GetReplaceInfo(string OrderId, string SkuId = "")
        //{
        //    return new ReplaceDao().GetReplaceInfo(OrderId, SkuId);
        //}

        //public static DbQueryResult GetReturnsApplys(ReturnsApplyQuery query)
        //{
        //    return new ReturnDao().GetReturnsApplys(query);
        //}

        //public static DataTable GetReturnsApplysTable(int returnsId)
        //{
        //    return new ReturnDao().GetReturnsApplysTable(returnsId);
        //}

        //public static ReturnsInfo GetReturnsInfo(int returnId)
        //{
        //    return new ReturnDao().GetReturnInfo(returnId);
        //}

        //public static ReturnsInfo GetReturnsInfo(string OrderId, string SkuId = "")
        //{
        //    return new ReturnDao().GetReturnInfo(OrderId, SkuId);
        //}

        //public static DataSet GetUserCoupons(UserCouponQuery query)
        //{
        //    return new CouponDao().GetUserCoupons(query);
        //}

        public static DataTable GetUserCoupons(int userId, int useType = 0)
        {
            return new CouponDao().GetUserCoupons(userId, useType);
        }

        //public static DbQueryResult GetUserOrder(int userId, OrderQuery query)
        //{
        //    return new OrderDao().GetMyUserOrder(userId, query);
        //}

        //public static DbQueryResult GetUserPoints(int pageIndex)
        //{
        //    return new PointDetailDao().GetUserPoints(pageIndex);
        //}

        public static bool IsOnlyOneSku(OrderInfo order)
        {
            if (order == null)
            {
                return false;
            }
            return (order.LineItems.Count <= 1);
        }

        public static bool OrderHasRefunding(OrderInfo order)
        {
            if (((order == null) || (order.LineItems.Count == 0)) || (order.OrderStatus == OrderStatus.ApplyForRefund))
            {
                return true;
            }
            foreach (LineItemInfo info in order.LineItems.Values)
            {
                if (info.Status == LineItemStatus.RefundApplied)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool OrderHasRefundOrReturning(OrderInfo order)
        {
            return (OrderHasRefunding(order) || OrderHasReturning(order));
        }

        public static bool OrderHasReturning(OrderInfo order)
        {
            if (((((order == null) || (order.LineItems.Count == 0)) || ((order.OrderStatus == OrderStatus.ApplyForReturns) || (order.OrderStatus == OrderStatus.MerchantsAgreedFoReturns))) || (order.OrderStatus == OrderStatus.DeliveryingForReturns)) || (order.OrderStatus == OrderStatus.GetGoodsForReturns))
            {
                return true;
            }
            foreach (LineItemInfo info in order.LineItems.Values)
            {
                if ((((info.Status == LineItemStatus.ReturnApplied) || (info.Status == LineItemStatus.MerchantsAgreedForReturn)) || (info.Status == LineItemStatus.DeliveryForReturn)) || (info.Status == LineItemStatus.GetGoodsForReturn))
                {
                    return true;
                }
            }
            return false;
        }

        //public static bool PointChageCoupon(int couponId, int needPoint, int currentPoint)
        //{
        //    Member user = HiContext.Current.User as Member;
        //    PointDetailInfo point = new PointDetailInfo {
        //        OrderId = string.Empty,
        //        UserId = user.UserId,
        //        TradeDate = DateTime.Now,
        //        TradeType = PointTradeType.ChangeCoupon,
        //        Increased = null,
        //        Reduced = new int?(needPoint),
        //        Points = currentPoint - needPoint
        //    };
        //    if ((point.Points >= 0) && new PointDetailDao().AddPointDetail(point))
        //    {
        //        CouponItemInfo couponItem = new CouponItemInfo {
        //            CouponId = couponId,
        //            UserId = new int?(user.UserId),
        //            UserName = user.Username,
        //            EmailAddress = user.Email,
        //            ClaimCode = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15),
        //            GenerateTime = DateTime.Now
        //        };
        //        Users.ClearUserCache(user);
        //        if (new CouponDao().SendClaimCodes(couponItem))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        public static int RemoveOrder(string orderIds)
        {
            return new OrderDao().DeleteOrders(orderIds);
        }

        //public static bool ReplaceShopSendGoods(int ReplaceId, string ExpressCompanyAbb, string ExpressCompanyName, string ShipOrderNumber, string OrderId, string skuId = "")
        //{
        //    if (new ReplaceDao().ShopSendGoods(ReplaceId, ExpressCompanyAbb, ExpressCompanyName, ShipOrderNumber, OrderId, skuId))
        //    {
        //        UpdateOrderItemStatus(OrderId);
        //        return true;
        //    }
        //    return false;
        //}

        //public static bool ReplaceUserSendGoods(int ReturnsId, string ExpressCompanyAbb, string ExpressCompanyName, string ShipOrderNumber, string OrderId, string skuId = "")
        //{
        //    if (new ReplaceDao().UserSendGoods(ReturnsId, ExpressCompanyAbb, ExpressCompanyName, ShipOrderNumber, OrderId, skuId))
        //    {
        //        UpdateOrderItemStatus(OrderId);
        //        return true;
        //    }
        //    return false;
        //}

        public static bool SaveDebitNote(DebitNoteInfo note)
        {
            return new DebitNoteDao().SaveDebitNote(note);
        }

        public static bool SetGroupBuyEndUntreated(int groupBuyId)
        {
            return new GroupBuyDao().SetGroupBuyEndUntreated(groupBuyId);
        }

        public static bool UpdateOrderItemStatus(string OrderId)
        {
            //OrderInfo orderInfo = GetOrderInfo(OrderId);
            //if (orderInfo == null)
            //{
            //    return false;
            //}
            //Dictionary<string, LineItemInfo> lineItems = orderInfo.LineItems;
            //OrderItemStatus nomarl = OrderItemStatus.Nomarl;
            //if (lineItems.Count > 0)
            //{
            //    if ((orderInfo.RefunedCount + orderInfo.ReturnedCount) == lineItems.Count)
            //    {
            //        nomarl = OrderItemStatus.AllRefunedOrReturned;
            //    }
            //    if (orderInfo.ItemRefundCount == lineItems.Count)
            //    {
            //        nomarl = OrderItemStatus.HasRefund;
            //    }
            //    if (orderInfo.ItemReturnsCount == lineItems.Count)
            //    {
            //        nomarl = OrderItemStatus.HasReturn;
            //    }
            //    if (orderInfo.ItemReplaceCount == lineItems.Count)
            //    {
            //        nomarl = OrderItemStatus.HasReplace;
            //    }
            //    if ((((orderInfo.ItemRefundCount + orderInfo.ItemReplaceCount) + orderInfo.ItemReturnsCount) >= lineItems.Count) && (lineItems.Count > 1))
            //    {
            //        nomarl = OrderItemStatus.AllInAfterSales;
            //    }
            //    if ((((orderInfo.ItemRefundCount >= 1) && ((orderInfo.ItemReplaceCount + orderInfo.ItemReturnsCount) > 0)) || ((orderInfo.ItemReplaceCount >= 1) && ((orderInfo.ItemRefundCount + orderInfo.ItemReturnsCount) > 0))) || ((orderInfo.ItemReturnsCount >= 1) && ((orderInfo.ItemRefundCount + orderInfo.ItemReplaceCount) > 0)))
            //    {
            //        nomarl = OrderItemStatus.HasRefundOrReturnOrReplace;
            //    }
            //    if (orderInfo.ItemRefundCount >= 1)
            //    {
            //        nomarl = OrderItemStatus.HasRefund;
            //    }
            //    if (orderInfo.ItemReplaceCount >= 1)
            //    {
            //        nomarl = OrderItemStatus.HasReplace;
            //    }
            //    if (orderInfo.ItemReturnsCount >= 1)
            //    {
            //        nomarl = OrderItemStatus.HasReturn;
            //    }
            //}
            //return new OrderDao().UpdateOrderItemStatus(orderInfo.OrderId, nomarl);
            return false;
        }

        public static bool UpdateOrderPaymentType(OrderInfo order)
        {
            return (order.CheckAction(OrderActions.SELLER_MODIFY_TRADE) && new OrderDao().UpdateOrder(order, null));
        }

        //private static void UpdateUserAccount(OrderInfo order)
        //{
        //    SplittinDetailInfo info2;
        //    Member member3;
        //    decimal orderSubReferralDeduct;
        //    int userId = order.UserId;
        //    if (userId == 0x44c)
        //    {
        //        userId = 0;
        //    }
        //    IUser user = Users.GetUser(userId, false);
        //    Member member = user as Member;
        //    if (member != null)
        //    {
        //        PointDetailInfo point = new PointDetailInfo {
        //            OrderId = order.OrderId,
        //            UserId = member.UserId,
        //            TradeDate = DateTime.Now,
        //            TradeType = PointTradeType.Bounty,
        //            Increased = new int?(order.Points),
        //            Points = order.Points + member.Points
        //        };
        //        if ((point.Points > 0x7fffffff) || (point.Points < 0))
        //        {
        //            point.Points = 0x7fffffff;
        //        }
        //        PointDetailDao dao = new PointDetailDao();
        //        dao.AddPointDetail(point);
        //        MemberDao dao2 = new MemberDao();
        //        dao2.UpdateMemberAccount(order.GetTotal(), member.UserId);
        //        int historyPoint = dao.GetHistoryPoint(member.UserId);
        //        dao2.ChangeMemberGrade(member.UserId, member.GradeId, historyPoint);
        //    }
        //    ReferralDao dao3 = new ReferralDao();
        //    if (order.ReferralUserId > 0)
        //    {
        //        Member member2 = Users.GetUser(order.ReferralUserId) as Member;
        //        if ((member2 != null) && (member2.ReferralStatus == 2))
        //        {
        //            decimal orderReferralDeduct = GetOrderReferralDeduct(order);
        //            if (orderReferralDeduct > 0M)
        //            {
        //                info2 = new SplittinDetailInfo {
        //                    OrderId = order.OrderId,
        //                    UserId = member2.UserId,
        //                    UserName = member2.Username,
        //                    SubUserId = order.UserId,
        //                    IsUse = false,
        //                    TradeDate = DateTime.Now,
        //                    TradeType = SplittingTypes.ReferralDeduct,
        //                    Income = new decimal?(orderReferralDeduct),
        //                    Balance = dao3.GetUserUseSplittin(member2.UserId),
        //                    Remark = "购买者是：" + order.Username + " 订单金额：" + order.GetTotal().ToString("F2")
        //                };
        //                dao3.AddSplittinDetail(info2);
        //            }
        //            if (member2.ReferralUserId.HasValue)
        //            {
        //                member3 = Users.GetUser(member2.ReferralUserId.Value) as Member;
        //                if ((member3 != null) && (member3.ReferralStatus == 2))
        //                {
        //                    orderSubReferralDeduct = GetOrderSubReferralDeduct(order);
        //                    info2 = new SplittinDetailInfo {
        //                        OrderId = order.OrderId,
        //                        UserId = member3.UserId,
        //                        UserName = member3.Username,
        //                        SubUserId = member2.UserId,
        //                        IsUse = false,
        //                        TradeDate = DateTime.Now,
        //                        TradeType = SplittingTypes.SubReferralDeduct,
        //                        Income = new decimal?(orderSubReferralDeduct),
        //                        Balance = dao3.GetUserUseSplittin(member3.UserId),
        //                        Remark = "下级推广员是：" + member2.Username + " 订单金额：" + order.GetTotal().ToString("F2")
        //                    };
        //                    dao3.AddSplittinDetail(info2);
        //                }
        //            }
        //        }
        //    }
        //    else if ((member != null) && member.ReferralUserId.HasValue)
        //    {
        //        member3 = Users.GetUser(member.ReferralUserId.Value) as Member;
        //        if ((member3 != null) && (member3.ReferralStatus == 2))
        //        {
        //            orderSubReferralDeduct = GetOrderSubMemberDeduct(order);
        //            info2 = new SplittinDetailInfo {
        //                OrderId = order.OrderId,
        //                UserId = member3.UserId,
        //                UserName = member3.Username,
        //                SubUserId = order.UserId,
        //                IsUse = false,
        //                TradeDate = DateTime.Now,
        //                TradeType = SplittingTypes.SubMemberDeduct,
        //                Income = new decimal?(orderSubReferralDeduct),
        //                Balance = dao3.GetUserUseSplittin(member3.UserId),
        //                Remark = "下级会员是：" + member.Username + " 订单金额：" + order.GetTotal().ToString("F2")
        //            };
        //            dao3.AddSplittinDetail(info2);
        //        }
        //    }
        //    Users.ClearUserCache(user);
        //}

        //public static bool UserPayOrder(OrderInfo order, bool isBalancePayOrder)
        //{
        //    bool flag = false;
        //    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        OrderDao dao = new OrderDao();
        //        order.OrderStatus = OrderStatus.BuyerAlreadyPaid;
        //        order.PayDate = DateTime.Now;
        //        flag = dao.UpdateOrder(order, null);
        //        if (flag)
        //        {
        //            Member user = Users.GetUser(order.UserId, false) as Member;
        //            BalanceDetailDao dao2 = new BalanceDetailDao();
        //            if (isBalancePayOrder && (user != null))
        //            {
        //                decimal num = user.Balance - order.GetTotal();
        //                BalanceDetailInfo balanceDetails = new BalanceDetailInfo {
        //                    UserId = order.UserId,
        //                    UserName = order.Username,
        //                    TradeDate = DateTime.Now,
        //                    TradeType = TradeTypes.Consume,
        //                    Expenses = new decimal?(order.GetTotal()),
        //                    Balance = num,
        //                    Remark = string.Format("对订单{0}付款", order.OrderId)
        //                };
        //                dao2.InsertBalanceDetail(balanceDetails);
        //            }
        //            if (((!masterSettings.OpenMultStore || (order.GroupBuyId > 0)) || (order.CountDownBuyId > 0)) || (order.BundlingID > 0))
        //            {
        //                if (((order.GroupBuyId > 0) || (order.CountDownBuyId > 0)) || (order.BundlingID > 0))
        //                {
        //                    dao.UpdatePayOrderStock(order.OrderId, 0);
        //                }
        //                else
        //                {
        //                    dao.UpdatePayOrderStock(order.OrderId, order.StoreId);
        //                }
        //            }
        //            ProductDao dao3 = new ProductDao();
        //            foreach (LineItemInfo info2 in order.LineItems.Values)
        //            {
        //                ProductInfo productDetails = dao3.GetProductDetails(info2.ProductId);
        //                productDetails.SaleCounts += info2.Quantity;
        //                productDetails.ShowSaleCounts += info2.Quantity;
        //                dao3.UpdateProduct(productDetails, null);
        //            }
        //            UpdateUserAccount(order);
        //        }
        //        scope.Complete();
        //    }
        //    return flag;
        //}

        //public static bool UserSendGoodsForReturn(int ReturnsId, string ExpressCompanyAbb, string ExpressCompanyName, string ShipOrderNumber, string OrderId, string skuId)
        //{
        //    if (new ReturnDao().UserSendGoods(ReturnsId, ExpressCompanyAbb, ExpressCompanyName, ShipOrderNumber, OrderId, skuId))
        //    {
        //        UpdateOrderItemStatus(OrderId);
        //        return true;
        //    }
        //    return false;
        //}
    }
}

