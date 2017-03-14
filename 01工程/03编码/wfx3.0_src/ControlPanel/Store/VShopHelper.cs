namespace Hidistro.ControlPanel.Store
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.VShop;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Members;
    using Hidistro.SqlDal.VShop;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Web;

    public static class VShopHelper
    {
        private const string CacheKey = "Message-{0}";

        public static int AddActivities(ActivitiesInfo activity)
        {
            return new ActivitiesDao().AddActivities(activity);
        }
        public static bool CreateImport(ImportOfProductsQuery dy)
        {
            return new DistributorsDao().CreateImport(dy);
        }
        public static bool AddHomeProdcut(int productId)
        {
            return new HomeProductDao().AddHomeProdcut(productId);
        }

        public static bool AddHomeTopic(int topicId)
        {
            return new HomeTopicDao().AddHomeTopic(topicId);
        }
        public static ImportOfProductsQuery GetHishop_PruductsListCommodityCode(string CommodityCode)
        {
            return new DistributorsDao().GetHishop_PruductsListCommodityCode(CommodityCode);
        }
        public static bool AddReleatesProdcutBytopicid(int topicid, int productId)
        {
            return new TopicDao().AddReleatesProdcutBytopicid(topicid, productId);
        }

        public static bool CanAddMenu(int parentId, ClientType clientType)
        {
            IList<MenuInfo> menusByParentId = new MenuDao().GetMenusByParentId(parentId, clientType);
            if ((menusByParentId == null) || (menusByParentId.Count == 0))
            {
                return true;
            }
            if (parentId == 0)
            {
                return (menusByParentId.Count < 3);
            }
            return (menusByParentId.Count < 5);
        }

        public static bool Createtopic(TopicInfo topic, out int id)
        {
            id = 0;
            if (topic == null)
            {
                return false;
            }
            Globals.EntityCoding(topic, true);
            id = new TopicDao().AddTopic(topic);
            ReplyInfo reply = new TextReplyInfo {
                Keys = topic.Keys,
                MatchType = MatchType.Equal,
                MessageType = MessageType.Text,
                ReplyType = ReplyType.Topic,
                ActivityId = id
            };
            return new ReplyDao().SaveReply(reply);
        }

        public static bool DeleteActivities(int ActivitiesId)
        {
            return new ActivitiesDao().DeleteActivities(ActivitiesId);
        }

        public static bool DeleteActivity(int activityId)
        {
            return new ActivityDao().DeleteActivity(activityId);
        }

        public static bool DeleteAlarm(int id)
        {
            return new AlarmDao().Delete(id);
        }

        public static bool DeleteFeedBack(int id)
        {
            return new FeedBackDao().Delete(id);
        }

        public static bool DeleteLotteryActivity(int activityid, string type = "")
        {
            return new LotteryActivityDao().DelteLotteryActivity(activityid, type);
        }

        public static bool DeleteMenu(int menuId)
        {
            return new MenuDao().DeleteMenu(menuId);
        }

        public static bool Deletetopic(int topicId)
        {
            return new TopicDao().DeleteTopic(topicId);
        }

        public static int Deletetopics(IList<int> topics)
        {
            if ((topics != null) && (topics.Count != 0))
            {
                return new TopicDao().DeleteTopics(topics);
            }
            return 0;
        }

        public static bool DelteLotteryTicket(int activityId)
        {
            return new LotteryActivityDao().DelteLotteryTicket(activityId);
        }

        public static bool DelTplCfg(int id)
        {
            return new BannerDao().DelTplCfg(id);
        }

        /// <summary>
        /// 删除大图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DelImgCfg(int id)
        {
            return new BannerDao().DelImgCfg(id);
        }

        public static IList<ActivitiesInfo> GetActivitiesInfo(string ActivitiesId)
        {
            return new ActivitiesDao().GetActivitiesInfo(ActivitiesId);
        }

        public static DbQueryResult GetActivitiesList(ActivitiesQuery query)
        {
            return new ActivitiesDao().GetActivitiesList(query);
        }

        public static ActivityInfo GetActivity(int activityId)
        {
            return new ActivityDao().GetActivity(activityId);
        }

        public static IList<ActivitySignUpInfo> GetActivitySignUpById(int activityId)
        {
            return new ActivitySignUpDao().GetActivitySignUpById(activityId);
        }

        public static DbQueryResult GetAlarms(int pageIndex, int pageSize)
        {
            return new AlarmDao().List(pageIndex, pageSize);
        }

        public static IList<ActivityInfo> GetAllActivity()
        {
            return new ActivityDao().GetAllActivity();
        }

        public static IList<BannerInfo> GetAllBanners()
        {
            return new BannerDao().GetAllBanners();
        }

        /// <summary>
        /// 获取所有大图
        /// </summary>
        /// <returns></returns>
        public static IList<BannerInfo> GetAllImgBanners()
        {
            return new BannerDao().GetAllImgBanners();
        }

        public static IList<NavigateInfo> GetAllNavigate()
        {
            return new BannerDao().GetAllNavigate();
        }

        public static DbQueryResult GetBalanceDrawRequest(BalanceDrawRequestQuery query)
        {
            return new DistributorsDao().GetBalanceDrawRequest(query);
        }

        public static DbQueryResult GetCommissions(CommissionsQuery query)
        {
            return new DistributorsDao().GetCommissions(query);
        }

        public static int GetCountBanner()
        {
            return new BannerDao().GetCountBanner();
        }

        public static IList<DistributorGradeInfo> GetDistributorGradeInfos()
        {
            return new DistributorGradeDao().GetDistributorGradeInfos();
        }
        public static IList<AgentGradeInfo> GetAgentGradeInfos()
        {
            return new AgentGradeDao().GetAgentGradeInfos();
        }

        public static DbQueryResult GetDistributors(DistributorsQuery query)
        {
            return new DistributorsDao().GetDistributors(query);
        }
        public static DbQueryResult GetImportProducts(ImportOfProductsQuery query)
        {
            return new DistributorsDao().GetImportProducts(query);
        }
        public static int GetDownDistributorNum(string userid)
        {
            return new DistributorsDao().GetDownDistributorNum(userid);
        }

        public static int GetDownDistributorNumReferralOrders(string userid)
        {
            return new DistributorsDao().GetDownDistributorNumReferralOrders(userid);
        }

        public static FeedBackInfo GetFeedBack(int id)
        {
            return new FeedBackDao().Get(id);
        }

        public static FeedBackInfo GetFeedBack(string feedBackID)
        {
            return new FeedBackDao().Get(feedBackID);
        }

        public static DbQueryResult GetFeedBacks(int pageIndex, int pageSize, string msgType)
        {
            return new FeedBackDao().List(pageIndex, pageSize, msgType);
        }

        public static DataTable GetHomeProducts()
        {
            return new HomeProductDao().GetHomeProducts();
        }

        public static DataTable GetHomeTopics()
        {
            return new HomeTopicDao().GetHomeTopics();
        }

        public static IList<MenuInfo> GetInitMenus(ClientType clientType)
        {
            MenuDao dao = new MenuDao();
            IList<MenuInfo> topMenus = dao.GetTopMenus(clientType);
            foreach (MenuInfo info in topMenus)
            {
                info.Chilren = dao.GetMenusByParentId(info.MenuId, clientType);
                if (info.Chilren == null)
                {
                    info.Chilren = new List<MenuInfo>();
                }
            }
            return topMenus;
        }

        public static IList<LotteryActivityInfo> GetLotteryActivityByType(LotteryActivityType type)
        {
            return new LotteryActivityDao().GetLotteryActivityByType(type);
        }

        public static LotteryActivityInfo GetLotteryActivityInfo(int activityid)
        {
            LotteryActivityInfo lotteryActivityInfo = new LotteryActivityDao().GetLotteryActivityInfo(activityid);
            lotteryActivityInfo.PrizeSettingList = JsonConvert.DeserializeObject<List<PrizeSetting>>(lotteryActivityInfo.PrizeSetting);
            return lotteryActivityInfo;
        }

        public static DbQueryResult GetLotteryActivityList(LotteryActivityQuery page)
        {
            return new LotteryActivityDao().GetLotteryActivityList(page);
        }

        public static DbQueryResult GetWKMList(WhoKnowMeQuery page)
        {
            return new LotteryActivityDao().GetWKMList(page);
        }

        public static LotteryTicketInfo GetLotteryTicket(int activityid)
        {
            LotteryTicketInfo lotteryTicket = new LotteryActivityDao().GetLotteryTicket(activityid);
            lotteryTicket.PrizeSettingList = JsonConvert.DeserializeObject<List<PrizeSetting>>(lotteryTicket.PrizeSetting);
            return lotteryTicket;
        }

        public static DbQueryResult GetLotteryTicketList(LotteryActivityQuery page)
        {
            return new LotteryActivityDao().GetLotteryTicketList(page);
        }

        public static MenuInfo GetMenu(int menuId)
        {
            return new MenuDao().GetMenu(menuId);
        }

        public static IList<MenuInfo> GetMenus(ClientType clientType)
        {
            IList<MenuInfo> list = new List<MenuInfo>();
            MenuDao dao = new MenuDao();
            IList<MenuInfo> topMenus = dao.GetTopMenus(clientType);
            if (topMenus != null)
            {
                foreach (MenuInfo info in topMenus)
                {
                    list.Add(info);
                    IList<MenuInfo> menusByParentId = dao.GetMenusByParentId(info.MenuId, clientType);
                    if (menusByParentId != null)
                    {
                        foreach (MenuInfo info2 in menusByParentId)
                        {
                            list.Add(info2);
                        }
                    }
                }
            }
            return list;
        }

        public static IList<MenuInfo> GetMenusByParentId(int parentId,ClientType clientType)
        {
            return new MenuDao().GetMenusByParentId(parentId, clientType);
        }

        public static MessageTemplate GetMessageTemplate(string messageType)
        {
            if (string.IsNullOrEmpty(messageType))
            {
                return null;
            }
            return new MessageTemplateHelperDao().GetMessageTemplate(messageType);
        }

        public static IList<MessageTemplate> GetMessageTemplates()
        {
            return new MessageTemplateHelperDao().GetMessageTemplates();
        }

        public static List<PrizeRecordInfo> GetPrizeList(PrizeQuery page)
        {
            return new LotteryActivityDao().GetPrizeList(page);
        }

        public static List<PrizeRecordInfo> GetPrizeListEx(PrizeQuery page)
        {
            return new LotteryActivityDao().GetPrizeListEx(page);
        }

        public static DataTable GetRelatedTopicProducts(int topicid)
        {
            return new TopicDao().GetRelatedTopicProducts(topicid);
        }

        public static TopicInfo Gettopic(int topicId)
        {
            return new TopicDao().GetTopic(topicId);
        }

        public static DbQueryResult GettopicList(TopicQuery page)
        {
            return new TopicDao().GetTopicList(page);
        }

        public static IList<TopicInfo> Gettopics()
        {
            return new TopicDao().GetTopics();
        }

        public static IList<MenuInfo> GetTopMenus(ClientType clientType)
        {
            return new MenuDao().GetTopMenus(clientType);
        }

        public static TplCfgInfo GetTplCfgById(int id)
        {
            return new BannerDao().GetTplCfgById(id);
        }

        /// <summary>
        /// 获取大图信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TplCfgInfo GetImgCfgById(int id)
        {
            return new BannerDao().GetImgCfgById(id);
        }

        public static DataTable GetType(int Types)
        {
            return new ActivitiesDao().GetType(Types);
        }

        public static DistributorsInfo GetUserIdDistributors(int userid)
        {
            return new DistributorsDao().GetDistributorInfo(userid);
        }

        public static int InsertLotteryActivity(LotteryActivityInfo info)
        {
            string str = JsonConvert.SerializeObject(info.PrizeSettingList);
            info.PrizeSetting = str;
            return new LotteryActivityDao().InsertLotteryActivity(info);
        }

        public static bool RemoveAllHomeProduct()
        {
            return new HomeProductDao().RemoveAllHomeProduct();
        }

        public static bool RemoveAllHomeTopics()
        {
            return new HomeTopicDao().RemoveAllHomeTopics();
        }

        public static bool RemoveHomeProduct(int productId)
        {
            return new HomeProductDao().RemoveHomeProduct(productId);
        }

        public static bool RemoveHomeTopic(int TopicId)
        {
            return new HomeTopicDao().RemoveHomeTopic(TopicId);
        }

        public static bool RemoveReleatesProductBytopicid(int topicid)
        {
            return new TopicDao().RemoveReleatesProductBytopicid(topicid);
        }

        public static bool RemoveReleatesProductBytopicid(int topicid, int productId)
        {
            return new TopicDao().RemoveReleatesProductBytopicid(topicid, productId);
        }

        public static bool SaveActivity(ActivityInfo activity)
        {
            int num = new ActivityDao().SaveActivity(activity);
            ReplyInfo reply = new TextReplyInfo {
                Keys = activity.Keys,
                MatchType = MatchType.Equal,
                MessageType = MessageType.Text,
                ReplyType = ReplyType.SignUp,
                ActivityId = num
            };
            return new ReplyDao().SaveReply(reply);
        }

        public static bool SaveAlarm(AlarmInfo info)
        {
            return new AlarmDao().Save(info);
        }

        public static bool SaveFeedBack(FeedBackInfo info)
        {
            return new FeedBackDao().Save(info);
        }

        public static int SaveLotteryTicket(LotteryTicketInfo info)
        {
            string str = JsonConvert.SerializeObject(info.PrizeSettingList);
            info.PrizeSetting = str;
            return new LotteryActivityDao().SaveLotteryTicket(info);
        }

        public static bool SaveMenu(MenuInfo menu)
        {
            return new MenuDao().SaveMenu(menu);
        }

        public static bool SaveTplCfg(TplCfgInfo info)
        {
            return new BannerDao().SaveTplCfg(info);
        }

        /// <summary>
        /// 保存大图
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool SaveImgCfg(TplCfgInfo info)
        {
            return new BannerDao().SaveImgCfg(info);
        }

        public static void SwapMenuSequence(int menuId, bool isUp)
        {
            new MenuDao().SwapMenuSequence(menuId, isUp);
        }

        public static bool SwapTopicSequence(int topicid, int displaysequence)
        {
            return new TopicDao().SwapTopicSequence(topicid, displaysequence);
        }

        public static void SwapTplCfgSequence(int bannerId, int replaceBannerId)
        {
            BannerDao dao = new BannerDao();
            TplCfgInfo tplCfgById = dao.GetTplCfgById(bannerId);
            TplCfgInfo info = dao.GetTplCfgById(replaceBannerId);
            if ((tplCfgById != null) && (info != null))
            {
                int displaySequence = tplCfgById.DisplaySequence;
                tplCfgById.DisplaySequence = info.DisplaySequence;
                info.DisplaySequence = displaySequence;
                dao.UpdateTplCfg(tplCfgById);
                dao.UpdateTplCfg(info);
            }
        }

        public static bool UpdateActivities(ActivitiesInfo activity)
        {
            return new ActivitiesDao().UpdateActivities(activity);
        }

        public static bool UpdateActivity(ActivityInfo activity)
        {
            return new ActivityDao().UpdateActivity(activity);
        }

        public static bool UpdateBalanceDistributors(int UserId, decimal ReferralRequestBalance)
        {
            return new DistributorsDao().UpdateBalanceDistributors(UserId, ReferralRequestBalance);
        }

        public static bool UpdateBalanceDrawRequest(int Id, string Remark)
        {
            HiCache.Remove(string.Format("DataCache-Distributor-{0}", Id));
            return new DistributorsDao().UpdateBalanceDrawRequest(Id, Remark);
        }

        public static bool UpdateFeedBackMsgType(string feedBackId, string msgType)
        {
            return new FeedBackDao().UpdateMsgType(feedBackId, msgType);
        }

        public static bool UpdateHomeProductSequence(int ProductId, int displaysequence)
        {
            return new HomeProductDao().UpdateHomeProductSequence(ProductId, displaysequence);
        }

        public static bool UpdateHomeTopicSequence(int TopicId, int displaysequence)
        {
            return new HomeTopicDao().UpdateHomeTopicSequence(TopicId, displaysequence);
        }

        public static bool UpdateLotteryActivity(LotteryActivityInfo info)
        {
            string str = JsonConvert.SerializeObject(info.PrizeSettingList);
            info.PrizeSetting = str;
            return new LotteryActivityDao().UpdateLotteryActivity(info);
        }

        public static bool UpdateLotteryTicket(LotteryTicketInfo info)
        {
            string str = JsonConvert.SerializeObject(info.PrizeSettingList);
            info.PrizeSetting = str;
            return new LotteryActivityDao().UpdateLotteryTicket(info);
        }

        public static bool UpdateMenu(MenuInfo menu)
        {
            return new MenuDao().UpdateMenu(menu);
        }

        public static bool UpdateRelateProductSequence(int TopicId, int RelatedProductId, int displaysequence)
        {
            return new TopicDao().UpdateRelateProductSequence(TopicId, RelatedProductId, displaysequence);
        }

        public static void UpdateSettings(IList<MessageTemplate> templates)
        {
            if ((templates != null) && (templates.Count != 0))
            {
                new MessageTemplateHelperDao().UpdateSettings(templates);
                foreach (MessageTemplate template in templates)
                {
                    HiCache.Remove(string.Format("Message-{0}", template.MessageType.ToLower()));
                }
            }
        }

        public static void UpdateTemplate(MessageTemplate template)
        {
            if (template != null)
            {
                new MessageTemplateHelperDao().UpdateTemplate(template);
                HiCache.Remove(string.Format("Message-{0}", template.MessageType.ToLower()));
            }
        }

        public static bool Updatetopic(TopicInfo topic)
        {
            if (topic == null)
            {
                return false;
            }
            Globals.EntityCoding(topic, true);
            return new TopicDao().UpdateTopic(topic);
        }

        public static bool UpdateTplCfg(TplCfgInfo info)
        {
            return new BannerDao().UpdateTplCfg(info);
        }

        /// <summary>
        /// 更新大图信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool UpdateImgCfg(TplCfgInfo info)
        {
            return new BannerDao().UpdateImgCfg(info);
        }

        public static string UploadDefautBg(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
            {
                return string.Empty;
            }
            string str = Globals.GetVshopSkinPath(null) + "/images/ad/DefautPageBg" + Path.GetExtension(postedFile.FileName);
            postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }

        public static string UploadTopicImage(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
            {
                return string.Empty;
            }
            string str = Globals.GetStoragePath() + "/topic/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
            postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }

        public static string UploadVipBGImage(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
            {
                return string.Empty;
            }
            string str = Globals.GetStoragePath() + "/Vipcard/vipbg" + Path.GetExtension(postedFile.FileName);
            postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }

        public static string UploadVipQRImage(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
            {
                return string.Empty;
            }
            string str = Globals.GetStoragePath() + "/Vipcard/vipqr" + Path.GetExtension(postedFile.FileName);
            postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }

        public static string UploadWeiXinCodeImage(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
            {
                return string.Empty;
            }
            string str = Globals.GetStoragePath() + "/WeiXinCodeImageUrl" + Path.GetExtension(postedFile.FileName);
            postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }

        

    }
}

