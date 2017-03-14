namespace Hidistro.SqlDal.Promotions
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Promotions;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class OrderGiftDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddOrderGift(string orderId, OrderGiftInfo gift, int quantity, DbTransaction dbTran)
        {
            //先查是否存在订单
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Hishop_OrderGifts where OrderId=@OrderId AND GiftId=@GiftId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, gift.GiftId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                //若存在,则update
                if (reader.Read())
                {
                    DbCommand command2 = this.database.GetSqlStringCommand("update Hishop_OrderGifts set Quantity=@Quantity where OrderId=@OrderId AND GiftId=@GiftId");
                    this.database.AddInParameter(command2, "OrderId", DbType.String, orderId);
                    this.database.AddInParameter(command2, "GiftId", DbType.Int32, gift.GiftId);
                    this.database.AddInParameter(command2, "Quantity", DbType.Int32, ((int)reader["Quantity"]) + quantity);
                    if (dbTran != null)
                    {
                        return (this.database.ExecuteNonQuery(command2, dbTran) == 1);
                    }
                    return (this.database.ExecuteNonQuery(command2) == 1);
                }
                //否则insert
                DbCommand command = this.database.GetSqlStringCommand("INSERT INTO Hishop_OrderGifts(OrderId,GiftId,GiftName,CostPrice,costPoint,ThumbnailsUrl,Quantity,PromoType) VALUES(@OrderId,@GiftId,@GiftName,@CostPrice,@costPoint,@ThumbnailsUrl,@Quantity,@PromoType)");
                this.database.AddInParameter(command, "OrderId", DbType.String, orderId);
                this.database.AddInParameter(command, "GiftId", DbType.Int32, gift.GiftId);
                this.database.AddInParameter(command, "GiftName", DbType.String, gift.GiftName);
                this.database.AddInParameter(command, "CostPrice", DbType.Currency, gift.CostPrice);
                this.database.AddInParameter(command, "costPoint", DbType.Currency, gift.costPoint);
                this.database.AddInParameter(command, "ThumbnailsUrl", DbType.String, gift.ThumbnailsUrl);
                this.database.AddInParameter(command, "Quantity", DbType.Int32, gift.Quantity);
                this.database.AddInParameter(command, "PromoType", DbType.Int16, gift.PromoteType);
                if (dbTran != null)
                {
                    return (this.database.ExecuteNonQuery(command, dbTran) == 1);
                }
                return (this.database.ExecuteNonQuery(command) == 1);
            }
        }

        public bool ClearOrderGifts(string orderId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_OrderGifts WHERE OrderId =@OrderId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public bool DeleteOrderGift(string orderId, int giftId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_OrderGifts WHERE OrderId=@OrderId AND GiftId=@GiftId ");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public IList<GiftInfo> GetGiftList(GiftQuery query)
        {
            IList<GiftInfo> list = new List<GiftInfo>();
            string str = string.Format("SELECT * FROM Hishop_Gifts WHERE [Name] LIKE '%{0}%'", DataHelper.CleanSearchString(query.Name));
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateGift(reader));
                }
            }
            return list;
        }

        public DbQueryResult GetGifts(GiftQuery query)
        {
            string filter = null;
            if (!string.IsNullOrEmpty(query.Name))
            {
                filter = string.Format("[Name] LIKE '%{0}%'", DataHelper.CleanSearchString(query.Name));
            }
            Pagination page = query.Page;
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Gifts", "GiftId", filter, "*");
        }

        public OrderGiftInfo GetOrderGift(int giftId, string orderId)
        {
            OrderGiftInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_OrderGifts WHERE OrderId=@OrderId AND GiftId=@GiftId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateOrderGift(reader);
                }
            }
            return info;
        }

        public DataTable GetOrderGiftsThumbnailsUrl(string orderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ThumbnailsUrl,GiftName ,GiftId,costPoint FROM Hishop_OrderGifts WHERE OrderId=@OrderId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DbQueryResult GetOrderGifts(OrderGiftQuery query)
        {
            DbQueryResult result = new DbQueryResult();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("select top {0} * from Hishop_OrderGifts where OrderId=@OrderId", query.Page.PageSize);
            if (query.Page.PageIndex == 1)
            {
                builder.Append(" ORDER BY GiftId ASC");
            }
            else
            {
                builder.AppendFormat(" and GiftId > (select max(GiftId) from (select top {0} GiftId from Hishop_OrderGifts where 0=0 and OrderId=@OrderId ORDER BY GiftId ASC ) as tbltemp) ORDER BY GiftId ASC", (query.Page.PageIndex - 1) * query.Page.PageSize);
            }
            if (query.Page.IsCount)
            {
                builder.AppendFormat(";select count(GiftId) as Total from Hishop_OrderGifts where OrderId=@OrderId", new object[0]);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, query.OrderId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                result.Data = DataHelper.ConverDataReaderToDataTable(reader);
                if (query.Page.IsCount && reader.NextResult())
                {
                    reader.Read();
                    result.TotalRecords = reader.GetInt32(0);
                }
            }
            return result;
        }
    }
}

