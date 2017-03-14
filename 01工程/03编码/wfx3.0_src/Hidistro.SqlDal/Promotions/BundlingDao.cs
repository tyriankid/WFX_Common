namespace Hidistro.SqlDal.Promotions
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class BundlingDao
    {
        private Database database = DatabaseFactory.CreateDatabase();
        /*
        public int AddBundlingProduct(BundlingInfo bind, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_BundlingProducts;INSERT INTO Hishop_BundlingProducts(Name,ShortDescription,Num,Price,SaleStatus,AddTime,DisplaySequence) VALUES(@Name,@ShortDescription,@Num,@Price,@SaleStatus,@AddTime,@DisplaySequence); SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, bind.Name);
            this.database.AddInParameter(sqlStringCommand, "ShortDescription", DbType.String, bind.ShortDescription);
            this.database.AddInParameter(sqlStringCommand, "Num", DbType.Int32, bind.Num);
            this.database.AddInParameter(sqlStringCommand, "Price", DbType.Currency, bind.Price);
            this.database.AddInParameter(sqlStringCommand, "SaleStatus", DbType.Int32, bind.SaleStatus);
            this.database.AddInParameter(sqlStringCommand, "AddTime", DbType.DateTime, bind.AddTime);
            object obj2 = null;
            if (dbTran != null)
            {
                obj2 = this.database.ExecuteScalar(sqlStringCommand, dbTran);
            }
            else
            {
                obj2 = this.database.ExecuteScalar(sqlStringCommand);
            }
            if (obj2 != null)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }

        public bool AddBundlingProductItems(int bundlingID, List<BundlingItemInfo> BundlingItemInfos, DbTransaction dbTran)
        {
            if (BundlingItemInfos.Count <= 0)
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_BundlingProductItems(BundlingID,ProductID,SkuId,ProductNum) VALUES(@BundlingID,@ProductID,@Skuid,@ProductNum)");
            this.database.AddInParameter(sqlStringCommand, "BundlingID", DbType.Int32, bundlingID);
            this.database.AddInParameter(sqlStringCommand, "ProductID", DbType.Int32);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String);
            this.database.AddInParameter(sqlStringCommand, "ProductNum", DbType.Int32);
            try
            {
                foreach (BundlingItemInfo info in BundlingItemInfos)
                {
                    this.database.SetParameterValue(sqlStringCommand, "ProductID", info.ProductID);
                    this.database.SetParameterValue(sqlStringCommand, "SkuId", info.SkuId);
                    this.database.SetParameterValue(sqlStringCommand, "ProductNum", info.ProductNum);
                    if (dbTran != null)
                    {
                        this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
                    }
                    else
                    {
                        this.database.ExecuteNonQuery(sqlStringCommand);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        */
        public bool DeleteBundlingByID(int BundlingID, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_BundlingProductItems WHERE  BundlingID=@BundlingID");
            this.database.AddInParameter(sqlStringCommand, "BundlingID", DbType.Int32, BundlingID);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeleteBundlingProduct(int BundlingID)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_BundlingProducts WHERE BundlingID=@BundlingID");
            this.database.AddInParameter(sqlStringCommand, "BundlingID", DbType.Int32, BundlingID);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /*
        public PromotionInfo GetAllProductPromotionInfo(int productid)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select * from Hishop_Promotions WHERE DateDiff(DD, StartDate, getdate()) >= 0 AND DateDiff(DD, EndDate, getdate()) <= 0 ");
            builder.Append("and ActivityId=(select ActivityId from dbo.Hishop_PromotionProducts where productid=@productid)");
            builder.Append(" AND ActivityId IN (SELECT ActivityId FROM Hishop_PromotionMemberGrades)");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "productid", DbType.Int32, productid);
            PromotionInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePromote(reader);
                }
            }
            return info;
        }

        public BundlingInfo GetBundlingInfo(int bundlingID)
        {
            BundlingInfo info = null;
            StringBuilder builder = new StringBuilder("SELECT * FROM Hishop_BundlingProducts WHERE BundlingID=@BundlingID;");
            builder.Append("SELECT [BundlingID] ,a.[ProductId] ,[SkuId] ,[ProductNum], productName,");
            builder.Append(" (select saleprice FROM  Hishop_SKUs c where c.SkuId= a.SkuId ) as ProductPrice");
            builder.Append(" FROM  Hishop_BundlingProductItems a JOIN Hishop_Products p ON a.ProductID = p.ProductId where BundlingID=@BundlingID AND p.SaleStatus = 1");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "BundlingID", DbType.Int32, bundlingID);
            List<BundlingItemInfo> list = new List<BundlingItemInfo>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateBindInfo(reader);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    BundlingItemInfo item = new BundlingItemInfo {
                        ProductID = (int) reader["ProductID"],
                        ProductNum = (int) reader["ProductNum"]
                    };
                    if (reader["SkuId"] != DBNull.Value)
                    {
                        item.SkuId = (string) reader["SkuId"];
                    }
                    if (reader["ProductName"] != DBNull.Value)
                    {
                        item.ProductName = (string) reader["ProductName"];
                    }
                    if (reader["ProductPrice"] != DBNull.Value)
                    {
                        item.ProductPrice = (decimal) reader["ProductPrice"];
                    }
                    item.BundlingID = bundlingID;
                    list.Add(item);
                }
            }
            if (info != null)
            {
                info.BundlingItemInfos = list;
            }
            return info;
        }
        
        public List<BundlingItemInfo> GetBundlingItemsByID(int bundlingID)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT [BundlingID] ,a.[ProductId] ,[SkuId] ,[ProductNum], productName, ");
            builder.Append(" (select saleprice FROM  Hishop_SKUs c where c.SkuId= a.SkuId ) as ProductPrice");
            builder.Append(" FROM  Hishop_BundlingProductItems a JOIN Hishop_Products p ON a.ProductID = p.ProductId where BundlingID=@BundlingID AND p.SaleStatus = 1");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "BundlingID", DbType.Int32, bundlingID);
            List<BundlingItemInfo> list = new List<BundlingItemInfo>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    BundlingItemInfo item = new BundlingItemInfo {
                        ProductID = (int) reader["ProductID"],
                        ProductNum = (int) reader["ProductNum"],
                        SkuId = (string) reader["SkuId"],
                        ProductName = (string) reader["ProductName"]
                    };
                    if (reader["ProductPrice"] != DBNull.Value)
                    {
                        item.ProductPrice = (decimal) reader["ProductPrice"];
                    }
                    item.BundlingID = bundlingID;
                    list.Add(item);
                }
            }
            return list;
        }

        public DbQueryResult GetBundlingProducts(BundlingInfoQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1");
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                builder.AppendFormat(" AND Name Like '%{0}%' ", DataHelper.CleanSearchString(query.ProductName));
            }
            string selectFields = "Bundlingid,Name,Num,price,SaleStatus,OrderCount,AddTime,DisplaySequence,ShortDescription";
            return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BundlingProducts", "Bundlingid", builder.ToString(), selectFields);
        }

        public PromotionInfo GetProductPromotionInfo(int productid, Member member)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select * from Hishop_Promotions WHERE DateDiff(DD, StartDate, getdate()) >= 0 AND DateDiff(DD, EndDate, getdate()) <= 0 ");
            builder.Append("and ActivityId=(select ActivityId from dbo.Hishop_PromotionProducts where productid=@productid)");
            builder.Append(" AND ActivityId IN (SELECT ActivityId FROM Hishop_PromotionMemberGrades WHERE GradeId = @GradeId)");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "productid", DbType.Int32, productid);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, member.GradeId);
            PromotionInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePromote(reader);
                }
            }
            return info;
        }

        public bool UpdateBundlingProduct(BundlingInfo bind, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_BundlingProducts  SET Name=@Name,ShortDescription=@ShortDescription,Num=@Num,Price=@Price,SaleStatus=@SaleStatus,AddTime=@AddTime WHERE BundlingID=@BundlingID");
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, bind.Name);
            this.database.AddInParameter(sqlStringCommand, "BundlingID", DbType.Int32, bind.BundlingID);
            this.database.AddInParameter(sqlStringCommand, "ShortDescription", DbType.String, bind.ShortDescription);
            this.database.AddInParameter(sqlStringCommand, "Num", DbType.Int32, bind.Num);
            this.database.AddInParameter(sqlStringCommand, "Price", DbType.Currency, bind.Price);
            this.database.AddInParameter(sqlStringCommand, "SaleStatus", DbType.Int32, bind.SaleStatus);
            this.database.AddInParameter(sqlStringCommand, "AddTime", DbType.DateTime, bind.AddTime);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }
         */
    }
}

