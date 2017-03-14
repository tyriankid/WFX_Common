namespace Hidistro.SaleSystem.Vshop
{
    using System.Linq;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.VShop;
    using Hidistro.SqlDal.Members;
    using Hidistro.SqlDal.Orders;
    using Hidistro.SqlDal.VShop;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Caching;
    using Hidistro.Entities.Commodities;
    using Hidistro.ControlPanel.Config;
    using Hidistro.ControlPanel.Commodities;


    public class DistributorsBrower
    {
        public static bool AddBalanceDrawRequest(BalanceDrawRequestInfo balancerequestinfo)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            DistributorsInfo currentDistributors = GetCurrentDistributors();
            if ((((currentMember != null) && !string.IsNullOrEmpty(currentMember.RealName)) && ((currentDistributors != null) && (currentDistributors.UserId > 0))) && !string.IsNullOrEmpty(currentMember.CellPhone))
            {
                if (!(string.IsNullOrEmpty(balancerequestinfo.MerchanCade) || !(currentDistributors.RequestAccount != balancerequestinfo.MerchanCade)))
                {
                    new DistributorsDao().UpdateDistributorById(balancerequestinfo.MerchanCade, currentMember.UserId);
                }
                balancerequestinfo.AccountName = currentMember.RealName;
                balancerequestinfo.UserId = currentMember.UserId;
                balancerequestinfo.UserName = currentMember.UserName;
                balancerequestinfo.CellPhone = currentMember.CellPhone;
                return new DistributorsDao().AddBalanceDrawRequest(balancerequestinfo);
            }
            return false;
        }

        public static void AddDistributorProductId(List<int> productList)
        {
            int userId = GetCurrentDistributors().UserId;
            if ((userId > 0) && (productList.Count > 0))
            {
                new DistributorsDao().RemoveDistributorProducts(productList, userId);
                foreach (int num2 in productList)
                {
                    new DistributorsDao().AddDistributorProducts(num2, userId);
                }
            }
        }

        public static bool AddDistributors(DistributorsInfo distributors)
        {
            if (IsExiteDistributorsByStoreName(distributors.StoreName) > 0)
            {
                return false;
            }
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            distributors.DistributorGradeId = DistributorGrade.OneDistributor;//分销商级别ID
            distributors.ParentUserId = new int?(currentMember.UserId);//所属上一级分销商ID,默认为自己
            distributors.UserId = currentMember.UserId;//会员ID
            DistributorsInfo currentDistributors = GetCurrentDistributors();
            if (currentDistributors != null)//如果当前访问的分销商存在
            {
                //所属上级分销商ID,多级用|分隔
                if (!(string.IsNullOrEmpty(currentDistributors.ReferralPath) || currentDistributors.ReferralPath.Contains("|")))
                {
                    distributors.ReferralPath = currentDistributors.ReferralPath + "|" + currentDistributors.UserId.ToString();
                }
                else if (!(string.IsNullOrEmpty(currentDistributors.ReferralPath) || !currentDistributors.ReferralPath.Contains("|")))
                {
                    distributors.ReferralPath = currentDistributors.ReferralPath.Split(new char[] { '|' })[1] + "|" + currentDistributors.UserId.ToString();
                }
                else
                {
                    distributors.ReferralPath = currentDistributors.UserId.ToString();
                }
                distributors.ParentUserId = new int?(currentDistributors.UserId);//所属上一级分销商ID
                if (currentDistributors.DistributorGradeId == DistributorGrade.OneDistributor)          //顶级(分销商相对于厂家的级别)
                {
                    distributors.DistributorGradeId = DistributorGrade.TowDistributor;
                }
                else if (currentDistributors.DistributorGradeId == DistributorGrade.TowDistributor)     //二级(分销商相对于厂家的级别)
                {
                    distributors.DistributorGradeId = DistributorGrade.ThreeDistributor;
                }
                else
                {
                    distributors.DistributorGradeId = DistributorGrade.ThreeDistributor;                //子级(分销商相对于厂家的级别)
                }
            }
            //设置所属代理商(By jinhb 20150820)
            distributors.IsAgent = 0;
            if (!string.IsNullOrEmpty(distributors.ReferralPath))
            {
                //得到所属分销商，从最近的往上找，找到就COPY对应的代理商累加字符串联
                DistributorsInfo parentDistributors = null;
                string[] arrayReferralPath = distributors.ReferralPath.Split('|');
                parentDistributors = GetCurrentDistributors(int.Parse(arrayReferralPath[arrayReferralPath.Length - 1]));
                if (parentDistributors.IsAgent == 1)
                {
                    distributors.AgentPath = (string.IsNullOrEmpty(parentDistributors.AgentPath))
                        ? parentDistributors.UserId.ToString() : parentDistributors.AgentPath + "|" + parentDistributors.UserId.ToString();
                }
                else
                {
                    distributors.AgentPath = parentDistributors.AgentPath;
                }
            }
            if (string.IsNullOrEmpty(distributors.AgentPath))
                return new DistributorsDao().CreateDistributor(distributors);   //分销商没有所属代理商
            else
                return new DistributorsDao().CreateAgent(distributors);   //分销商有所属代理商
        }

        public static void DeleteDistributorProductIds(List<int> productList)
        {
            int userId = GetCurrentDistributors().UserId;
            if ((userId > 0) && (productList.Count > 0))
            {
                new DistributorsDao().RemoveDistributorProducts(productList, userId);
            }
        }

        public static bool FrozenCommision(int userid, string ReferralStatus)
        {
            RemoveDistributorCache(userid);
            return new DistributorsDao().FrozenCommision(userid, ReferralStatus);
        }

        public static DbQueryResult GetBalanceDrawRequest(BalanceDrawRequestQuery query)
        {
            return new DistributorsDao().GetBalanceDrawRequest(query);
        }

        public static bool GetBalanceDrawRequestIsCheck(int serialid)
        {
            return new DistributorsDao().GetBalanceDrawRequestIsCheck(serialid);
        }

        public static DbQueryResult GetCommissions(CommissionsQuery query)
        {
            return new DistributorsDao().GetCommissions(query);
        }

        public static DistributorsInfo GetCurrentDistributors()
        {
            return GetCurrentDistributors(Globals.GetCurrentDistributorId());
        }

        public static DistributorsInfo GetCurrentDistributors(int userId)
        {
            DistributorsInfo distributorInfo = HiCache.Get(string.Format("DataCache-Distributor-{0}", userId)) as DistributorsInfo;
            if ((distributorInfo == null) || (distributorInfo.UserId == 0))
            {
                distributorInfo = new DistributorsDao().GetDistributorInfo(userId);
                HiCache.Insert(string.Format("DataCache-Distributor-{0}", userId), distributorInfo, 360, CacheItemPriority.Normal);
            }
            return distributorInfo;
        }

        public static DataTable GetCurrentDistributorsCommosion()
        {
            return new DistributorsDao().GetDistributorsCommosion(Globals.GetCurrentDistributorId());
        }

        public static DataTable GetCurrentDistributorsCommosion(int userId)
        {
            return new DistributorsDao().GetCurrentDistributorsCommosion(userId);
        }

        public static int GetDistributorGrades(string ReferralUserId)
        {
            DistributorsInfo userIdDistributors = GetUserIdDistributors(int.Parse(ReferralUserId));
            List<DistributorGradeInfo> distributorGrades = new DistributorsDao().GetDistributorGrades() as List<DistributorGradeInfo>;
            foreach (DistributorGradeInfo info2 in from item in distributorGrades
                                                   orderby item.CommissionsLimit descending
                                                   select item)
            {
                if (userIdDistributors.DistriGradeId == info2.GradeId)
                {
                    return 0;
                }
                if (info2.CommissionsLimit <= (userIdDistributors.ReferralBlance + userIdDistributors.ReferralRequestBalance))
                {
                    userIdDistributors.DistriGradeId = info2.GradeId;
                    return info2.GradeId;
                }
            }
            return 0;
        }

        public static DistributorsInfo GetDistributorInfo(int distributorid)
        {
            return new DistributorsDao().GetDistributorInfo(distributorid);
        }

        public static int GetDistributorNum(DistributorGrade grade)
        {
            return new DistributorsDao().GetDistributorNum(grade);
        }

        public static DataSet GetDistributorOrder(OrderQuery query)
        {
            return new OrderDao().GetDistributorOrder(query);
        }
        /// <summary>
        /// 获取天使下面所有分销商的所有订单信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DbQueryResult GetUnderOrders(OrderQuery query)
        {
            return new OrderDao().GetUnderOrders(query);
        }

        public static int GetDistributorOrderCount(OrderQuery query)
        {
            return new OrderDao().GetDistributorOrderCount(query);
        }

        public static DbQueryResult GetDistributors(DistributorsQuery query)
        {
            return new DistributorsDao().GetDistributors(query);
        }

        public static DataTable GetDistributorsCommission(DistributorsQuery query)
        {
            return new DistributorsDao().GetDistributorsCommission(query);
        }

        public static DataTable GetDistributorsCommosion(int userId, DistributorGrade distributorgrade)
        {
            return new DistributorsDao().GetDistributorsCommosion(userId, distributorgrade);
        }

        public static int GetDownDistributorNum(string userid)
        {
            return new DistributorsDao().GetDownDistributorNum(userid);
        }

        public static DataTable GetDownDistributors(DistributorsQuery query)
        {
            return new DistributorsDao().GetDownDistributors(query);
        }

        public static DataTable GetDownDistributor(int distributorId, string startDate = "", string endDate = "")
        {
            return new DistributorsDao().GetDownDistributor(distributorId,startDate,endDate);
        }
         /// <summary>
        /// 获取当前的distributor
        /// </summary>
        public static DataTable GetDistributor(int distributorId, string startDate = "", string endDate = "")
        {
            return new DistributorsDao().GetDistributor(distributorId, startDate,endDate);
        }

        /// <summary>
        /// 得到所有一级代理商
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllFirstDis(string startDate = "", string endDate = "")
        {
            return new DistributorsDao().GetAllFirstDis(startDate,endDate);
        }
         //获得下级
        public static DataTable GetDownDis(int UserID, string startDate = "", string endDate = "")
        {
            return new DistributorsDao().GetDownDis(UserID,startDate,endDate);
        }

        public static DataTable GetThreeDownDistributors(DistributorsQuery query)
        {
            return new DistributorsDao().GetThreeDownDistributors(query);
        }

        public static DataTable GetDownDistributorsAndAgents(DistributorsQuery query)
        {
            return new DistributorsDao().GetDownDistributorsAndAgents(query);
        }

        public static DataTable GetDownDistributorsAndA(DistributorsQuery query)
        {
            return new DistributorsDao().GetDownDistributorsAndA(query);
        }

        public static DataTable GetFirstDistributors(string startDate = "", string endDate = "")
        {
            return new DistributorsDao().GetFirstDistributors(startDate,endDate);
        }

        public static int GetNotDescDistributorGrades(string ReferralUserId)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            DistributorsInfo userIdDistributors = GetUserIdDistributors(int.Parse(ReferralUserId));
            decimal num2 = userIdDistributors.ReferralBlance + userIdDistributors.ReferralRequestBalance;//用于判断的变量:佣金
            decimal num3 = userIdDistributors.OrdersTotal;//用于判断的变量:销售额
            DistributorGradeInfo distributorGradeInfo = DistributorGradeBrower.GetDistributorGradeInfo(userIdDistributors.DistriGradeId);
            //增加了升级判断类型:根据分销商的销售价来判断
            switch (masterSettings.DistributorUpgradeType)
            {
                case "byComm":
                    //根据分销商的佣金判断
                    if ((distributorGradeInfo != null) && (num2 < distributorGradeInfo.CommissionsLimit))
                    {
                        return userIdDistributors.DistriGradeId;
                    }
                    List<DistributorGradeInfo> distributorGrades = new DistributorsDao().GetDistributorGrades() as List<DistributorGradeInfo>;
                    foreach (DistributorGradeInfo info3 in from item in distributorGrades
                                                           orderby item.CommissionsLimit descending
                                                           select item)
                    {
                        if (userIdDistributors.DistriGradeId == info3.GradeId)
                        {
                            return 0;
                        }
                        if (info3.CommissionsLimit <= (userIdDistributors.ReferralBlance + userIdDistributors.ReferralRequestBalance))
                        {
                            return info3.GradeId;
                        }
                    }
                    break;
                case "byOrdersTotal":
                    //根据分销商的销售额判断
                    if ((distributorGradeInfo != null) && (num3 < distributorGradeInfo.CommissionsLimit))
                    {
                        return userIdDistributors.DistriGradeId;
                    }
                    List<DistributorGradeInfo> distributorGrades2 = new DistributorsDao().GetDistributorGrades() as List<DistributorGradeInfo>;
                    foreach (DistributorGradeInfo info4 in from item in distributorGrades2
                                                           orderby item.CommissionsLimit descending
                                                           select item)
                    {
                        if (userIdDistributors.DistriGradeId == info4.GradeId)
                        {
                            return 0;
                        }
                        if (info4.CommissionsLimit <= (new DistributorsDao().GetDistributorDirectOrderTotal(userIdDistributors.UserId)))
                        {
                            return info4.GradeId;
                        }
                    }
                    break;
            }

            return 0;
        }

        public static DataTable GetNotSendRedpackRecord(int balancedrawrequestid)
        {
            return new SendRedpackRecordDao().GetNotSendRedpackRecord(balancedrawrequestid);
        }

        public static int GetRedPackTotalAmount(int balancedrawrequestid, int userid)
        {
            return new SendRedpackRecordDao().GetRedPackTotalAmount(balancedrawrequestid, userid);
        }

        public static SendRedpackRecordInfo GetSendRedpackRecordByID(int id)
        {
            return new SendRedpackRecordDao().GetSendRedpackRecordByID(id);
        }

        public static DbQueryResult GetSendRedpackRecordRequest(SendRedpackRecordQuery query)
        {
            return new SendRedpackRecordDao().GetSendRedpackRecordRequest(query);
        }

        public static decimal GetUserCommissions(int userid, DateTime fromdatetime)
        {
            return new DistributorsDao().GetUserCommissions(userid, fromdatetime);
        }

        public static DistributorsInfo GetUserIdDistributors(int userid)
        {
            return new DistributorsDao().GetDistributorInfo(userid);
        }

        public static DataSet GetUserRanking(int userid)
        {
            return new DistributorsDao().GetUserRanking(userid);
        }

        public static bool HasDrawRequest(int serialid)
        {
            return new SendRedpackRecordDao().HasDrawRequest(serialid);
        }

        public static int IsExiteDistributorsByStoreName(string stroname)
        {
            return new DistributorsDao().IsExiteDistributorsByStoreName(stroname);
        }

        public static bool IsExitsCommionsRequest()
        {
            return new DistributorsDao().IsExitsCommionsRequest(Globals.GetCurrentDistributorId());
        }

        public static DataTable OrderIDGetCommosion(string orderid)
        {
            return new DistributorsDao().OrderIDGetCommosion(orderid);
        }

        public static void RemoveDistributorCache(int userId)
        {
            HiCache.Remove(string.Format("DataCache-Distributor-{0}", userId));
        }

        public static string SendRedPackToBalanceDrawRequest(int serialid)
        {
            return new DistributorsDao().SendRedPackToBalanceDrawRequest(serialid);
        }

        public static bool setCommission(OrderInfo order, DistributorsInfo DisInfo)
        {
            bool flag = false;
            decimal num = 0M;
            decimal num2 = 0M;
            decimal resultCommTatal = 0M;
            string userId = order.ReferralUserId.ToString();
            string orderId = order.OrderId;
            decimal orderTotal = 0M;
            ArrayList gradeIdList = new ArrayList();
            ArrayList referralUserIdList = new ArrayList();
            foreach (LineItemInfo info in order.LineItems.Values)
            {
                if (info.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
                {
                    num2 += info.ItemsCommission;
                    if (!(string.IsNullOrEmpty(info.ItemAdjustedCommssion.ToString()) || (info.ItemAdjustedCommssion <= 0M)))
                    {
                        num += info.ItemAdjustedCommssion;
                    }
                    orderTotal += info.GetSubTotal();
                }
            }
            resultCommTatal = num2 - num;
            flag = new DistributorsDao().UpdateCalculationCommission(userId, userId, orderId, orderTotal, resultCommTatal);
            int notDescDistributorGrades = GetNotDescDistributorGrades(userId);
            if (notDescDistributorGrades > 0)
            {
                gradeIdList.Add(notDescDistributorGrades);
                referralUserIdList.Add(userId);
                flag = new DistributorsDao().UpdateGradeId(gradeIdList, referralUserIdList);
            }
            return flag;
        }

        public static bool SetRedpackRecordIsUsed(int id, bool issend)
        {
            return new SendRedpackRecordDao().SetRedpackRecordIsUsed(id, issend);
        }

        /// <summary>
        /// jinhb,20150820，更新三级返佣金额，加入了无限代理佣金算法
        /// </summary>
        public static bool UpdateCalculationCommission(OrderInfo order)
        {
            DistributorsInfo userIdDistributors = GetUserIdDistributors(order.ReferralUserId);

            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            bool flag = false;
            if (userIdDistributors != null)
            {
                bool isAgent1 = (userIdDistributors.IsAgent == 1) ? true : false;
                bool isAgent2 = false;
                bool isAgent3 = false;
                if (!masterSettings.EnableCommission)//未启用三级返佣
                {
                    if (userIdDistributors.ReferralStatus == 0)//状态正常
                    {
                        flag = setCommission(order, userIdDistributors);//设置分销商佣金(仅更新直接销售的分销商返佣)
                    }
                }
                else//启用了三级返佣
                {
                    if (userIdDistributors.ReferralStatus == 0)
                    {
                        flag = setCommission(order, userIdDistributors);//更新直接销售的分销商返佣
                    }
                    if (!string.IsNullOrEmpty(userIdDistributors.ReferralPath))
                    {
                        ArrayList commTatalList = new ArrayList();
                        decimal num = 0M;
                        ArrayList userIdList = new ArrayList();
                        string referralUserId = order.ReferralUserId.ToString();
                        string orderId = order.OrderId;
                        ArrayList orderTotalList = new ArrayList();
                        decimal num2 = 0M;
                        ArrayList gradeIdList = new ArrayList();
                        string[] strArray = userIdDistributors.ReferralPath.Split(new char[] { '|' });
                        if (strArray.Length == 1)
                        {
                            #region 上一级返佣
                            DistributorsInfo info2 = GetUserIdDistributors(int.Parse(strArray[0]));
                            isAgent2 = (info2.IsAgent == 1) ? true : false;
                            if (info2.ReferralStatus == 0)
                            {
                                foreach (LineItemInfo info3 in order.LineItems.Values)
                                {
                                    if (info3.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
                                    {
                                        num += info3.SecondItemsCommission;
                                        num2 += info3.GetSubTotal();
                                    }
                                }
                                commTatalList.Add(num);
                                orderTotalList.Add(num2);
                                userIdList.Add(info2.UserId);
                            }
                            #endregion 上一级返佣
                        }
                        if (strArray.Length == 2)
                        {
                            #region 上一级、上二级返佣
                            DistributorsInfo info4 = GetUserIdDistributors(int.Parse(strArray[0]));
                            isAgent3 = (info4.IsAgent == 1) ? true : false;
                            if (info4.ReferralStatus == 0)
                            {
                                foreach (LineItemInfo info3 in order.LineItems.Values)
                                {
                                    if (info3.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
                                    {
                                        num += info3.ThirdItemsCommission;
                                        num2 += info3.GetSubTotal();
                                    }
                                }
                                commTatalList.Add(num);
                                orderTotalList.Add(num2);
                                userIdList.Add(info4.UserId);
                            }
                            DistributorsInfo info5 = GetUserIdDistributors(int.Parse(strArray[1]));
                            isAgent2 = (info5.IsAgent == 1) ? true : false;
                            num = 0M;
                            num2 = 0M;
                            if (info5.ReferralStatus == 0)
                            {
                                foreach (LineItemInfo info3 in order.LineItems.Values)
                                {
                                    if (info3.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
                                    {
                                        num += info3.SecondItemsCommission;
                                        num2 += info3.GetSubTotal();
                                    }
                                }
                                commTatalList.Add(num);
                                orderTotalList.Add(num2);
                                userIdList.Add(info5.UserId);
                            }
                            #endregion 上一级、上二级返佣

                            //三级向上存在代理时进行设置
                            if (!string.IsNullOrEmpty(info4.AgentPath) && CustomConfigHelper.Instance.SelectServerAgent==false)
                            {
                                /*20160424,by 迪蔓。后期服务门店类时可通用
                                1.消费者购买时可选择服务门店(天使,下拉框列表)
                                1.1如果服务门店选择无，则按正常三级返佣(三级之外的不参与返佣，如无限级代理)
                                1.2如果选择了服务门店，正常三级返佣不变(同1.1规则)，额外10%的比例给服务门店(同时支持天使的返佣)
                                1.3店铺订单，列表中额外查出服务订单
                                */

                                    System.Data.DataView defaultViewAgent = DistributorGradeBrower.GetAllAgentGrade().DefaultView;  //当前系统中所有代理商等级
                                    #region 设置无限代理返佣

                                    string[] arrayAgentPath = info4.AgentPath.Split(new char[] { '|' });
                                    foreach (string strAgentPat in arrayAgentPath)
                                    {
                                        DistributorsInfo infoAgent = GetUserIdDistributors(int.Parse(strAgentPat));
                                        if (infoAgent.IsAgent != 1 || infoAgent.ReferralStatus == 1) continue;
                                        defaultViewAgent.RowFilter = " AgentGradeId=" + infoAgent.AgentGradeId;
                                        if (defaultViewAgent.Count == 0) continue;
                                        num = 0M;
                                        num2 = 0M;
                                        foreach (LineItemInfo info in order.LineItems.Values)
                                        {
                                            if (info.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
                                            {
                                                if (!SettingsManager.GetMasterSettings(false).EnableProfit)
                                                    num += decimal.Parse(defaultViewAgent[0]["FirstCommissionRise"].ToString()) / 100m * info.GetSubTotal();
                                                else
                                                    num += decimal.Parse(defaultViewAgent[0]["FirstCommissionRise"].ToString()) / 100m * info.GetSubTotalProfit();

                                                num2 += info.GetSubTotal();
                                            }
                                        }
                                        commTatalList.Add(num);
                                        orderTotalList.Add(num2);
                                        userIdList.Add(infoAgent.UserId);

                                        //齐品汇的代理商特殊返佣算法,此处为代理商在三级分佣之外时的情况:实际返佣=商品售价-出货折扣-三级返佣
                                        if (CustomConfigHelper.Instance.IsQipinhui)
                                        {
                                            decimal num_new = 0m;
                                            foreach (LineItemInfo info in order.LineItems.Values)
                                            {
                                                if (info.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
                                                {
                                                    int categoryId = int.Parse(info.MainCategoryPath.TrimEnd('|'));//分类id
                                                    string specialName = "";//当前代理商的名字
                                                    specialName = DistributorGradeBrower.GetAgentGradeInfo(infoAgent.AgentGradeId).AgentGradeName;

                                                    //通过分类id和代理商名字获取特殊的分佣比例
                                                    decimal currentRate = Convert.ToDecimal(CatalogHelper.GetSpecialCategoryRent(categoryId, specialName));
                                                    string noCommOpenids = CustomConfigHelper.Instance.NoCommOpenids != "" ? CustomConfigHelper.Instance.NoCommOpenids.ToLower() : "";//先获取不参加返佣的4个openid
                                                    decimal currentSubTotal = CatalogHelper.isSpecialRateExist(categoryId) ? info.GetSubTotal() * currentRate : info.GetSubTotalByCostPrice();//根据当前分类id判断当前是用特殊比例计算还是使用成本价计算

                                                    if (infoAgent.OpenId != null && noCommOpenids.IndexOf(infoAgent.OpenId.ToLower()) >= 0)
                                                    {
                                                        num_new = 0;
                                                    }
                                                    else
                                                    {
                                                        num_new = info.GetSubTotal() - currentSubTotal - (info.ItemsCommission + info.SecondItemsCommission + info.ThirdItemsCommission);
                                                    }
                                                    /*if (infoAgent.OpenId != null)
                                                        num_new = noCommOpenids.IndexOf(infoAgent.OpenId.ToLower()) >= 0 ? 0 : (info.GetSubTotal() - currentSubTotal - (info.ItemsCommission + info.SecondItemsCommission + info.ThirdItemsCommission));
                                                    else
                                                    {
                                                        num_new = info.GetSubTotal() - currentSubTotal - (info.ItemsCommission + info.SecondItemsCommission + info.ThirdItemsCommission);
                                                    }
                                                    */
                                                }
                                            }


                                            commTatalList[commTatalList.Count - 1] = num_new;
                                            break;
                                        }


                                    }

                                    #endregion 设置无限代理返佣

                            }

                            #region 迪蔓,只返一次无限级
                            if (CustomConfigHelper.Instance.SelectServerAgent) {
                                if (isAgent1 == false && isAgent2 == false && isAgent3 == false)
                                { 
                                    //三级之外存在店铺时，向上找最近的代理商
                                    while (!string.IsNullOrEmpty(info4.ReferralPath))
                                    { 
                                        string[] arrayReferralPath = info4.ReferralPath.Split(new char[] { '|' });
                                        info4 = GetUserIdDistributors(int.Parse(arrayReferralPath[arrayReferralPath.Length-1]));
                                        if (info4.IsAgent == 1)
                                        {
                                            System.Data.DataView defaultViewAgent = DistributorGradeBrower.GetAllAgentGrade().DefaultView;  //当前系统中所有代理商等级
                                            defaultViewAgent.RowFilter = " AgentGradeId=" + info4.AgentGradeId;
                                            if (defaultViewAgent.Count == 0) break;
                                            num = 0M;
                                            num2 = 0M;
                                            foreach (LineItemInfo info in order.LineItems.Values)
                                            {
                                                if (info.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
                                                {
                                                    if (!SettingsManager.GetMasterSettings(false).EnableProfit)
                                                        num += decimal.Parse(defaultViewAgent[0]["FirstCommissionRise"].ToString()) / 100m * info.GetSubTotal();
                                                    else
                                                        num += decimal.Parse(defaultViewAgent[0]["FirstCommissionRise"].ToString()) / 100m * info.GetSubTotalProfit();

                                                    num2 += info.GetSubTotal();
                                                }
                                            }
                                            commTatalList.Add(num);
                                            orderTotalList.Add(num2);
                                            userIdList.Add(info4.UserId);//添加无限级代理商佣金
                                            break;
                                        }
                                    }
                                }
                            }
                            #endregion 
                        }
                        #region 51跟单特殊处理,如果是分销商在自己店铺购买商品,三级佣金往上推一层(直接佣金为0推至第四级佣金)
                        if (CustomConfigHelper.Instance.Is51gendan && order.UserId == order.ReferralUserId)
                        {
                            commTatalList = new ArrayList();
                            userIdList = new ArrayList();
                            orderTotalList = new ArrayList();
                            num = 0m; num2 = 0m;
                            //获取第四级分销商id
                            string[] strArrayNew = userIdDistributors.ReferralPath.Split(new char[] { '|' });
                            DistributorsInfo user4 = new DistributorsInfo();
                            if (strArrayNew.Length == 2)
                            {
                                user4 = GetUserIdDistributors(GetUserIdDistributors(strArrayNew[0].ToInt()).ReferralUserId);//第四级的分销商为当前分销商的第三级的上一级分销商
                            }


                            //一级返佣为0
                            DistributorsInfo user1 = GetUserIdDistributors(order.ReferralUserId);
                            if (user1.ReferralStatus == 0)
                            {
                                foreach (LineItemInfo item1 in order.LineItems.Values)
                                {
                                    if (item1.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
                                    {
                                        num += 0;//一级不加返佣
                                        num2 += item1.GetSubTotal();
                                    }
                                }
                                commTatalList.Add(num); num = 0m;
                                orderTotalList.Add(num2); num2 = 0m;
                                userIdList.Add(user1.UserId);
                            }
                            //一共有二级的返佣处理
                            if (strArrayNew.Length == 1)
                            {
                                DistributorsInfo user2 = GetUserIdDistributors(int.Parse(strArray[0]));
                                if (user2.ReferralStatus == 0)
                                {
                                    foreach (LineItemInfo item2 in order.LineItems.Values)
                                    {
                                        if (item2.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
                                        {
                                            num += item2.ItemsCommission;
                                            num2 += item2.GetSubTotal();
                                        }
                                    }
                                    commTatalList.Add(num);
                                    orderTotalList.Add(num2);
                                    userIdList.Add(user2.UserId);
                                }
                            }
                            //一共有三级的返佣处理
                            if (strArray.Length == 2)
                            {
                                DistributorsInfo user2 = GetUserIdDistributors(int.Parse(strArrayNew[0]));
                                if (user2.ReferralStatus == 0)
                                {
                                    foreach (LineItemInfo item2 in order.LineItems.Values)
                                    {
                                        if (item2.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
                                        {
                                            num += item2.SecondItemsCommission;
                                            num2 += item2.GetSubTotal();
                                        }
                                    }
                                    commTatalList.Add(num); num = 0M;
                                    orderTotalList.Add(num2); num2 = 0M;
                                    userIdList.Add(user2.UserId);
                                }
                                DistributorsInfo user3 = GetUserIdDistributors(int.Parse(strArrayNew[1]));
                                if (user3.ReferralStatus == 0)
                                {
                                    foreach (LineItemInfo item3 in order.LineItems.Values)
                                    {
                                        if (item3.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
                                        {
                                            num += item3.ItemsCommission;
                                            num2 += item3.GetSubTotal();
                                        }
                                    }
                                    commTatalList.Add(num); num = 0m;
                                    orderTotalList.Add(num2); num2 = 0m;
                                    userIdList.Add(user3.UserId);
                                }
                            }
                            //四级返佣
                            if (user4 != null && user4.ReferralStatus == 0)
                            {
                                foreach (LineItemInfo item4 in order.LineItems.Values)
                                {
                                    if (item4.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
                                    {
                                        num += item4.ThirdItemsCommission;
                                        num2 += item4.GetSubTotal();
                                    }
                                }
                                commTatalList.Add(num);
                                orderTotalList.Add(num2);
                                userIdList.Add(user4.UserId);

                            }
                        }
                        #endregion

                        flag = new DistributorsDao().UpdateTwoCalculationCommission(userIdList, referralUserId, orderId, orderTotalList, commTatalList);
                        for (int i = 0; i < userIdList.Count; i++)
                        {
                            int notDescDistributorGrades = GetNotDescDistributorGrades(userIdList[i].ToString());
                            gradeIdList.Add(notDescDistributorGrades);
                        }
                        flag = new DistributorsDao().UpdateGradeId(gradeIdList, userIdList);
                    }


                }
                RemoveDistributorCache(userIdDistributors.UserId);
            }
            
            
                //额外10%的比例给服务门店
                if (order.RedPagerID > 0)//如果选中了门店
                {
                    #region 迪蔓门店服务费10%
                    ArrayList commTatalList = new ArrayList();
                    ArrayList userIdList = new ArrayList();
                    string referralUserId = order.ReferralUserId.ToString();
                    string orderId = order.OrderId;
                    ArrayList orderTotalList = new ArrayList();
                    decimal num = 0M;
                    decimal num2 = 0M;
                    foreach (LineItemInfo info in order.LineItems.Values)
                    {
                        if (info.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
                        {
                            if (!SettingsManager.GetMasterSettings(false).EnableProfit)
                                num += decimal.Parse("10") / 100m * info.GetSubTotal();
                            else
                                num += decimal.Parse("10") / 100m * info.GetSubTotalProfit();

                            num2 += info.GetSubTotal();
                        }
                    }
                    commTatalList.Add(num);
                    orderTotalList.Add(num2);
                    userIdList.Add(order.RedPagerID);
                    flag = new DistributorsDao().UpdateTwoCalculationCommission(userIdList, referralUserId, orderId, orderTotalList, commTatalList);
                    #endregion
                }

            
            OrderRedPagerBrower.CreateOrderRedPager(order.OrderId, order.GetTotal(), order.UserId);
            return flag;
        }

        public static bool UpdateDistributor(DistributorsInfo query)
        {
            int num = IsExiteDistributorsByStoreName(query.StoreName);
            if ((num != 0) && (num != query.UserId))
            {
                return false;
            }
            return new DistributorsDao().UpdateDistributor(query);
        }

        public static bool UpdateDistributorMessage(DistributorsInfo query)
        {
            int num = IsExiteDistributorsByStoreName(query.StoreName);
            if ((num != 0) && (num != query.UserId))
            {
                return false;
            }
            return new DistributorsDao().UpdateDistributorMessage(query);
        }

        public static DataTable GetDistributorsByWhere(string where)
        {
            return new DistributorsDao().SelectDistributorsByWhere(where);
        }
        public static void CommitDistributors(DataTable dtData)
        {
            new DistributorsDao().CommitDistributors(dtData);
        }

        public static DataTable GetDistributorProductRangeByUserid(int userid)
        {
            return new DistributorsDao().GetDistributorProductRangeByUserid(userid);
        }
        public static void CommitDistributorProductRange(DataTable dtData)
        {
            new DistributorsDao().CommitDistributorProductRange(dtData);
        }

        /// <summary>
        /// 获取当前店铺商品限定范围的枚举
        /// </summary>
        /// <returns></returns>
        public static ProductInfo.ProductRanage GetCurrStoreProductRange()
        {
            ProductInfo.ProductRanage productRanage = ProductInfo.ProductRanage.NormalSelect;
            DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors();
            if ((currentDistributors != null) && (currentDistributors.UserId != 0))
            {
                if (Hidistro.Core.SettingsManager.GetMasterSettings(false).EnableStoreProductAuto)
                    productRanage = ProductInfo.ProductRanage.All;
                else if (Hidistro.Core.SettingsManager.GetMasterSettings(false).EnableAgentProductRange)
                    productRanage = ProductInfo.ProductRanage.RangeSelect;
                else
                    productRanage = ProductInfo.ProductRanage.NormalSelect;
            }
            else
                productRanage = ProductInfo.ProductRanage.All;
            return productRanage;
        }

        /// <summary>
        /// 累加分销商店铺访问量,触发一次访问量+1
        /// </summary>
        /// <param name="distributorId">分销商id</param>
        public static void UpdateDistributorVisitCount(int memberId, int distributorId)
        {
            new DistributorsDao().UpdateDistributorVisitCount(memberId, distributorId);
        }
        /// <summary>
        /// 获取分销商店铺访问总数
        /// </summary>
        /// <param name="distributorId"></param>
        /// <returns></returns>
        public static int GetDistributorVisitCount(int distributorId, string date = "", int memberId = -1)
        {
            return new DistributorsDao().GetDistributorVisitCount(distributorId, date, memberId).Rows[0]["visitCount"].ToInt();
        }

        /// <summary>
        /// 获取分销商的会员数
        /// </summary>
        public static int GetDistributorMemberCount(int distributorId, string date = "")
        {
            return new DistributorsDao().GetDistributorMemberCount(distributorId, date).Rows.Count;
        }

        public static DataTable GetAgentDistributorsVisitInfo(int agentId, string date = "")
        {
            return new DistributorsDao().GetAgentDistributorsVisitInfo(agentId, date);
        }
        public static int Updateaspnet_DistributorsUserId(int userid) 
        { 
            return new DistributorsDao().Updateaspnet_DistributorsUserId(userid);
        }
        public static int Updateaspnet_DistributorsServiceStoreId(int userid) 
        {
            return new DistributorsDao().Updateaspnet_DistributorsServiceStoreId(userid);
        }
        public static int Updateaspnet_DistributorsServiceToreId(int userid)
        {
            return new DistributorsDao().Updateaspnet_DistributorsServiceToreId(userid);
        }
        public static bool DeleteProduct(int CommodityID)
        {
            return new DistributorsDao().DeleteProduct(CommodityID);
        }
        public static bool DeleteChannel(Guid ChannelId)
        {
            return new DistributorsDao().DeleteChannel(ChannelId);
        }
        public static bool DeleteDistributor(int userId)
        {
            //如果该分销商有下属,则不允许删除
            if (DistributorsBrower.GetDownDistributorNum(userId.ToString()) > 0)
            {
                return false;
            }
            bool flag = new DistributorsDao().DeleteDistributor(userId);
            if (flag)
            {
                HiCache.Remove(string.Format("DataCache-Member-{0}", userId));
                HiCache.Remove(string.Format("DataCache-Distributor-{0}", userId));
            }
            return flag;
        }

        public static DataTable Export(DataTable OldDt)
        {
            DataTable dt = OldDt.Clone();
            foreach (DataRow dr in OldDt.Rows)
            {
                dt.Rows.Add(dr.ItemArray);
                GetDownDistributorExport(dt,Convert.ToInt32(dr["UserID"]));
            }
            return dt;
        }
        public static void GetDownDistributorExport(DataTable dt,int UserID)
        {
            DataTable ChiDt = GetDownDis(UserID);
            foreach(DataRow dr in ChiDt.Rows){
                dt.Rows.Add(dr.ItemArray);
                GetDownDistributorExport(dt, Convert.ToInt32(dr["UserID"]));
            }
        }

        /// <summary>
        /// 根据子账号id获取前台绑定的分销商id(目前仅用于爽爽挝啡多店铺子账号管理)
        /// </summary>
        public static int GetSenderDistributorId(string sender)
        {
            return new DistributorsDao().GetSenderDistributorId(sender);
        }
    }
}

