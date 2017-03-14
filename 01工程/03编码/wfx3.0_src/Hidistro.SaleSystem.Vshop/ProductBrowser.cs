namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.SqlDal.Comments;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Members;
    using Hidistro.SqlDal.Promotions;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public static class ProductBrowser
    {
        public static bool AddProductToFavorite(int productId, int userId)
        {
            FavoriteDao dao = new FavoriteDao();
            return (dao.ExistsProduct(productId, userId) || dao.AddProductToFavorite(productId, userId));
        }

        public static DbQueryResult GetOnlineGifts(GiftQuery page)
        {
            page.IsOnline = true;
            return new GiftDao().GetGifts(page);
        }

        public static DbQueryResult GetGifts(GiftQuery query)
        {
            return new GiftDao().GetGifts(query);
        }

        public static IList<GiftInfo> GetGifts(int maxnum)
        {
            return new GiftDao().GetGifts(maxnum);
        }

        public static GiftInfo GetGiftDetails(int giftid)
        {
            return new GiftDao().GetGiftDetails(giftid);
        }

        public static bool CheckHasCollect(int memberId, int productId)
        {
            return new FavoriteDao().CheckHasCollect(memberId, productId);
        }

        public static int DeleteFavorite(int favoriteId)
        {
            return new FavoriteDao().DeleteFavorite(favoriteId);
        }

        public static bool DeleteFavorites(string ids)
        {
            return new FavoriteDao().DeleteFavorites(ids);
        }

        public static bool ExistsProduct(int productId, int userId)
        {
            return new FavoriteDao().ExistsProduct(productId, userId);
        }

        public static DataTable GetActiviOne(int ActivitiesType, decimal MeetMoney)
        {
            return new ProductBrowseDao().GetActiviOne(ActivitiesType, MeetMoney);
        }

        public static DataTable GetActivitie(int ActivitiesId)
        {
            return new ProductBrowseDao().GetActivitie(ActivitiesId);
        }

        public static DataTable GetAllFull()
        {
            return new HomeProductDao().GetAllFull();
        }

        public static DataTable GetAllFull(int ActivitiesType)
        {
            return new ProductBrowseDao().GetAllFull(ActivitiesType);
        }

        public static DataTable GetBrandProducts(MemberInfo member, int? brandId, int pageNumber, int maxNum, out int total)
        {
            return new ProductBrowseDao().GetBrandProducts(member, brandId, pageNumber, maxNum, out total);
        }

        public static DataTable GetExpandAttributes(int productId)
        {
            return new SkuDao().GetExpandAttributes(productId);
        }

        public static DataTable GetFavorites(MemberInfo member)
        {
            return new FavoriteDao().GetFavorites(member);
        }

        public static DbQueryResult GetHomeProduct(MemberInfo member, ProductQuery query)
        {
            /*
            DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors();
            if ((currentDistributors != null) && (currentDistributors.UserId != 0))
            {
                return new HomeProductDao().GetHomeProductsEx(member, query, true);
            }
            return new HomeProductDao().GetHomeProductsEx(member, query, false);
            */
            return new HomeProductDao().GetHomeProductsEx(member, query, DistributorsBrower.GetCurrStoreProductRange());
        }

        /// <summary>
        /// 产品Top显示 数据集
        /// </summary>
        public static DataTable GetHomeProductTop(int top, ProductInfo.ProductTop productTop)
        {
            DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors();

                if (productTop == ProductInfo.ProductTop.MostLike)//如果是猜你喜欢的产品
                {
                    MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                    if (currentMember != null)
                    {
                        DataTable favPros = ProductBrowser.GetFavorites(currentMember);
                        if (favPros.Rows.Count > 0)
                        {
                            return favPros;
                        }
                    }
                }
                return new HomeProductDao().GetHomeProductTop(top, productTop, DistributorsBrower.GetCurrStoreProductRange());
        }

        /// <summary>
        /// 产品Top显示 新增重载,根据产品id筛选
        /// </summary>
        public static DataTable GetHomeProductTop(string top, ProductInfo.ProductTop productTop, int CategoryId)
        {
            DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors();

                if (productTop == ProductInfo.ProductTop.MostLike)//如果是猜你喜欢的产品
                {
                    MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                    if (currentMember != null)
                    {
                        DataTable favPros = ProductBrowser.GetFavorites(currentMember);
                        if (favPros.Rows.Count > 0)
                        {
                            return favPros;
                        }
                    }
                }
            return new HomeProductDao().GetHomeProductTop(top, productTop, DistributorsBrower.GetCurrStoreProductRange(), CategoryId);
        }

        public static ProductInfo GetProduct(MemberInfo member, int productId)
        {
            return new ProductBrowseDao().GetProduct(member, productId);
        }

        public static DataTable GetProductCategories(int prouctId)
        {
            return new ProductDao().GetProductCategories(prouctId);
        }

        public static DbQueryResult GetProductConsultations(ProductConsultationAndReplyQuery consultationQuery)
        {
            return new ProductConsultationDao().GetConsultationProducts(consultationQuery);
        }

        public static int GetProductConsultationsCount(int productId, bool includeUnReplied)
        {
            return new ProductConsultationDao().GetProductConsultationsCount(productId, includeUnReplied);
        }

        public static DbQueryResult GetProductReviews(ProductReviewQuery reviewQuery)
        {
            return new ProductReviewDao().GetProductReviews(reviewQuery);
        }

        public static int GetProductReviewsCount(int productId)
        {
            return new ProductReviewDao().GetProductReviewsCount(productId);
        }

        public static DataTable GetProducts(MemberInfo member, int? topicId, int BrandId, int? categoryId, string keyWord, int pageNumber, int maxNum, out int total, string sort, string order, string swr = "", int rangeType = 0,int storeid = 0)
        {
            ProductInfo.ProductRanage productRanage = new ProductInfo.ProductRanage();
            if (!Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.AnonymousOrder)
                productRanage = DistributorsBrower.GetCurrStoreProductRange();
            else//如果匿名点餐功能开启,则获取所有商品
                productRanage = ProductInfo.ProductRanage.All;
            return new ProductBrowseDao().GetProductsRange(member, topicId, BrandId, categoryId, Globals.GetCurrentDistributorId(), keyWord, pageNumber, maxNum, out total, sort, order == "asc", productRanage, swr, rangeType,storeid);
        }
        /// <summary>
        /// 通过显示端获取第一个分类id
        /// </summary>
        public static int GetFirstCategoryId(int rangeType)
        {
            return new CategoryDao().GetFirstCategoryId(rangeType);
        }

        /// <summary>
        /// up by:JHB 150916 店铺上架商品，根据配置来限定范围
        /// type:0正常显示店铺已上架的商品，1正常显示店铺未上架的商品，2显示所有出售状态的商品，3根据上架范围显示已上架的商品，4根据上架范围显示未上架的商品
        /// </summary>
        public static DataTable GetProductsEx(MemberInfo member, int? topicId, int? categoryId, string keyWord, int pageNumber, int maxNum, out int total, string sort, string order, ProductInfo.ProductRanage productRanage)
        {
            DataTable table = new DataTable();
            int toal = 0;
            int currentDistributorId = Globals.GetCurrentDistributorId();
            if (currentDistributorId > 0)
            {
                table = new ProductBrowseDao().GetProductsEx(member, topicId, categoryId, currentDistributorId, keyWord, pageNumber, maxNum, out toal, sort, order == "asc", productRanage);
            }
            total = toal;
            return table;
        }

        public static DataTable GetSkus(int productId)
        {
            return new SkuDao().GetSkus(productId);
        }

        public static DataTable GetTopicProducts(MemberInfo member, int topicid, int maxNum)
        {
            return new ProductBrowseDao().GetTopicProducts(member, topicid, maxNum);
        }

        public new static DataTable GetType()
        {
            return new ProductBrowseDao().GetType();
        }

        public static bool InsertProductConsultation(ProductConsultationInfo productConsultation)
        {
            return new ProductConsultationDao().InsertProductConsultation(productConsultation);
        }

        public static bool InsertProductReview(ProductReviewInfo review)
        {
            return new ProductReviewDao().InsertProductReview(review);
        }

        public static void LoadProductReview(int productId, int userId, out int buyNum, out int reviewNum)
        {
            new ProductReviewDao().LoadProductReview(productId, userId, out buyNum, out reviewNum);
        }

        public static int UpdateFavorite(int favoriteId, string tags, string remark)
        {
            return new FavoriteDao().UpdateFavorite(favoriteId, tags, remark);
        }

        public static bool UpdateVisitCounts(int productId)
        {
            return new ProductBatchDao().UpdateVisitCounts(productId);
        }
        /// <summary>
        /// 查看用户尚未被回复的信息条数
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public static int FindUserNoReply(int UserID,int ProductID)
        {
            return new ProductConsultationDao().FindUserNoReply(UserID, ProductID);
        }

        /// <summary>
        /// 得到所有商品规格信息  -使用在代理商采购
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSkuItems()
        {
            return new SkuDao().GetSkuItems();
        }

        /// <summary>
        /// 根据Skuid及用户Id（代理商）删除代理商订购列表
        /// </summary>
        /// <param name="skuids">skuId集合以'',''分割</param>
        /// <param name="userId">用户Id;后台用户Id</param>
        /// <returns></returns>
        public static int DeleteAgentProduct(string skuids, int userId)
        {
            return new SkuDao().DeleteAgentProduct(skuids, userId);
        }
        /// <summary>
        /// 获取正在限时抢购商品列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DbQueryResult GetCountDownProductList(ProductBrowseQuery query)
        {
            return new CountDownDao().GetCountDownProductList(query);
        }

        public static CountDownInfo GetCountDownInfo(int productId)
        {
            return new CountDownDao().GetCountDownByProductId(productId);
        }

        public static CountDownInfo GetCountDownInfoByCountDownId(int countDownId)
        {
            return new CountDownDao().GetCountDownInfo(countDownId);
        }

        public static CountDownInfo GetCountDownInfo(int countDownID, int buyAmount, out string msg)
        {
            msg = "";
            CountDownInfo countDownInfo = new CountDownDao().GetCountDownInfo(countDownID);
            if (countDownInfo == null)
            {
                msg = "抢购信息不存在！";
                return null;
            }
            if (((countDownInfo.MaxCount < buyAmount) || (countDownInfo.StartDate > DateTime.Now)) || (countDownInfo.EndDate < DateTime.Now))
            {
                msg = "抢购还没有开始或者已经结束,或者数量超过了抢购数量";
                return null;
            }
            return countDownInfo;
        }
        /*
        public static ProductBrowseInfo GetProductBrowseInfo(int productId, int? maxReviewNum, int? maxConsultationNum, int storeStockValidateType, bool MutiStores = false)
        {
            return new ProductBrowseDao().GetProductBrowseInfo(productId, maxReviewNum, maxConsultationNum, storeStockValidateType, MutiStores);
        }
        */

        public static DataTable GetGroupBuyProducts(int? categoryId, string keywords, int page, int size, out int total, bool onlyUnFinished = true)
        {
            return new GroupBuyDao().GetGroupBuyProducts(categoryId, keywords, page, size, out total, onlyUnFinished);
        }

        public static GroupBuyInfo GetProductGroupBuyInfo(int productId)
        {
            return new GroupBuyDao().GetProductGroupBuyInfo(productId);
        }

        public static GroupBuyInfo GetGroupBuy(int groupbuyId)
        {
            return new GroupBuyDao().GetGroupBuy(groupbuyId);
        }

        public static DataTable GetCutDownProducts(int? categoryId, string keywords, int page, int size, out int total, bool onlyUnFinished = true)
        {
            return new CutDownDao().GetCutDownProducts(categoryId, keywords, page, size, out total, onlyUnFinished);
        }

        public static string GetProductType(int productId)
        {
            return new ProductDao().GetProductType(productId);
        }

        public static ProductBrowseInfo GetProductBrowseInfo(int productId, int? maxReviewNum, int? maxConsultationNum, int storeStockValidateType, bool MutiStores = false)
        {
            return new ProductBrowseDao().GetProductBrowseInfo(productId, maxReviewNum, maxConsultationNum, storeStockValidateType, MutiStores);
        }

        /// <summary>
        /// 根据分类id获取商品实体类list
        /// </summary>
        public static IList<ProductInfo> GetProductList(int categoryId)
        {
            return new ProductBrowseDao().GetProductList(categoryId);
        }


        public static int GetDataCount(string from, string where)
        {
            return new ProductBrowseDao().SelectDataCount(from, where);
        }

        public static DataTable GetPageData(string tablename, string orderFields, string selectFields, int currPage, int pagesize, string where)
        {
            return new ProductBrowseDao().SelectPageData(tablename, orderFields, selectFields, currPage, pagesize, where);
        }

        public static DataTable getSkusByWhere(string where)
        {
            return new ProductBrowseDao().getSkusByWhere(where);
        }
    }
}

