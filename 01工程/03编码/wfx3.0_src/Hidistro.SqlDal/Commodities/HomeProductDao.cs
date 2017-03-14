namespace Hidistro.SqlDal.Commodities
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.SqlDal.Members;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class HomeProductDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddHomeProdcut(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Vshop_HomeProducts(ProductId,DisplaySequence) VALUES (@ProductId,0)");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            try
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            catch
            {
                return false;
            }
        }

        public DataTable GetAllFull()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select  * from Hishop_Activities where datediff(dd,GETDATE(),StartTime)<=0 and datediff(dd,GETDATE(),EndTIme)>=0 order by MeetMoney asc    ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetHomeProducts()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select p.ProductId, ProductCode, ProductName,ShortDescription,ThumbnailUrl40,ThumbnailUrl160,ThumbnailUrl100,MarketPrice, SalePrice,ShowSaleCounts,SaleCounts, Stock,t.DisplaySequence from vw_Hishop_BrowseProductList p inner join  Vshop_HomeProducts t on p.productid=t.ProductId ");
            builder.AppendFormat(" and SaleStatus = {0}", 1);
            builder.Append(" order by t.DisplaySequence asc");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DbQueryResult GetHomeProductsDrop(MemberInfo member, ProductQuery query, bool isdistributor)
        {
            int discount = 100;
            StringBuilder builder = new StringBuilder();
            int currentDistributorId = Globals.GetCurrentDistributorId();
            builder.Append("MainCategoryPath,ProductId, ProductCode,ShortDescription,ProductName,ShowSaleCounts, ThumbnailUrl60,ThumbnailUrl40,ThumbnailUrl100,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310, MarketPrice,");
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
            builder.Append("SaleCounts, Stock");
            StringBuilder builder2 = new StringBuilder(" SaleStatus =" + 1);
            if (query.CategoryId > 0)
            {
                builder2.AppendFormat(" and CategoryId={0}", query.CategoryId);
            }
            if (isdistributor && (currentDistributorId > 0))
            {
                builder2.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_DistributorProducts WHERE UserId={0})", currentDistributorId);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", builder2.ToString(), builder.ToString());
        }

        /// <summary>
        /// add by:JHB 150916 获取首页商品信息【根据配置来限定商品的范围】
        /// type:0正常显示店铺已上架的商品，1正常显示店铺未上架的商品，2显示所有出售状态的商品，3根据上架范围显示已上架的商品，4根据上架范围显示未上架的商品
        /// </summary>
        public DbQueryResult GetHomeProductsEx(MemberInfo member, ProductQuery query, ProductInfo.ProductRanage productRanage)
        {
            int discount = 100;
            StringBuilder builder = new StringBuilder();
            int currentDistributorId = Globals.GetCurrentDistributorId();
            builder.Append("MainCategoryPath,ProductId, ProductCode,ShortDescription,ProductName,ShowSaleCounts, ThumbnailUrl60,ThumbnailUrl40,ThumbnailUrl100,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310, MarketPrice,");
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
            builder.Append("SaleCounts, Stock");
            StringBuilder builder2 = new StringBuilder(" SaleStatus =" + 1);
            if (query.CategoryId > 0)
            {
                builder2.AppendFormat(" and CategoryId={0}", query.CategoryId);
            }
            if (query.TypeId > 0)
            {
                builder2.AppendFormat(" and TypeId={0}", query.TypeId);
            }
            switch (productRanage)
            {
                case ProductInfo.ProductRanage.NormalSelect:    //正常显示店铺已上架的商品
                    builder2.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_DistributorProducts WHERE UserId={0})", currentDistributorId);
                    break;
                case ProductInfo.ProductRanage.All:             //显示所有出售状态的商品
                    break;
                case ProductInfo.ProductRanage.RangeSelect:     //根据上架范围显示已上架的商品

                    //本身上架的记录
                    builder2.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_DistributorProducts WHERE UserId={0})", currentDistributorId);

                    //代理取系统设置的上架范围，分销商取所属代理商的上架范围
                    int ofAgentID = currentDistributorId;
                    DataTable dtDistributors = this.database.ExecuteDataSet( CommandType.Text,
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
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", builder2.ToString(), builder.ToString());
        }

        /// <summary>
        /// 产品Top显示 数据集
        /// </summary>
        public DataTable GetHomeProductTop(int top, ProductInfo.ProductTop productTop, ProductInfo.ProductRanage productRanage)
        {
            int currentDistributorId = Globals.GetCurrentDistributorId();
            string where = "Where 1=1";
            string order = "";
            switch (productTop)
            { 
                case ProductInfo.ProductTop.New:
                    order = " order by AddedDate desc";
                    break;
                case ProductInfo.ProductTop.Hot:
                    order = " order by ShowSaleCounts desc";
                    break;
                case ProductInfo.ProductTop.Discount:
                    order = " order by (MarketPrice-SalePrice) desc";
                    break;
                case ProductInfo.ProductTop.MostLike:
                    order = " order by VistiCounts desc";
                    break;
                case ProductInfo.ProductTop.Activity:
                    order = " order by VistiCounts desc";
                    break;
                default :
                    order = " order by ShowSaleCounts desc";
                    break;
            }
            switch (productRanage)
            {
                case ProductInfo.ProductRanage.NormalSelect:    //正常显示店铺已上架的商品
                    where += string.Format(" AND ProductId IN (SELECT ProductId FROM Hishop_DistributorProducts WHERE UserId={0})", currentDistributorId);
                    break;
                case ProductInfo.ProductRanage.All:             //显示所有出售状态的商品
                    break;
                case ProductInfo.ProductRanage.RangeSelect:     //根据上架范围显示已上架的商品

                    //本身上架的记录
                    where += string.Format(" AND ProductId IN (SELECT ProductId FROM Hishop_DistributorProducts WHERE UserId={0})", currentDistributorId);

                    //代理取系统设置的上架范围，分销商取所属代理商的上架范围
                    int ofAgentID = currentDistributorId;
                    DataTable dtDistributors = this.database.ExecuteDataSet(CommandType.Text,
                        string.Format("Select UserId,IsAgent,AgentPath From aspnet_Distributors Where UserId={0}", ofAgentID)).Tables[0];
                    if (dtDistributors.Rows[0]["IsAgent"].ToString() != "1" && dtDistributors.Rows[0]["AgentPath"] != DBNull.Value)
                        ofAgentID = int.Parse(dtDistributors.Rows[0]["AgentPath"].ToString().Split('|')[dtDistributors.Rows[0]["AgentPath"].ToString().Split('|').Length - 1]);
                    DataTable dtProductRange = this.database.ExecuteDataSet(CommandType.Text,
                        string.Format("Select * From Hishop_DistributorProductRange Where UserId={0}", ofAgentID)).Tables[0];
                    if (dtProductRange.Rows.Count == 0)
                    {
                        where += string.Format(" And 1=2 ");//未设置上架范围
                    }
                    else
                    {
                        DataRow drProductRange = dtProductRange.Rows[0];
                        where += string.Format(" AND (");
                        if (drProductRange["ProductRange1"] != DBNull.Value)
                            where += string.Format(string.Format("CategoryId in({0})", drProductRange["ProductRange1"].ToString()));
                        else
                            where += string.Format("CategoryId in(null)");
                        if (drProductRange["ProductRange2"] != DBNull.Value)
                            where += string.Format(string.Format(" or BrandId in({0})", drProductRange["ProductRange2"].ToString()));
                        else
                            where += string.Format(" or BrandId in(null)");
                        if (drProductRange["ProductRange3"] != DBNull.Value)
                            where += string.Format(string.Format(" or TypeId in({0})", drProductRange["ProductRange3"].ToString()));
                        else
                            where += string.Format(" or TypeId in(null)");
                        where += string.Format(")");
                    }
                    break;
            }
            string topStr = (top == 0) ? "count(*)" : string.Format("top {0} *",top);
            if (top == 0) order = "";
            string selectSql = string.Format("Select {0}  From vw_Hishop_BrowseProductList {1} {2}", topStr, where, order);
            return this.database.ExecuteDataSet(CommandType.Text, selectSql).Tables[0];
        }

        /// <summary>
        /// 产品Top显示新增产品类型参数筛选
        /// </summary>
        public DataTable GetHomeProductTop(string top, ProductInfo.ProductTop productTop, ProductInfo.ProductRanage productRanage, int CategoryId)
        {
            string where = "Where 1=1";
            string order = "";
            string topStr = " top " + top+ " ";
            int currentDistributorId = Globals.GetCurrentDistributorId();
            switch (productTop)
            {
                case ProductInfo.ProductTop.Category:
                    topStr += "a.*,b.name";
                    where = string.Format(" a left join Hishop_Categories b on a.CategoryId=b.CategoryId where a.CategoryId =  {0}", CategoryId);
                    break;
                default:
                    topStr += "a.*";
                    break;
            }
            switch (productRanage)
            {
                case ProductInfo.ProductRanage.NormalSelect:    //正常显示店铺已上架的商品
                    where += string.Format(" AND ProductId IN (SELECT ProductId FROM Hishop_DistributorProducts WHERE UserId={0})", currentDistributorId);
                    break;
                case ProductInfo.ProductRanage.All:             //显示所有出售状态的商品
                    break;
                case ProductInfo.ProductRanage.RangeSelect:     //根据上架范围显示已上架的商品

                    //本身上架的记录
                    where += string.Format(" AND a.ProductId IN (SELECT ProductId FROM Hishop_DistributorProducts WHERE UserId={0})", currentDistributorId);

                    //代理取系统设置的上架范围，分销商取所属代理商的上架范围
                    int ofAgentID = currentDistributorId;
                    DataTable dtDistributors = this.database.ExecuteDataSet(CommandType.Text,
                        string.Format("Select UserId,IsAgent,AgentPath From aspnet_Distributors Where UserId={0}", ofAgentID)).Tables[0];
                    if (dtDistributors.Rows[0]["IsAgent"].ToString() != "1" && dtDistributors.Rows[0]["AgentPath"] != DBNull.Value)
                        ofAgentID = int.Parse(dtDistributors.Rows[0]["AgentPath"].ToString().Split('|')[dtDistributors.Rows[0]["AgentPath"].ToString().Split('|').Length - 1]);
                    DataTable dtProductRange = this.database.ExecuteDataSet(CommandType.Text,
                        string.Format("Select * From Hishop_DistributorProductRange Where UserId={0}", ofAgentID)).Tables[0];
                    if (dtProductRange.Rows.Count == 0)
                    {
                        where += string.Format(" And 1=2 ");//未设置上架范围
                    }
                    else
                    {
                        DataRow drProductRange = dtProductRange.Rows[0];
                        where += string.Format(" AND (");
                        if (drProductRange["ProductRange1"] != DBNull.Value)
                            where += string.Format(string.Format("a.CategoryId in({0})", drProductRange["ProductRange1"].ToString()));
                        else
                            where += string.Format("a.CategoryId in(null)");
                        if (drProductRange["ProductRange2"] != DBNull.Value)
                            where += string.Format(string.Format(" or a.BrandId in({0})", drProductRange["ProductRange2"].ToString()));
                        else
                            where += string.Format(" or a.BrandId in(null)");
                        if (drProductRange["ProductRange3"] != DBNull.Value)
                            where += string.Format(string.Format(" or a.TypeId in({0})", drProductRange["ProductRange3"].ToString()));
                        else
                            where += string.Format(" or a.TypeId in(null)");
                        where += string.Format(")");
                    }
                    break;
            }
            if (top == "0") topStr = "count(*)";
            if (top == "0") order = "";
            string selectSql = string.Format("Select {0} From vw_Hishop_BrowseProductList {1} {2}", topStr, where, order);
            return this.database.ExecuteDataSet(CommandType.Text, selectSql).Tables[0];
        }

        public DbQueryResult GetProducts(ProductQuery query)
        {
            query.IsIncludeHomeProduct = false;
            return new ProductDao().GetProducts(query);
        }

        public bool RemoveAllHomeProduct()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Vshop_HomeProducts");
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool RemoveHomeProduct(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Vshop_HomeProducts WHERE ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateHomeProductSequence(int ProductId, int displaysequence)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Vshop_HomeProducts  set DisplaySequence=@DisplaySequence where ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "@DisplaySequence", DbType.Int32, displaysequence);
            this.database.AddInParameter(sqlStringCommand, "@ProductId", DbType.Int32, ProductId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

