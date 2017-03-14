namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Sales;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public static class ShoppingCartProcessor
    {
        /// <summary>
        /// add by hj 20150918 增加礼品
        /// </summary>
        /// <param name="giftId"></param>
        /// <param name="quantity"></param>
        /// <param name="promotype"></param>
        /// <returns></returns>
        public static bool AddGiftItem(int giftId, int quantity)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (quantity <= 0)
            {
                quantity = 1;
            }
            return new ShoppingCartDao().AddGiftItem(currentMember,giftId, quantity);
        }

        public static void AddLineItem(string skuId, int quantity, int categoryid)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (quantity <= 0)
            {
                quantity = 1;
            }
            new ShoppingCartDao().AddLineItem(currentMember, skuId, quantity, categoryid);
        }
        public static void AddLineItemPC(string skuId, int quantity, int categoryid,int pcUserid) {
            new ShoppingCartDao().AddLineItemPC(pcUserid, skuId, quantity, categoryid);
        }

        public static void ClearShoppingCart()
        {
            new ShoppingCartDao().ClearShoppingCart(Globals.GetCurrentMemberUserId());
        }
        public static void ClearShoppingCartPC()
        {
            new ShoppingCartDao().ClearShoppingCart(Globals.GetCurrentManagerUserId());
        }
        public static void ClearGiftShoppingCart()
        {
            new ShoppingCartDao().ClearGiftShoppingCart(Globals.GetCurrentMemberUserId());
        }


        public static ShoppingCartInfo GetGroupBuyShoppingCart(int groupbuyId, string productSkuId, int buyAmount)
        {
            ShoppingCartItemInfo shoppingCartItemInfo;
            ShoppingCartInfo info = new ShoppingCartInfo();
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            ShoppingCartItemInfo info3 = new ShoppingCartDao().GetCartItemInfo(currentMember, productSkuId, buyAmount);
            if (info3 == null)
            {
                return null;
            }
            GroupBuyInfo groupBuy = GroupBuyBrowser.GetGroupBuy(groupbuyId);
            if (((groupBuy == null) || (groupBuy.StartDate > DateTime.Now)) || (groupBuy.Status != GroupBuyStatus.UnderWay))
            {
                return null;
            }
            int count = groupBuy.Count;
            decimal price = groupBuy.Price;
            shoppingCartItemInfo = new ShoppingCartItemInfo();
            shoppingCartItemInfo.SkuId = info3.SkuId;
            shoppingCartItemInfo.ProductId = info3.ProductId;
            shoppingCartItemInfo.SKU = info3.SKU;
            shoppingCartItemInfo.Name = info3.Name;
            shoppingCartItemInfo.MemberPrice = shoppingCartItemInfo.AdjustedPrice = price;
            shoppingCartItemInfo.SkuContent = info3.SkuContent;
            shoppingCartItemInfo.Quantity = shoppingCartItemInfo.ShippQuantity = buyAmount;
            shoppingCartItemInfo.Weight = info3.Weight;
            shoppingCartItemInfo.ThumbnailUrl40 = info3.ThumbnailUrl40;
            shoppingCartItemInfo.ThumbnailUrl60 = info3.ThumbnailUrl60;
            shoppingCartItemInfo.ThumbnailUrl100 = info3.ThumbnailUrl100;
            shoppingCartItemInfo.MainCategoryPath = info3.MainCategoryPath;
            info.LineItems.Add(shoppingCartItemInfo);
            return info;
        }

        /// <summary>
        /// 代理商采购使用
        /// </summary>
        /// <param name="currentMember">前端用户信息对象</param>
        /// <param name="price">商品单价，也是实际价格</param>
        /// <param name="productSkuId">商品入库ID</param>
        /// <param name="buyAmount">购买数量</param>
        /// <returns></returns>
        public static ShoppingCartInfo GetGroupBuyShoppingCart(MemberInfo currentMember, decimal price, string productSkuId, int buyAmount)
        {
            ShoppingCartItemInfo shoppingCartItemInfo;
            ShoppingCartInfo info = new ShoppingCartInfo();
            //MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            ShoppingCartItemInfo info3 = new ShoppingCartDao().GetCartItemInfoAll(currentMember, productSkuId, buyAmount);
            if (info3 == null)
            {
                return null;
            }
            //GroupBuyInfo groupBuy = GroupBuyBrowser.GetGroupBuy(groupbuyId);
            //if (((groupBuy == null) || (groupBuy.StartDate > DateTime.Now)) || (groupBuy.Status != GroupBuyStatus.UnderWay))
            //{
            //    return null;
            //}
            //int count = groupBuy.Count;
            //decimal price = groupBuy.Price;
            shoppingCartItemInfo = new ShoppingCartItemInfo();
            shoppingCartItemInfo.SkuId = info3.SkuId;
            shoppingCartItemInfo.ProductId = info3.ProductId;
            shoppingCartItemInfo.SKU = info3.SKU;
            shoppingCartItemInfo.Name = info3.Name;
            shoppingCartItemInfo.MemberPrice = shoppingCartItemInfo.AdjustedPrice = price;
            shoppingCartItemInfo.SkuContent = info3.SkuContent;
            shoppingCartItemInfo.Quantity = shoppingCartItemInfo.ShippQuantity = buyAmount;
            shoppingCartItemInfo.Weight = info3.Weight;
            shoppingCartItemInfo.ThumbnailUrl40 = info3.ThumbnailUrl40;
            shoppingCartItemInfo.ThumbnailUrl60 = info3.ThumbnailUrl60;
            shoppingCartItemInfo.ThumbnailUrl100 = info3.ThumbnailUrl100;
            shoppingCartItemInfo.MainCategoryPath = info3.MainCategoryPath;


            info.LineItems.Add(shoppingCartItemInfo);
            return info;
        }


        public static ShoppingCartInfo GetShoppingCart(int pcUserid = 0)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null && pcUserid==0)
            {
                return null;
            }
            if (pcUserid > 0) currentMember = null;
            ShoppingCartInfo shoppingCart = new ShoppingCartDao().GetShoppingCart(currentMember, pcUserid);
            if (shoppingCart.LineItems.Count == 0 && shoppingCart.LineGifts.Count==0)
            {
                return null;
            }
            return shoppingCart;
        }

        public static List<ShoppingCartInfo> GetShoppingCartList()
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                return null;
            }
            List<ShoppingCartInfo> shoppingCart = new ShoppingCartDao().GetShoppingCartList(currentMember);
            if (shoppingCart.Count == 0)
            {
                return null;
            }
            return shoppingCart;
        }

        public static ShoppingCartInfo GetShoppingCart(string productSkuId, int buyAmount)
        {
            ShoppingCartInfo info = new ShoppingCartInfo();
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            ShoppingCartItemInfo item = new ShoppingCartDao().GetCartItemInfo(currentMember, productSkuId, buyAmount);
            if (item == null)
            {
                return null;
            }
            info.LineItems.Add(item);
            return info;
        }
        /*
        public static ShoppingCartInfo GetShoppingCart(string currentBuyProductckIds = null, bool JoinPromotion = true)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                return new CookieShoppingDao().GetShoppingCart(currentBuyProductckIds);
            }
            ShoppingCartInfo shoppingCart = new ShoppingCartDao().GetShoppingCart(HiContext.Current.User.UserId);
            if ((shoppingCart.LineItems.Count == 0) && (shoppingCart.LineGifts.Count == 0))
            {
                return null;
            }
            if (!string.IsNullOrEmpty(currentBuyProductckIds))
            {
                string[] source = currentBuyProductckIds.Split(new char[] { ',' });
                for (int i = 0; i < shoppingCart.LineItems.Count; i++)
                {
                    ShoppingCartItemInfo item = shoppingCart.LineItems[i];
                    if (!source.Contains<string>(item.SkuId))
                    {
                        shoppingCart.LineItems.Remove(item);
                        i = -1;
                    }
                }
            }
            if (currentBuyProductckIds == string.Empty)
            {
                shoppingCart.LineItems.Clear();
            }
            if (JoinPromotion)
            {
                decimal reducedAmount = 0M;
                PromotionInfo info3 = new PromotionDao().GetReducedPromotion(user, shoppingCart.GetAmount(), shoppingCart.GetQuantity(), out reducedAmount);
                if (info3 != null)
                {
                    shoppingCart.ReducedPromotionId = info3.ActivityId;
                    shoppingCart.ReducedPromotionName = info3.Name;
                    shoppingCart.ReducedPromotionAmount = reducedAmount;
                    shoppingCart.IsReduced = true;
                }
                PromotionInfo info4 = new PromotionDao().GetSendPromotion(user, shoppingCart.GetTotal(), PromoteType.FullAmountSentGift);
                if (info4 != null)
                {
                    shoppingCart.SendGiftPromotionId = info4.ActivityId;
                    shoppingCart.SendGiftPromotionName = info4.Name;
                    shoppingCart.IsSendGift = true;
                }
                PromotionInfo info5 = new PromotionDao().GetSendPromotion(user, shoppingCart.GetTotal(), PromoteType.FullAmountSentTimesPoint);
                if (info5 != null)
                {
                    shoppingCart.SentTimesPointPromotionId = info5.ActivityId;
                    shoppingCart.SentTimesPointPromotionName = info5.Name;
                    shoppingCart.IsSendTimesPoint = true;
                    shoppingCart.TimesPoint = info5.DiscountValue;
                }
                PromotionInfo info6 = new PromotionDao().GetSendPromotion(user, shoppingCart.GetTotal(), PromoteType.FullAmountSentFreight);
                if (info6 != null)
                {
                    shoppingCart.FreightFreePromotionId = info6.ActivityId;
                    shoppingCart.FreightFreePromotionName = info6.Name;
                    shoppingCart.IsFreightFree = true;
                }
                return shoppingCart;
            }
            shoppingCart.IsReduced = false;
            shoppingCart.IsSendGift = false;
            shoppingCart.IsSendTimesPoint = false;
            shoppingCart.IsFreightFree = false;
            return shoppingCart;
        }
        */
        public static List<ShoppingCartInfo> GetShoppingCartAviti(int pcUserid=0)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null && pcUserid==0)
            {
                return null;
            }
            if (pcUserid > 0) currentMember = null;
            List<ShoppingCartInfo> shoppingCartAviti = new ShoppingCartDao().GetShoppingCartAviti(currentMember, pcUserid);
            if (shoppingCartAviti.Count == 0)
            {
                return null;
            }
            return shoppingCartAviti;
        }

        public static int GetSkuStock(string skuId)
        {
            return new SkuDao().GetSkuItem(skuId).Stock;
        }

        public static void RemoveLineItem(string skuId)
        {
            new ShoppingCartDao().RemoveLineItem(Globals.GetCurrentMemberUserId(), skuId);
        }
        public static void RemoveLineItemPC(string skuId,int pcUserid)
        {
            new ShoppingCartDao().RemoveLineItem(pcUserid, skuId);
        }

        public static void UpdateLineItemQuantity(string skuId, int quantity)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (quantity <= 0)
            {
                RemoveLineItem(skuId);
            }
            new ShoppingCartDao().UpdateLineItemQuantity(currentMember, skuId, quantity);
        }
        public static void UpdateLineItemQuantityPC(string skuId, int quantity,int pcUserid) {
            new ShoppingCartDao().UpdateLineItemQuantityPC(pcUserid,skuId,quantity);
        }

        /// <summary>
        /// 20150921新增:更新GiftItem的购买数量Quantity
        /// </summary>
        /// <param name="skuId"></param>
        /// <param name="quantity"></param>
        public static void UpdateGiftItemQuantity(int giftId, int quantity)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (quantity <= 0)
            {
                RemoveGiftItem(giftId);
            }
            new ShoppingCartDao().UpdateGiftItemQuantity(currentMember, giftId, quantity);
        }

        public static void RemoveGiftItem(int giftId)
        {
            new ShoppingCartDao().RemoveGiftItem(Globals.GetCurrentMemberUserId(), giftId);
        }

        public static ShoppingCartInfo GetCountDownShoppingCart(string productSkuId, int buyAmount)
        {
            ShoppingCartItemInfo info4;
            ShoppingCartInfo info = new ShoppingCartInfo();
            ShoppingCartItemInfo info2 = new ShoppingCartDao().GetCartItemInfo(MemberProcessor.GetCurrentMember(), productSkuId, buyAmount);
            if (info2 == null)
            {
                return null;
            }
            CountDownInfo countDownInfo = ProductBrowser.GetCountDownInfo(info2.ProductId);
            if (countDownInfo == null)
            {
                return null;
            }
            info4 = new ShoppingCartItemInfo
            {
                SkuId = info2.SkuId,
                ProductId = info2.ProductId,
                SKU = info2.SKU,
                Name = info2.Name,

                SkuContent = info2.SkuContent,

                Weight = info2.Weight,
                ThumbnailUrl40 = info2.ThumbnailUrl40,
                ThumbnailUrl60 = info2.ThumbnailUrl60,
                ThumbnailUrl100 = info2.ThumbnailUrl100,
                IsfreeShipping = info2.IsfreeShipping,
                MainCategoryPath = info2.MainCategoryPath,
            };

            info4.MemberPrice = info4.AdjustedPrice = countDownInfo.CountDownPrice;
            info4.Quantity = info4.ShippQuantity = buyAmount;
            info.LineItems.Add(info4);
            return info;
        }

        public static ShoppingCartInfo GetCutDownShoppingCart(string productSkuId, int buyAmount,int cutDownId)
        {
            ShoppingCartItemInfo info5;
            ShoppingCartInfo info = new ShoppingCartInfo();
            ShoppingCartItemInfo info2 = new ShoppingCartDao().GetCartItemInfo(MemberProcessor.GetCurrentMember(), productSkuId, buyAmount);
            if (info2 == null)
            {
                return null;
            }
            CutDownInfo cutDownInfo = Hidistro.ControlPanel.Promotions.PromoteHelper.GetCutDown(cutDownId); ;
            if (cutDownInfo == null)
            {
                return null;
            }
            info5 = new ShoppingCartItemInfo
            {
                SkuId = info2.SkuId,
                ProductId = info2.ProductId,
                SKU = info2.SKU,
                Name = info2.Name,

                SkuContent = info2.SkuContent,

                Weight = info2.Weight,
                ThumbnailUrl40 = info2.ThumbnailUrl40,
                ThumbnailUrl60 = info2.ThumbnailUrl60,
                ThumbnailUrl100 = info2.ThumbnailUrl100,
                IsfreeShipping = info2.IsfreeShipping,
                MainCategoryPath = info2.MainCategoryPath,
            };

            info5.MemberPrice = info5.AdjustedPrice = cutDownInfo.CurrentPrice;
            info5.Quantity = info5.ShippQuantity = buyAmount;
            info.LineItems.Add(info5);
            return info;
        }
        /// <summary>
        /// 2016-01-22，根据用户ID得到购物车表信息
        /// </summary>
        /// <returns>购物车表数据集</returns>
        public static DataTable GetCartShopping(int pcUserid)
        {
            return new ShoppingCartDao().GetCartShopping((pcUserid == 0) ? Globals.GetCurrentMemberUserId() : pcUserid);
        }

        /// <summary>
        /// 2016-01-22，修改购物车商品信息，买一送一活动使用
        /// </summary>
        public static void UpdateLineItemBuyToGive(string skuId,int pcUserid)
        {
            new ShoppingCartDao().UpdateLineItemBuyToGive((pcUserid == 0) ? Globals.GetCurrentMemberUserId() : pcUserid, skuId);
        }

        /// <summary>
        /// 2016-10-27，修改购物车商品信息，第二杯半价活动使用
        /// </summary>
        public static void UpdateLineItemBuyHalfGive(string skuId, int pcUserid)
        {
            new ShoppingCartDao().UpdateLineItemBuyHalfGive((pcUserid == 0) ? Globals.GetCurrentMemberUserId() : pcUserid, skuId);
        }

        public static int GetShoppingCartNum()
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                return -1;
            }
            return new ShoppingCartDao().GetShoppingCartNum(currentMember.UserId);
        }

        public static decimal GetShoppingCartTotal()
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                return -1;
            }
            return new ShoppingCartDao().GetShoppingCartTotal(currentMember.UserId);
        }

    }
}

