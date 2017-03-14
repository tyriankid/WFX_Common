namespace Hidistro.ControlPanel.Promotions
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Entities.VShop;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.SqlDal.Comments;
    using Hidistro.SqlDal.Promotions;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using Hidistro.SqlDal.Members;
    using Hidistro.ControlPanel.Members;

    public static class PromoteHelper
    {
        /*
        public static bool AddBundlingProduct(BundlingInfo bundlingInfo)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    BundlingDao dao = new BundlingDao();
                    int bundlingID = dao.AddBundlingProduct(bundlingInfo, dbTran);
                    if (bundlingID <= 0)
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!dao.AddBundlingProductItems(bundlingID, bundlingInfo.BundlingItemInfos, dbTran))
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
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }
        */
        public static bool AddCountDown(CountDownInfo countDownInfo)
        {
            return new CountDownDao().AddCountDown(countDownInfo);
        }
        /*
        public static bool AddGroupBuy(GroupBuyInfo groupBuy)
        {
            bool flag;
            Globals.EntityCoding(groupBuy, true);
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    GroupBuyDao dao = new GroupBuyDao();
                    int groupBuyId = dao.AddGroupBuy(groupBuy, dbTran);
                    if (groupBuyId <= 0)
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!dao.AddGroupBuyCondition(groupBuyId, groupBuy.GroupBuyConditions, dbTran))
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
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }
        */
        /*
        public static int AddPromotion(PromotionInfo promotion)
        {
            int num2;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    PromotionDao dao = new PromotionDao();
                    int activityId = dao.AddPromotion(promotion, dbTran);
                    if (activityId <= 0)
                    {
                        dbTran.Rollback();
                        return -1;
                    }
                    if (!dao.AddPromotionMemberGrades(activityId, promotion.MemberGradeIds, dbTran))
                    {
                        dbTran.Rollback();
                        return -2;
                    }
                    dbTran.Commit();
                    num2 = activityId;
                }
                catch (Exception)
                {
                    dbTran.Rollback();
                    num2 = 0;
                }
                finally
                {
                    connection.Close();
                }
            }
            return num2;
        }
        */
        public static bool AddPromotionProducts(int activityId, string productIds)
        {
            return new PromotionDao().AddPromotionProducts(activityId, productIds);
        }

        public static bool DeleteBundlingProduct(int bundlingID)
        {
            return new BundlingDao().DeleteBundlingProduct(bundlingID);
        }

        public static bool DeleteCountDown(int countDownId)
        {
            return new CountDownDao().DeleteCountDown(countDownId);
        }

        public static bool DeleteGroupBuy(int groupBuyId)
        {
            return new GroupBuyDao().DeleteGroupBuy(groupBuyId);
        }

        public static bool DeletePromotion(int activityId)
        {
            return new PromotionDao().DeletePromotion(activityId);
        }

        public static bool DeletePromotionProducts(int activityId, int? productId)
        {
            return new PromotionDao().DeletePromotionProducts(activityId, productId);
        }
        /*
        public static int EditPromotion(PromotionInfo promotion)
        {
            int num;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    PromotionDao dao = new PromotionDao();
                    if (!dao.EditPromotion(promotion, dbTran))
                    {
                        dbTran.Rollback();
                        return -1;
                    }
                    if (!dao.AddPromotionMemberGrades(promotion.ActivityId, promotion.MemberGradeIds, dbTran))
                    {
                        dbTran.Rollback();
                        return -2;
                    }
                    dbTran.Commit();
                    num = 1;
                }
                catch (Exception)
                {
                    dbTran.Rollback();
                    num = 0;
                }
                finally
                {
                    connection.Close();
                }
            }
            return num;
        }
        
        public static BundlingInfo GetBundlingInfo(int bundlingID)
        {
            return new BundlingDao().GetBundlingInfo(bundlingID);
        }

        public static DbQueryResult GetBundlingProducts(BundlingInfoQuery query)
        {
            return new BundlingDao().GetBundlingProducts(query);
        }
        */
        public static CountDownInfo GetCountDownInfo(int countDownId)
        {
            return new CountDownDao().GetCountDownInfo(countDownId);
        }

        public static DbQueryResult GetCountDownList(GroupBuyQuery query)
        {
            return new CountDownDao().GetCountDownList(query);
        }

        public static decimal GetCurrentPrice(int groupBuyId, int prodcutQuantity)
        {
            return new GroupBuyDao().GetCurrentPrice(groupBuyId, prodcutQuantity);
        }

        public static GroupBuyInfo GetGroupBuy(int groupBuyId)
        {
            return new GroupBuyDao().GetGroupBuy(groupBuyId);
        }

        public static DbQueryResult GetGroupBuyList(GroupBuyQuery query)
        {
            return new GroupBuyDao().GetGroupBuyList(query);
        }
        /*
        public static IList<Member> GetMembersByRank(int? gradeId)
        {
            return new MessageBoxDao().GetMembersByRank(gradeId);
        }
        */
        public static IList<Member> GetMemdersByNames(IList<string> names)
        {
            IList<Member> list = new List<Member>();
            foreach (string str in names)
            {
                IUser user = Users.GetUser(0, str, false, false);
                if ((user != null) && (user.UserRole == UserRole.Member))
                {
                    list.Add(user as Member);
                }
            }
            return list;
        }

        public static int GetOrderCount(int groupBuyId)
        {
            return new GroupBuyDao().GetOrderCount(groupBuyId);
        }

        public static string GetPriceByProductId(int productId)
        {
            return new GroupBuyDao().GetPriceByProductId(productId);
        }

        public static IList<MemberGradeInfo> GetPromoteMemberGrades(int activityId)
        {
            return new PromotionDao().GetPromoteMemberGrades(activityId);
        }

        public static PromotionInfo GetPromotion(int activityId)
        {
            return new PromotionDao().GetPromotion(activityId);
        }

        public static PromotionInfo GetPromotionByProduct(int productId)
        {
            PromotionInfo promotion = null;
            int? activeIdByProduct = new PromotionDao().GetActiveIdByProduct(productId);
            if (activeIdByProduct.HasValue)
            {
                promotion = GetPromotion(activeIdByProduct.Value);
            }
            return promotion;
        }

        public static DataTable GetPromotionProducts(int activityId)
        {
            return new PromotionDao().GetPromotionProducts(activityId);
        }

        public static DataTable GetPromotions(bool isProductPromote, bool isWholesale)
        {
            return new PromotionDao().GetPromotions(isProductPromote, isWholesale);
        }
        /*
        public static CouponInfo GetShakeCoupon()
        {
            return new CouponDao().GetShakeCoupon();
        }
        */
        public static bool ProductCountDownExist(int productId)
        {
            return new CountDownDao().ProductCountDownExist(productId);
        }

        public static bool ProductGroupBuyExist(int productId)
        {
            return new GroupBuyDao().ProductGroupBuyExist(productId);
        }

        //public static int SetClaimShake(int couponId)
        //{
        //    return new CouponDao().SetClaimShake(couponId);
        //}

        public static bool SetGroupBuyEndUntreated(int groupBuyId)
        {
            return new GroupBuyDao().SetGroupBuyEndUntreated(groupBuyId);
        }

        public static bool SetGroupBuyStatus(int groupBuyId, GroupBuyStatus status)
        {
            return new GroupBuyDao().SetGroupBuyStatus(groupBuyId, status);
        }

        public static void SwapCountDownSequence(int countDownId, int displaySequence)
        {
            new CountDownDao().SwapCountDownSequence(countDownId, displaySequence);
        }

        public static void SwapGroupBuySequence(int groupBuyId, int displaySequence)
        {
            new GroupBuyDao().SwapGroupBuySequence(groupBuyId, displaySequence);
        }
        /*
        public static bool UpdateBundlingProduct(BundlingInfo bundlingInfo)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    BundlingDao dao = new BundlingDao();
                    if (!dao.UpdateBundlingProduct(bundlingInfo, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!dao.DeleteBundlingByID(bundlingInfo.BundlingID, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!dao.AddBundlingProductItems(bundlingInfo.BundlingID, bundlingInfo.BundlingItemInfos, dbTran))
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
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }
        */
        public static bool UpdateCountDown(CountDownInfo countDownInfo)
        {
            return new CountDownDao().UpdateCountDown(countDownInfo);
        }

        public static bool UpdateGroupBuy(GroupBuyInfo groupBuy)
        {
            bool flag;
            Globals.EntityCoding(groupBuy, true);
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    GroupBuyDao dao = new GroupBuyDao();
                    if (!dao.UpdateGroupBuy(groupBuy, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!dao.DeleteGroupBuyCondition(groupBuy.GroupBuyId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!dao.AddGroupBuyCondition(groupBuy.GroupBuyId, groupBuy.GroupBuyConditions, dbTran))
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
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public static bool AddGroupBuy(GroupBuyInfo groupBuy)
        {
            bool flag;
            Globals.EntityCoding(groupBuy, true);
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    GroupBuyDao dao = new GroupBuyDao();
                    int groupBuyId = dao.AddGroupBuy(groupBuy, dbTran);
                    if (groupBuyId <= 0)
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!dao.AddGroupBuyCondition(groupBuyId, groupBuy.GroupBuyConditions, dbTran))
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
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public static DbQueryResult GetCutDownList(CutDownQuery query)
        {
            return new CutDownDao().GetCutDownList(query);
        }

        public static CutDownInfo GetCutDown(int cutDownId)
        {
            return new CutDownDao().GetCutDown(cutDownId);
        }

        public static bool DeleteCutDown(int cutDownId)
        {
            return new CutDownDao().DeleteCutDown(cutDownId);
        }

        public static void SwapCutDownSequence(int cutDownId, int displaySequence)
        {
            new CutDownDao().SwapCutDownSequence(cutDownId, displaySequence);
        }

        public static bool AddCutDown(CutDownInfo cutDown)
        {
            bool flag;
            Globals.EntityCoding(cutDown, true);
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    CutDownDao dao = new CutDownDao();
                    int cutDownId = dao.AddCutDown(cutDown, dbTran);
                    if (cutDownId <= 0)
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
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public static bool ProductCutDownExist(int cutDownId)
        {
            return new CutDownDao().ProductCutDownExist(cutDownId);
        }

        public static bool UpdateCutDown(CutDownInfo cutDown)
        {
            bool flag;
            Globals.EntityCoding(cutDown, true);
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    CutDownDao dao = new CutDownDao();
                    if (!dao.UpdateCutDown(cutDown, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    /*
                    if (!dao.DeleteCutDownDetail(cutDown.CutDownId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    */
                    dbTran.Commit();
                    flag = true;
                }
                catch (Exception)
                {
                    dbTran.Rollback();
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public static int GetCutDownOrderCount(int cutDownId)
        {
            return new CutDownDao().GetCutDownOrderCount(cutDownId);
        }

        public static DbQueryResult GetCutDownDetailList(CutDownDetailsQuery query)
        {
            return new CutDownDao().GetCutDownDetailList(query);
        }

        public static decimal GetCutDownTotalPrice(int cutDownId)
        {
            return new CutDownDao().GetCutDownTotalPrice(cutDownId);
        }

        public static bool IsAlreadyCut(int cutDownId, int memberId)
        {
            return new CutDownDao().GetCutDownTotalCount(cutDownId, memberId) > 0;
        }

        public static int GetCutDownTotalCount(int cutDownId, int memberId = 0)
        {
            return new CutDownDao().GetCutDownTotalCount(cutDownId, memberId);
        }
        /// <summary>
        /// 砍价
        /// </summary>
        public static string goCutDown(CutDownDetailInfo info)
        {
            CutDownInfo cutDown = GetCutDown(info.CutDownId);
            string result = "";
            //如果当前价格超过最低价, 并且活动时间结束,并且达到最大砍价次数,则返回提示信息
            if (cutDown.CurrentPrice <= cutDown.MinPrice)
            {
                return result = "当前价格已经达到最低价,无法砍价！";
            }
            if (cutDown.EndDate <= DateTime.Now)
            {
                return result = "活动已经结束，无法砍价！";
            }
            if (cutDown.MaxCount <= GetCutDownTotalCount(cutDown.CutDownId))
            {
                return result = "当前砍价次数已经达到上限，无法砍价！";
            }
            //开始砍价
            if (new CutDownDao().goCutDown(info) && new CutDownDao().updateCurrentPrice(info.CutDownId, cutDown.PerCutPrice))
                result = "success";
            return result;
        }

        public static bool CloseCutDown(int cutdownId)
        {
            return new CutDownDao().updateCutDownStatus(cutdownId, CutDownStatus.End);
        }

        /// <summary>
        /// 增加微信答题活动
        /// </summary>
        public static bool AddWKM(WKMInfo info)
        {
            return new PromotionDao().AddWKM(info);
        }

        /// <summary>
        /// 添加背景图片
        /// </summary>
        public static bool addBackImgUrl(Guid activityId, string imgUrl)
        {
            return new PromotionDao().addBackImgUrl(activityId, imgUrl);
        }

        /// <summary>
        /// 添加logo图片
        /// </summary>
        public static bool addLogoImgUrl(Guid activityId, string imgUrl)
        {
            return new PromotionDao().addLogoImgUrl(activityId, imgUrl);
        }

        /// <summary>
        /// 添加wx分享图标
        /// </summary>
        public static bool addWxImgUrl(Guid activityId, string imgUrl)
        {
            return new PromotionDao().addWxImgUrl(activityId, imgUrl);
        }

        /// <summary>
        /// 获取背景图和logo图和wx分享图地址
        /// </summary>
        public static DataTable getBackImgUrl(Guid activityId)
        {
            return new PromotionDao().getBackImgUrl(activityId);
        }

        ///// <summary>
        ///// 设置匹配度描述信息
        ///// </summary>
        //public static bool setMatchInfoList(Guid activityId, IList<string> mStartList, IList<string> mEndList, IList<string> mDesList)
        //{
        //    if (!new PromotionDao().isMatchInfoListExist(activityId))
        //    {
        //        return new PromotionDao().addMatchInfoList(activityId, mStartList, mEndList, mDesList);
        //    }
        //    else
        //    {
        //        return new PromotionDao().updateMatchInfoList(activityId, mStartList, mEndList, mDesList);
        //    }
        //}

        /// <summary>
        /// 查询匹配度描述列表
        /// </summary>
        public static DataTable getWKMMatchInfoList(Guid activityId)
        {
            return new PromotionDao().getWKMMatchInfoList(activityId);
        }

        /// <summary>
        /// 添加广告图片
        /// </summary>
        public static bool addAdImgUrl(Guid activityid, IList<string> imgUrlList,IList<string> adLinkList)
        {
            return new PromotionDao().addAdImgUrl(activityid, imgUrlList,adLinkList);
        }

        /// <summary>
        /// 获取广告图片和链接
        /// </summary>
        public static DataTable getAdImgAndUrls(Guid activityId)
        {
            return new PromotionDao().getAdImgAndUrls(activityId);
        }

        public static bool UpdateWKM(WKMInfo info)
        {
            return new PromotionDao().UpdateWKM(info);
        }
        public static bool UpdateHishop_products(ModifyGoodsQuery query)
        {
            return new PromotionDao().UpdateHishop_products(query);
        }

        /// <summary>
        /// 获取微信答题活动实体类
        /// </summary>
        public static WKMInfo GetWKMInfo(Guid wkmId)
        {
            return new PromotionDao().GetWKMInfo(wkmId);
        }
        public static ModifyGoodsQuery GetHishop_productsListID(int CommodityID)
        {
            return new PromotionDao().GetHishop_productsListID(CommodityID);
        }

        public static WKMOptionInfo GetWKMOptInfoBySubjectId(Guid sbjId)
        {
            return new PromotionDao().GetWKMOptInfoBySubjectId(sbjId);
        }
        /// <summary>
        /// 设置出题者详情
        /// </summary>
        /// <param name="hosterId">出题者id</param>
        /// <param name="sbjIds">题目id列表</param>
        /// <param name="optIds">答案id列表</param>
        public static bool SetHosterDetail(int hosterId, IList<string> sbjIds, IList<string> optIds, Guid activityId)
        {
            return new PromotionDao().SetHosterDetail(hosterId, sbjIds, optIds, activityId);
        }

        public static int SetGuestDetail(int guestId, int hosterId, IList<string> optIds, Guid activityId)
        {
            if (!isGuestExist(hosterId, guestId, activityId))
                return new PromotionDao().SetGuestDetail(guestId, hosterId, optIds, activityId)?1:-1;
            else
                return 0;
        }

        public static DataTable GetHosterDetail(int hosterId, Guid activityId)
        {
            return new PromotionDao().GetHosterDetail(hosterId, activityId);
        }

        public static DataTable GetGuestsDetail(int hosterId, int guestId, Guid activityId)
        {
            return new PromotionDao().GetGuestsDetail(hosterId, guestId, activityId);
        }
        /// <summary>
        /// 获取匹配度
        /// </summary>
        public static int getMatchInfo(Guid activityId, int hosterId, int guestId)
        {
            return new PromotionDao().getMatchInfo(activityId, hosterId, guestId);
        }

        /// <summary>
        /// 获取匹配度列表
        /// </summary>
        public static DataTable GetMatchInfoList(int hosterId, Guid activityId)
        {
            return new PromotionDao().GetMatchInfoList(hosterId, activityId);
        }

        /// <summary>
        /// 根据匹配度获取匹配度描述
        /// </summary>
        public static string GetMatchDescription(int matchRate, Guid activityId)
        {
            return new PromotionDao().GetMatchDescription(matchRate, activityId);
        }

        /// <summary>
        /// 判断出题者是否已经出题
        /// </summary>
        public static bool isHosterExist(int hosterId, Guid activityId)
        {
            return new PromotionDao().isHosterExist(hosterId, activityId);
        }

        /// <summary>
        /// 删除活动(联动删除)
        /// </summary>
        public static bool DeleteWKMInfo(Guid activityId)
        {
            return new PromotionDao().DeleteWKMInfo(activityId);
        }

        /// <summary>
        /// 判断答题者是否已经对hoster答过了题目
        /// </summary>
        public static bool isGuestExist(int hosterId, int guestId, Guid activityId)
        {
            return new PromotionDao().isGuestExist(hosterId, guestId, activityId);
        }
        /// <summary>
        /// 查询matchrate
        /// </summary>
        /// <returns></returns>
        public static int getMatchRate(Guid activityId, int hosterId, int guestId)
        {
            return new PromotionDao().getMatchRate(activityId, hosterId, guestId);
        }

        /// <summary>
        /// 设置版权信息
        /// </summary>
        public static bool setWKMCopyRight(string copyRight, Guid activityId)
        {
            return new PromotionDao().setWKMCopyRight(copyRight, activityId);
        }

        public static string getWKMCopyRight(Guid activityId)
        {
            return new PromotionDao().getWKMCopyRight(activityId);
        }

        /// <summary>
        /// 设置引导页url
        /// </summary>
        public static bool setGuidePageUrl(string guidePageUrl, Guid activityId)
        {
            return new PromotionDao().setGuidePageUrl(guidePageUrl, activityId);
        }

        ///获取引导页url
        public static string getGuidePageUrl(Guid activityId)
        {
            return new PromotionDao().getGuidePageUrl(activityId);
        }


        public static DataTable GetUserSignInfo(int userId)
        {
            return new PromotionDao().GetUserSignInfo(userId);
        }

        public static bool isTodaySigned(int userId)
        {
            return new PromotionDao().IsTodaySigned(userId);
        }

        public static object GoSign(MemberInfo User)
        {
            if (new PromotionDao().IsTodaySigned(User.UserId))//如果今天已经签到过,则不执行签到方法
            {
                return -11;
            }

            if (new PromotionDao().GoSignToday(User.UserId)) //成功签到后发送积分
            {
                //获取签到送积分规则
                DataTable ruleDT = GetSignRule();
                string[] dayList = ruleDT.Rows[0]["NeedDays"].ToString().Split(',');
                string[] pointList = ruleDT.Rows[0]["SendPoints"].ToString().Split(',');
                int sendPoint = 0;
                int continuitySignCounts = GetContinuitySignCounts(User.UserId) ;
                //根据规则和当前连续签到的日期计算送出的积分数量
                continuitySignCounts = continuitySignCounts == 0 ? 1 : continuitySignCounts;
                for (int i = 0; i < dayList.Length; i++)
                {
                    //如果当前签到天数能被设置好的天数整除,则按照相应的索引加积分
                    if (continuitySignCounts % Convert.ToInt32(dayList[i]) == 0)
                    {
                        sendPoint = sendPoint + Convert.ToInt32(pointList[i]);
                    }
                }
                //实例化积分实体类
                PointDetailInfo point = new PointDetailInfo
                {
                    OrderId = "签到送积分",
                    UserId = User.UserId,
                    TradeDate = DateTime.Now,
                    TradeType = PointTradeType.Bounty,
                    Increased = sendPoint,
                    Points = User.Points + sendPoint
                };
                if ((point.Points > 0x7fffffff) || (point.Points < 0))
                {
                    point.Points = 0;
                }
                //送积分
                try
                {
                    new PointDetailDao().AddPointDetail(point);//积分detail表
                    User.Points = User.Points + sendPoint;
                    MemberHelper.UpdateNoLog(User);//积分主表
                    return sendPoint;
                }
                catch (Exception ex)
                { 
                    return ex.Message.ToString();
                }
                HiCache.Remove(string.Format("DataCache-Member-{0}", User.UserId));

            }
            else
            {
                return -1;
            }
            

        }
        /// <summary>
        /// 获取签到奖励积分规则
        /// </summary>
        public static DataTable GetSignRule()
        {
            return new PromotionDao().GetSignRule();
        }

        /// <summary>
        /// 获取用户连续签到天数
        /// </summary>
        public static int GetContinuitySignCounts(int userId)
        {
            DataTable signDays = new PromotionDao().GetAllSignTime(userId);
            IList<DateTime> signDaysList =new List<DateTime>();
            //首先要加上今天的时间
            bool isTodaySigned=new PromotionDao().IsTodaySigned(userId);
            if(!isTodaySigned)
                signDaysList.Add(DateTime.Now);
            foreach (DataRow row in signDays.Rows)
            {
                signDaysList.Add(Convert.ToDateTime(row["SignTime"].ToString()));
            }
            int count = 0;
            for (int i = 0; i < signDaysList.Count - 1; i++)
            {
                TimeSpan ts = Convert.ToDateTime(signDaysList[i].ToString("yyyy-MM-dd")) - Convert.ToDateTime(signDaysList[i + 1].ToString("yyyy-MM-dd"));
                if (ts.Days == 1)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            if (isTodaySigned)
            {
                count = count + 1;
            }
            return count;
        }

        public static bool SetSignRoles(string days, string points,int state)
        {
            return new PromotionDao().SetSignRule(days, points, state);
        }
    }
}

