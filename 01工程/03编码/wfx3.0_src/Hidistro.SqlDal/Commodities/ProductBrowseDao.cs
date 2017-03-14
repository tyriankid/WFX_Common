namespace Hidistro.SqlDal.Commodities
{
    using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.SqlDal.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

    public class ProductBrowseDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public DataTable GetActiviOne(int ActivitiesType, decimal MeetMoney)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Concat(new object[] { "select top 1 ReductionMoney,ActivitiesId,ActivitiesName,MeetMoney,ActivitiesType from Hishop_Activities where datediff(dd,GETDATE(),StartTime)<=0 and datediff(dd,GETDATE(),EndTIme)>=0 and (ActivitiesType=0 or ActivitiesType=", ActivitiesType, ") and MeetMoney<=", MeetMoney, "  order by MeetMoney desc" }));
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetActivitie(int ActivitiesId)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ReductionMoney,ActivitiesId,ActivitiesName,MeetMoney,ActivitiesType,ActivitiesDescription from Hishop_Activities where datediff(dd,GETDATE(),StartTime)<=0 and datediff(dd,GETDATE(),EndTIme)>=0  and ActivitiesId=" + ActivitiesId + "  order by MeetMoney asc");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetAllFull(int ActivitiesType)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ReductionMoney,ActivitiesId,ActivitiesName,MeetMoney,ActivitiesType,ActivitiesDescription from Hishop_Activities where datediff(dd,GETDATE(),StartTime)<=0 and datediff(dd,GETDATE(),EndTIme)>=0 and (ActivitiesType=0 or ActivitiesType=" + ActivitiesType + ")  order by MeetMoney asc");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetBrandProducts(MemberInfo member, int? brandId, int pageNumber, int maxNum, out int total)
        {
            int discount = 100;
            StringBuilder builder = new StringBuilder();
            builder.Append("ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,ShowSaleCounts,");
            builder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,");
            if (member != null)
            {
                discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", member.GradeId);
                builder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
            }
            else
            {
                builder.Append("SalePrice");
            }
            StringBuilder builder2 = new StringBuilder();
            builder2.Append(" SaleStatus=1");
            if (brandId.HasValue)
            {
                builder2.AppendFormat(" AND BrandId = {0}", brandId);
            }
            DbQueryResult result = DataHelper.PagingByRownumber(pageNumber, maxNum, "DisplaySequence", SortAction.Desc, true, "vw_Hishop_BrowseProductList s", "ProductId", builder2.ToString(), builder.ToString());
            DataTable data = (DataTable)result.Data;
            total = result.TotalRecords;
            return data;
        }

        public ProductInfo GetProduct(MemberInfo member, int productId)
        {
            int discount = 100;
            StringBuilder builder = new StringBuilder();
            if (member != null)
            {
                discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", member.GradeId);
                builder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
            }
            else
            {
                builder.Append("SalePrice,CostPrice");
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Products WHERE ProductId =@ProductId;SELECT SkuId, ProductId, SKU,Weight, Stock, CostPrice, " + builder.ToString() + " FROM Hishop_SKUs s WHERE ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            ProductInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateProduct(reader);
                }
                if (!reader.NextResult())
                {
                    return info;
                }
                while (reader.Read())
                {
                    info.Skus.Add((string)reader["SkuId"], DataMapper.PopulateSKU(reader));
                }
            }
            return info;
        }

        public DataTable GetProductsRange(MemberInfo member, int? topicId, int BrandId, int? categoryId, int distributorId, string keyWord, int pageNumber, int maxNum, out int toal, string sort, bool isAsc = false, ProductInfo.ProductRanage productRanage = ProductInfo.ProductRanage.NormalSelect, string swr = "", int rangeType = 0,int storeid=0)
        {
            int discount = 100;
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("ProductId,CategoryId,SkuId,Stock,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,BrandId,", maxNum);
            builder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,VistiCounts,");
            if (member != null && member.UserId!=0)
            {
                discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = vw_Hishop_BrowseProductList.SkuId AND GradeId = {0}) = 1", member.GradeId);
                builder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = vw_Hishop_BrowseProductList.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
            }
            else
            {
                builder.Append("SalePrice");
            }
            StringBuilder builder2 = new StringBuilder();
            builder2.Append(" SaleStatus=1");
            if (topicId.HasValue)
            {
                builder2.AppendFormat(" AND ProductId IN (SELECT RelatedProductId FROM Vshop_RelatedTopicProducts WHERE TopicId = {0})", topicId.Value);
            }
            if (BrandId>0)
            {
                builder2.AppendFormat(" AND (BrandId LIKE '%{0}%')", BrandId);
            }
            if (categoryId.HasValue)
            {
                CategoryInfo category = new CategoryDao().GetCategory(categoryId.Value);
                if (category != null)
                {
                    builder2.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%') ", category.Path);
                }
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                builder2.AppendFormat(" AND (ProductName LIKE '%{0}%')", keyWord);
            }
            if (!string.IsNullOrEmpty(swr))
            {
                builder2.AppendFormat(" AND {0}", swr);
            }
            if (rangeType != 0 )//爽爽挝啡特殊规则,判断商品是展示在pc端还是微信端
            {
                builder2.AppendFormat(" AND ([Range] in ({0},0) )", rangeType);
            }
            if(storeid !=0)//爽爽挝啡特殊规则,对门店进行相应的商品展示
            {
                builder2.AppendFormat(" AND storeid = {0}", storeid);
            }
            //只有审核通过的商品才能予以展示
            builder2.AppendFormat(" AND isnull( reviewState,0) = 0 ");
            switch (productRanage)
            {
                case ProductInfo.ProductRanage.NormalSelect: //正常显示店铺已上架的商品
                    builder2.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_DistributorProducts WHERE UserId={0})", distributorId);
                    break;
                case ProductInfo.ProductRanage.All: //显示所有出售状态的商品
                    break;
                case ProductInfo.ProductRanage.RangeSelect: //根据上架范围显示已上架的商品

                    //本身上架的记录
                    builder2.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_DistributorProducts WHERE UserId={0})", distributorId);

                    //代理取系统设置的上架范围，分销商取所属代理商的上架范围
                    int ofAgentID = distributorId;
                    DataTable dtDistributors = this.database.ExecuteDataSet(CommandType.Text,
                        string.Format("Select UserId,IsAgent,AgentPath From aspnet_Distributors Where UserId={0}", ofAgentID)).Tables[0];
                    if (dtDistributors.Rows[0]["IsAgent"].ToString() != "1" && dtDistributors.Rows[0]["AgentPath"] != DBNull.Value)
                        ofAgentID = int.Parse(dtDistributors.Rows[0]["AgentPath"].ToString().Split('|')[dtDistributors.Rows[0]["AgentPath"].ToString().Split('|').Length - 1]);
                    DataTable dtProductRange = this.database.ExecuteDataSet(CommandType.Text,
                        string.Format("Select * From Hishop_DistributorProductRange Where UserId={0}", ofAgentID)).Tables[0];
                    if (dtProductRange.Rows.Count == 0)
                    {
                        builder2.AppendFormat(" And 1=2 ");//未设置上架范围
                    }
                    else
                    {
                        DataRow drProductRange = dtProductRange.Rows[0];
                        builder2.AppendFormat(" AND (");
                        if (drProductRange["ProductRange1"] != DBNull.Value)
                            builder2.AppendFormat(string.Format("CategoryId in({0})", drProductRange["ProductRange1"].ToString()));
                        else
                            builder2.AppendFormat("CategoryId in(null)");
                        if (drProductRange["ProductRange2"] != DBNull.Value)
                            builder2.AppendFormat(string.Format(" or BrandId in({0})", drProductRange["ProductRange2"].ToString()));
                        else
                            builder2.AppendFormat(" or BrandId in(null)");
                        if (drProductRange["ProductRange3"] != DBNull.Value)
                            builder2.AppendFormat(string.Format(" or TypeId in({0})", drProductRange["ProductRange3"].ToString()));
                        else
                            builder2.AppendFormat(" or TypeId in(null)");
                        builder2.AppendFormat(")");
                    }
                    break;
            }
            if (string.IsNullOrWhiteSpace(sort))
            {
                sort = "ProductId";
            }
            DbQueryResult result = DataHelper.PagingByRownumber(pageNumber, maxNum, sort, isAsc ? SortAction.Asc : SortAction.Desc, true, "vw_Hishop_BrowseProductList", "ProductId", builder2.ToString(), builder.ToString());
            DataTable data = (DataTable)result.Data;
            toal = result.TotalRecords;
            return data;
        }

        /// <summary>
        /// add by:JHB 150916 店铺上架商品，根据配置来限定范围
        /// type:0正常显示店铺已上架的商品，1正常显示店铺未上架的商品，2显示所有出售状态的商品，3根据上架范围显示已上架的商品，4根据上架范围显示未上架的商品
        /// </summary>
        public DataTable GetProductsEx(MemberInfo member, int? topicId, int? categoryId, int distributorId, string keyWord, int pageNumber, int maxNum, out int toal, string sort, bool isAsc = false
            , ProductInfo.ProductRanage productRanage = ProductInfo.ProductRanage.NormalSelect)
        {
            int discount = 100;
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,", maxNum);
            builder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,VistiCounts,");
            if (member != null)
            {
                discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = vw_Hishop_BrowseProductList.SkuId AND GradeId = {0}) = 1", member.GradeId);
                builder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = vw_Hishop_BrowseProductList.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
            }
            else
            {
                builder.Append("SalePrice");
            }
            StringBuilder builder2 = new StringBuilder();
            builder2.Append(" SaleStatus=1");
            if (topicId.HasValue)
            {
                builder2.AppendFormat(" AND ProductId IN (SELECT RelatedProductId FROM Vshop_RelatedTopicProducts WHERE TopicId = {0})", topicId.Value);
            }
            if (categoryId.HasValue)
            {
                CategoryInfo category = new CategoryDao().GetCategory(categoryId.Value);
                if (category != null)
                {
                    builder2.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%') ", category.Path);
                }
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                builder2.AppendFormat(" AND (ProductName LIKE '%{0}%' OR ProductCode LIKE '%{0}%')", keyWord);
            }
            switch (productRanage)
            {
                case ProductInfo.ProductRanage.NormalSelect: //正常显示店铺已上架的商品
                    builder2.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_DistributorProducts WHERE UserId={0})", distributorId);
                    break;
                case ProductInfo.ProductRanage.NormalUnSelect: //正常显示店铺未上架的商品
                    builder2.AppendFormat(" AND ProductId NOT IN (SELECT ProductId FROM Hishop_DistributorProducts WHERE UserId={0})", distributorId);
                    break;
                case ProductInfo.ProductRanage.All: //显示所有出售状态的商品
                    break;
                case ProductInfo.ProductRanage.RangeSelect: //根据上架范围显示已上架的商品

                    //本身上架的记录
                    builder2.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_DistributorProducts WHERE UserId={0})", distributorId);

                    //代理取系统设置的上架范围，分销商取所属代理商的上架范围
                    int ofAgentID = distributorId;
                    DataTable dtDistributors = this.database.ExecuteDataSet(CommandType.Text,
                        string.Format("Select UserId,IsAgent,AgentPath From aspnet_Distributors Where UserId={0}", ofAgentID)).Tables[0];
                    if (dtDistributors.Rows[0]["IsAgent"].ToString() != "1" && dtDistributors.Rows[0]["AgentPath"] != DBNull.Value)
                        ofAgentID = int.Parse(dtDistributors.Rows[0]["AgentPath"].ToString().Split('|')[dtDistributors.Rows[0]["AgentPath"].ToString().Split('|').Length - 1]);
                    DataTable dtProductRange = this.database.ExecuteDataSet(CommandType.Text,
                        string.Format("Select * From Hishop_DistributorProductRange Where UserId={0}", ofAgentID)).Tables[0];
                    if (dtProductRange.Rows.Count == 0)
                    {
                        builder2.AppendFormat(" And 1=2 ");//未设置上架范围
                    }
                    else
                    {
                        DataRow drProductRange = dtProductRange.Rows[0];
                        builder2.AppendFormat(" AND (");
                        if (drProductRange["ProductRange1"] != DBNull.Value)
                            builder2.AppendFormat(string.Format("CategoryId in({0})", drProductRange["ProductRange1"].ToString()));
                        else
                            builder2.AppendFormat("CategoryId in(null)");
                        if (drProductRange["ProductRange2"] != DBNull.Value)
                            builder2.AppendFormat(string.Format(" or BrandId in({0})", drProductRange["ProductRange2"].ToString()));
                        else
                            builder2.AppendFormat(" or BrandId in(null)");
                        if (drProductRange["ProductRange3"] != DBNull.Value)
                            builder2.AppendFormat(string.Format(" or TypeId in({0})", drProductRange["ProductRange3"].ToString()));
                        else
                            builder2.AppendFormat(" or TypeId in(null)");
                        builder2.AppendFormat(")");
                    }
                    break;
                case ProductInfo.ProductRanage.RangeUnSelect: //根据上架范围显示未上架的商品

                    //本身上架的记录
                    builder2.AppendFormat(" AND ProductId Not IN (SELECT ProductId FROM Hishop_DistributorProducts WHERE UserId={0})", distributorId);

                    //代理取系统设置的上架范围，分销商取所属代理商的上架范围
                    int ofAgentIDUn = distributorId;
                    DataTable dtDistributorsUn = this.database.ExecuteDataSet(CommandType.Text,
                        string.Format("Select UserId,IsAgent,AgentPath From aspnet_Distributors Where UserId={0}", ofAgentIDUn)).Tables[0];
                    if (dtDistributorsUn.Rows[0]["IsAgent"].ToString() != "1" && dtDistributorsUn.Rows[0]["AgentPath"] != DBNull.Value)
                        ofAgentIDUn = int.Parse(dtDistributorsUn.Rows[0]["AgentPath"].ToString().Split('|')[dtDistributorsUn.Rows[0]["AgentPath"].ToString().Split('|').Length - 1]);
                    DataTable dtProductRangeUn = this.database.ExecuteDataSet(CommandType.Text,
                        string.Format("Select * From Hishop_DistributorProductRange Where UserId={0}", ofAgentIDUn)).Tables[0];
                    if (dtProductRangeUn.Rows.Count == 0)
                    {
                        builder2.AppendFormat(" And 1=2 ");//未设置上架范围
                    }
                    else
                    {
                        DataRow drProductRange = dtProductRangeUn.Rows[0];
                        builder2.AppendFormat(" AND (");
                        if (drProductRange["ProductRange1"] != DBNull.Value)
                            builder2.AppendFormat(string.Format("CategoryId in({0})", drProductRange["ProductRange1"].ToString()));
                        else
                            builder2.AppendFormat("CategoryId in(null)");
                        if (drProductRange["ProductRange2"] != DBNull.Value)
                            builder2.AppendFormat(string.Format(" or BrandId in({0})", drProductRange["ProductRange2"].ToString()));
                        else
                            builder2.AppendFormat(" or BrandId in(null)");
                        if (drProductRange["ProductRange3"] != DBNull.Value)
                            builder2.AppendFormat(string.Format(" or TypeId in({0})", drProductRange["ProductRange3"].ToString()));
                        else
                            builder2.AppendFormat(" or TypeId in(null)");
                        builder2.AppendFormat(")");
                    }
                    break;
            }
            if (string.IsNullOrWhiteSpace(sort))
            {
                sort = "ProductId";
            }
            DbQueryResult result = DataHelper.PagingByRownumber(pageNumber, maxNum, sort, isAsc ? SortAction.Asc : SortAction.Desc, true, "vw_Hishop_BrowseProductList", "ProductId", builder2.ToString(), builder.ToString());
            DataTable data = (DataTable)result.Data;
            toal = result.TotalRecords;
            return data;
        }

        public DataTable GetTopicProducts(MemberInfo member, int topicid, int maxNum)
        {
            int discount = 100;
            StringBuilder builder = new StringBuilder();
            builder.Append("select top " + maxNum);
            builder.Append(" p.ProductId, ProductCode, ProductName,ShortDescription,ShowSaleCounts,ThumbnailUrl40,ThumbnailUrl100,ThumbnailUrl160,MarketPrice,");
            if (member != null)
            {
                discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1", member.GradeId);
                builder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice, ", member.GradeId, discount);
            }
            else
            {
                builder.Append("SalePrice,");
            }
            builder.Append("SaleCounts, Stock,t.DisplaySequence from vw_Hishop_BrowseProductList p inner join  Vshop_RelatedTopicProducts t on p.productid=t.RelatedProductId where t.topicid=" + topicid);
            builder.AppendFormat(" and SaleStatus = {0}", 1);
            builder.Append(" order by t.DisplaySequence asc");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        new public DataTable GetType()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select distinct ActivitiesType  from Hishop_Activities where datediff(dd,GETDATE(),StartTime)<=0 and datediff(dd,GETDATE(),EndTIme)>=0");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }
        
        public ProductBrowseInfo GetProductBrowseInfo(int productId, int? maxReviewNum, int? maxConsultationNum, int storeStockValidateType, bool MutiStores = false)
        {
            int discount = 100;
            ProductBrowseInfo info = new ProductBrowseInfo();
            StringBuilder builder = new StringBuilder();
            
            builder.Append("UPDATE Hishop_Products SET VistiCounts = VistiCounts + 1 WHERE ProductId = @ProductId;");
            builder.Append(" SELECT * ,CASE WHEN BrandId IS NULL THEN NULL ELSE (SELECT BrandName FROM Hishop_BrandCategories WHERE BrandId= p.BrandId) END AS BrandName");
            builder.Append(" FROM Hishop_Products p where ProductId=@ProductId;");
            //if (HiContext.Current.User.UserRole == UserRole.Member)
            //{
            //    Member user = HiContext.Current.User as Member;
            //    discount = new MemberGradeDao().GetMemberGrade(user.GradeId).Discount;
            //    if (!MutiStores)
            //    {
            //        builder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock,WarningStock, CostPrice,StoreStock=Stock,");
            //    }
            //    else if (storeStockValidateType == 1)
            //    {
            //        builder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock,WarningStock, CostPrice,StoreStock=(SELECT ISNULL(MAX(Stock),s.Stock) FROM Hishop_StoreStock WHERE SkuId = s.SkuID),");
            //    }
            //    else if (storeStockValidateType == 2)
            //    {
            //        builder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock,WarningStock, CostPrice,StoreStock=(SELECT ISNULL(SUM(Stock),s.Stock) FROM Hishop_StoreStock WHERE SkuId = s.SkuID),");
            //    }
            //    builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", user.GradeId);
            //    builder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", user.GradeId, discount);
            //    builder.Append(" FROM Hishop_SKUs s WHERE ProductId = @ProductId");
            //}
            if (!MutiStores)
            {
                builder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, CostPrice,StoreStock=Stock, SalePrice FROM Hishop_SKUs WHERE ProductId = @ProductId");
            }
            else if (storeStockValidateType == 1)
            {
                builder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, CostPrice,StoreStock=(SELECT ISNULL(MAX(stock),Hishop_Skus.Stock) FROM Hishop_StoreStock ss WHERE SkuId = SkuID), SalePrice FROM Hishop_SKUs WHERE ProductId = @ProductId");
            }
            else if (storeStockValidateType == 2)
            {
                builder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, CostPrice,StoreStock=(SELECT ISNULL(SUM(stock),Hishop_Skus.Stock) FROM Hishop_StoreStock ss WHERE SkuId = SkuID), SalePrice FROM Hishop_SKUs WHERE ProductId = @ProductId");
            }
            if (maxReviewNum.HasValue)
            {
                builder.AppendFormat(" SELECT TOP {0} * FROM Hishop_ProductReviews where ProductId=@ProductId ORDER BY ReviewId DESC; ", maxReviewNum);
                builder.Append(" SELECT Count(*) FROM Hishop_ProductReviews where ProductId=@ProductId; ");
            }
            else
            {
                builder.Append(" SELECT * FROM Hishop_ProductReviews where ProductId=@ProductId ORDER BY ReviewId DESC; ");
                builder.Append(" SELECT Count(*) FROM Hishop_ProductReviews where ProductId=@ProductId; ");
            }
            if (maxConsultationNum.HasValue)
            {
                builder.AppendFormat(" SELECT TOP {0} * FROM Hishop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ORDER BY ConsultationId DESC ;", maxConsultationNum);
                builder.Append(" SELECT Count(*) FROM Hishop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ; ");
            }
            else
            {
                builder.Append(" SELECT * FROM Hishop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ORDER BY ConsultationId DESC ;");
                builder.Append(" SELECT Count(*) FROM Hishop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ; ");
            }
            builder.Append(" SELECT a.AttributeId, AttributeName, ValueStr FROM Hishop_ProductAttributes pa JOIN Hishop_Attributes a ON pa.AttributeId = a.AttributeId");
            builder.Append(" JOIN Hishop_AttributeValues v ON a.AttributeId = v.AttributeId AND pa.ValueId = v.ValueId  WHERE ProductId = @ProductId ORDER BY a.DisplaySequence DESC, v.DisplaySequence DESC");
            builder.Append(" SELECT SkuId, a.AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr, ImageUrl FROM Hishop_SKUItems s join Hishop_Attributes a on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId) ORDER BY a.DisplaySequence DESC,av.DisplaySequence DESC;");
            builder.Append(" SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice FROM vw_Hishop_BrowseProductList");
            builder.AppendFormat(" WHERE SaleStatus = {0} AND ProductId IN (SELECT RelatedProductId FROM Hishop_RelatedProducts WHERE ProductId = {1}) ORDER BY DisplaySequence DESC;", 1, productId);
            builder.Append("SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice FROM vw_Hishop_BrowseProductList");
            builder.AppendFormat(" WHERE SaleStatus = {0} AND ProductId<>{1}  AND CategoryId = (SELECT CategoryId FROM Hishop_Products WHERE ProductId={1} AND SaleStatus = {0})", 1, productId);
            builder.AppendFormat(" AND ProductId NOT IN (SELECT RelatedProductId FROM Hishop_RelatedProducts WHERE ProductId = {0})", productId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info.Product = DataMapper.PopulateProduct(reader);
                    if (reader["BrandName"] != DBNull.Value)
                    {
                        info.BrandName = (string)reader["BrandName"];
                    }
                }
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        info.Product.Skus.Add((string)reader["SkuId"], DataMapper.PopulateSKU(reader));
                    }
                }
                if (reader.NextResult())
                {
                    info.DBReviews = DataHelper.ConverDataReaderToDataTable(reader);
                }
                if (reader.NextResult() && reader.Read())
                {
                    info.ReviewCount = (int)reader[0];
                }
                if (reader.NextResult())
                {
                    info.DBConsultations = DataHelper.ConverDataReaderToDataTable(reader);
                }
                if (reader.NextResult() && reader.Read())
                {
                    info.ConsultationCount = (int)reader[0];
                }
                if (reader.NextResult())
                {
                    DataTable table = DataHelper.ConverDataReaderToDataTable(reader);
                    if ((table != null) && (table.Rows.Count > 0))
                    {
                        DataTable table2 = table.Clone();
                        foreach (DataRow row in table.Rows)
                        {
                            bool flag = false;
                            if (table2.Rows.Count > 0)
                            {
                                foreach (DataRow row2 in table2.Rows)
                                {
                                    if (((int)row2["AttributeId"]) == ((int)row["AttributeId"]))
                                    {
                                        DataRow row4;
                                        flag = true;
                                        (row4 = row2)["ValueStr"] = row4["ValueStr"] + ", " + row["ValueStr"];
                                    }
                                }
                            }
                            if (!flag)
                            {
                                DataRow row3 = table2.NewRow();
                                row3["AttributeId"] = row["AttributeId"];
                                row3["AttributeName"] = row["AttributeName"];
                                row3["ValueStr"] = row["ValueStr"];
                                table2.Rows.Add(row3);
                            }
                        }
                        info.DbAttribute = table2;
                    }
                }
                if (reader.NextResult())
                {
                    info.DbSKUs = DataHelper.ConverDataReaderToDataTable(reader);
                    info.ListSKUs = ReaderConvert.ReaderToList<SKUItem>(reader);
                }
                if (reader.NextResult())
                {
                    info.DbCorrelatives = DataHelper.ConverDataReaderToDataTable(reader);
                    info.ListCorrelatives = ReaderConvert.ReaderToList<ProductInfo>(reader);
                }
                if (!reader.NextResult())
                {
                    return info;
                }
                info.DbCorrelatives.Merge(DataHelper.ConverDataReaderToDataTable(reader), true);
                System.Collections.Generic.IList<ProductInfo> list = ReaderConvert.ReaderToList<ProductInfo>(reader);
                if (list == null)
                {
                    return info;
                }
                foreach (ProductInfo info2 in list)
                {
                    info.ListCorrelatives.Add(info2);
                }
            }
            return info;
        }
        
        public DataTable GetProducts(ProductQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" WHERE SaleStatus = {0}", 1);
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] strArray = Regex.Split(query.Keywords.Trim(), @"\s+");
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[0]));
                for (int i = 1; (i < strArray.Length) && (i <= 4); i++)
                {
                    builder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
                }
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (query.CategoryId.HasValue && (query.CategoryId.Value > 0))
            {
                builder.AppendFormat(" AND MainCategoryPath LIKE '{0}|%'", query.MaiCategoryPath);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ProductId,ProductName FROM Hishop_Products" + builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 根据分类id获取商品实体类
        /// </summary>
        public IList<ProductInfo> GetProductList(int categoryId)
        {
            StringBuilder builder = new StringBuilder();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(
                @"SELECT * FROM Hishop_Products WHERE categoryId =@CategoryId and SaleStatus = 1;
                SELECT SkuId, ProductId, SKU,Weight, Stock, CostPrice, SalePrice FROM Hishop_SKUs s WHERE ProductId in(Select ProductId From Hishop_Products Where categoryId =@CategoryId and SaleStatus = 1)");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            
            DataSet dsData=this.database.ExecuteDataSet(sqlStringCommand);
            dsData.Tables[0].TableName = "Hishop_Products";
            dsData.Tables[1].TableName = "Hishop_SKUs";
            dsData.Tables[0].PrimaryKey = new DataColumn[] { dsData.Tables[0].Columns["ProductId"] };
            dsData.Tables[1].PrimaryKey = new DataColumn[] { dsData.Tables[1].Columns["SkuId"] };
            DataRelation dataRelation = new DataRelation("Products2SKUs", dsData.Tables[0].Columns["ProductId"], dsData.Tables[1].Columns["ProductId"]);
            dsData.Relations.Add(dataRelation);

            IList<ProductInfo> infoList = new List<ProductInfo>();
            foreach (DataRow dr in dsData.Tables["Hishop_Products"].Rows)
            {
                infoList.Add(DataMapper.PopulateProduct(dr));
            }
            return infoList;
        }


        public int SelectDataCount(string tablename, string where = "")
        {
            string selectSql = string.Format("Select count(*) From {0} {1}", tablename, "");
            int strCount = this.database.ExecuteScalar(this.database.GetSqlStringCommand(selectSql)).ToInt();
            return strCount;
        }

        public DataTable SelectPageData(string tablename, string orderFields, string selectFields, int currPage, int pagesize, string where)
        {
            string selectSql = @"Select * From (
                                    Select ROW_NUMBER() over(order by {1} ) as rid ,{2} From {0} {5}
                                )t Where t.rid>={3} And t.rid<{4}";
            int startIndex = (currPage - 1) * pagesize + 1;
            int endIndex = startIndex + pagesize;
            selectSql = string.Format(selectSql, tablename, orderFields, selectFields, startIndex, endIndex, "");
            using (IDataReader reader = this.database.ExecuteReader(this.database.GetSqlStringCommand(selectSql)))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
            //return this.database.ExecuteDataSet(CommandType.Text, selectSql).Tables[0];
        }

        public DataTable getSkusByWhere(string where)
        {
            string selectSql = string.Format("SELECT SkuId, ProductId, SKU,Weight, Stock, CostPrice, SalePrice FROM Hishop_SKUs s WHERE ProductId in(Select ProductId From Hishop_Products {0} )", where );
            using (IDataReader reader = this.database.ExecuteReader(this.database.GetSqlStringCommand(selectSql)))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

    }
}

