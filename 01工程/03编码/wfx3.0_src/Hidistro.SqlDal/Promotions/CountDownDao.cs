namespace Hidistro.SqlDal.Promotions
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.SqlDal.Commodities;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class CountDownDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddCountDown(CountDownInfo countDownInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_CountDown;INSERT INTO Hishop_CountDown(ProductId,CountDownPrice,StartDate,EndDate,Content,DisplaySequence,MaxCount ) VALUES(@ProductId,@CountDownPrice,@StartDate,@EndDate,@Content,@DisplaySequence,@MaxCount );");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, countDownInfo.ProductId);
            this.database.AddInParameter(sqlStringCommand, "CountDownPrice", DbType.Currency, countDownInfo.CountDownPrice);
            this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, countDownInfo.StartDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, countDownInfo.EndDate);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, countDownInfo.Content);
            this.database.AddInParameter(sqlStringCommand, "MaxCount", DbType.Int32, countDownInfo.MaxCount);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeleteCountDown(int countDownId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_CountDown WHERE CountDownId=@CountDownId");
            this.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public DataTable GetCounDownProducList(int maxnum)
        {
            DataTable table = new DataTable();
            string query = string.Format("select top " + maxnum + " CountDownId,ProductId,ProductName,SalePrice,CountDownPrice,StartDate,EndDate,MarketPrice, ThumbnailUrl60,ThumbnailUrl100, ThumbnailUrl160,ThumbnailUrl180, ThumbnailUrl220,ThumbnailUrl310 from vw_Hishop_CountDown where datediff(hh,EndDate,getdate())<=0 AND ProductId IN(SELECT ProductId FROM Hishop_Products WHERE SaleStatus={0}) order by DisplaySequence desc", 1);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public CountDownInfo GetCountDownByProductId(int productId)
        {
            CountDownInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_CountDown WHERE datediff(hh,EndDate,getdate())<=0 AND ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateCountDown(reader);
                }
            }
            return info;
        }

        public CountDownInfo GetCountDownInfo(int countDownId)
        {
            CountDownInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_CountDown WHERE CountDownId=@CountDownId");
            this.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateCountDown(reader);
                }
            }
            return info;
        }

        public DbQueryResult GetCountDownList(GroupBuyQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                builder.AppendFormat("ProductName Like '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
            }
            string selectFields = "CountDownId,productId,ProductName,CountDownPrice,StartDate,EndDate,DisplaySequence,MaxCount ";
            return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_CountDown", "CountDownId", builder.ToString(), selectFields);
        }
        
        public DbQueryResult GetCountDownProductList(ProductBrowseQuery query)
        {
            string filter = string.Format(" datediff(hh,EndDate,getdate())<0 AND ProductId IN(SELECT ProductId FROM Hishop_Products WHERE SaleStatus={0})", 1);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_CountDown", "CountDownId", filter, "*");
        }
        
        public DataTable GetCountDownProductList(int? categoryId, string keyWord, int page, int size, out int total, bool onlyUnFinished = true)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("a.CountDownId,a.ProductId,a.ProductName,b.ProductCode,b.ShortDescription,a.CountDownPrice,");
            builder.Append(" a.ThumbnailUrl60,a.ThumbnailUrl100,a.ThumbnailUrl160,a.ThumbnailUrl180,a.ThumbnailUrl220,a.ThumbnailUrl310,a.MarketPrice,b.SalePrice,a.MaxCount");
            StringBuilder builder2 = new StringBuilder();
            builder2.Append(" vw_Hishop_CountDown a left join vw_Hishop_BrowseProductList b on a.ProductId = b.ProductId  ");
            StringBuilder builder3 = new StringBuilder(" SaleStatus=1");
            if (onlyUnFinished)
            {
                string str = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                builder3.AppendFormat(" AND ( a.StartDate <= '{0}' ) AND ( a.EndDate >= '{0}') ", str);
            }
            if (categoryId.HasValue)
            {
                CategoryInfo category = new CategoryDao().GetCategory(categoryId.Value);
                if (category != null)
                {
                    builder3.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%' OR ExtendCategoryPath1 LIKE '{0}|%' OR ExtendCategoryPath2 LIKE '{0}|%' OR ExtendCategoryPath3 LIKE '{0}|%' OR ExtendCategoryPath4 LIKE '{0}|%') ", category.Path);
                }
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                builder3.AppendFormat(" AND (ProductName LIKE '%{0}%' OR ProductCode LIKE '%{0}%')", keyWord);
            }
            string sortBy = "a.DisplaySequence";
            DbQueryResult result = DataHelper.PagingByRownumber(page, size, sortBy, SortAction.Desc, true, builder2.ToString(), "CountDownId", builder3.ToString(), builder.ToString());
            DataTable data = (DataTable) result.Data;
            total = result.TotalRecords;
            return data;
        }

        public bool ProductCountDownExist(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_CountDown WHERE ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public void SwapCountDownSequence(int countDownId, int displaySequence)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_CountDown SET DisplaySequence = @DisplaySequence WHERE CountDownId=@CountDownId");
            this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.Int32, displaySequence);
            this.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool UpdateCountDown(CountDownInfo countDownInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_CountDown SET ProductId=@ProductId,CountDownPrice=@CountDownPrice,StartDate=@StartDate,EndDate=@EndDate,Content=@Content,MaxCount=@MaxCount  WHERE CountDownId=@CountDownId");
            this.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownInfo.CountDownId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, countDownInfo.ProductId);
            this.database.AddInParameter(sqlStringCommand, "CountDownPrice", DbType.Currency, countDownInfo.CountDownPrice);
            this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, countDownInfo.StartDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, countDownInfo.EndDate);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, countDownInfo.Content);
            this.database.AddInParameter(sqlStringCommand, "MaxCount", DbType.Int32, countDownInfo.MaxCount);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

