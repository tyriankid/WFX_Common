namespace Hidistro.SqlDal.Orders
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Orders;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class ReplaceDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AgreedReplace(int ReplaceId, string AdminRemark, string OrderId, string skuId, string AdminShipAddress = "")
        {
            StringBuilder builder = new StringBuilder();
            if (string.IsNullOrEmpty(skuId))
            {
                builder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
            }
            else
            {
                builder.Append("UPDATE Hishop_OrderItems SET Status = @ItemStatus WHERE OrderId = @OrderId AND SkuId = @SkuId;");
            }
            builder.Append("UPDATE Hishop_OrderReplace SET AdminRemark = @AdminRemark,HandleStatus = @HandleStatus,HandleTime = @HandleTime,AdminShipAddress = @AdminShipAddress WHERE ReplaceId = @ReplaceId ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 3);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, AdminRemark);
            this.database.AddInParameter(sqlStringCommand, "ReplaceId", DbType.Int32, ReplaceId);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, OrderStatus.MerchantsAgreedForReplace);
            this.database.AddInParameter(sqlStringCommand, "AdminShipAddress", DbType.String, AdminShipAddress);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 0x1f);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool ApplyForReplace(string orderId, string remark, string skuId, int quantity, string ShopName, int StoreId, string ReplaceReason, string UserCredentials)
        {
            StringBuilder builder = new StringBuilder();
            if (string.IsNullOrEmpty(skuId))
            {
                builder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
            }
            else
            {
                builder.Append("UPDATE Hishop_OrderItems SET Status = @ItemStatus WHERE OrderId = @OrderId AND SkuId = @SkuId;");
                builder.Append("DELETE FROM Hishop_OrderReplace WHERE OrderId = @OrderId AND SkuId = @SkuId;");
            }
            builder.Append(" INSERT INTO Hishop_OrderReplace(OrderId,ApplyForTime,Comments,HandleStatus,SkuId,Quantity,ShopName,StoreId,ReplaceReason,UserCredentials) VALUES(@OrderId,@ApplyForTime,@Comments,0,@SkuId,@Quantity,@ShopName,@StoreId,@ReplaceReason,@UserCredentials);");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 8);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "ApplyForTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "Comments", DbType.String, remark);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(sqlStringCommand, "ShopName", DbType.String, ShopName);
            this.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, StoreId);
            this.database.AddInParameter(sqlStringCommand, "ReplaceReason", DbType.String, ReplaceReason);
            this.database.AddInParameter(sqlStringCommand, "UserCredentials", DbType.String, UserCredentials);
            this.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 30);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool CheckReplace(string orderId, string adminRemark, bool accept, string SkuId = "", string AdminShipAddress = "")
        {
            StringBuilder builder = new StringBuilder();
            if (string.IsNullOrEmpty(SkuId))
            {
                builder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
            }
            else
            {
                builder.Append("UPDATE Hishop_OrderItems SET Status = @ItemStatus WHERE OrderId = @OrderId AND SkuId = @SkuId;");
            }
            builder.Append("UPDATE Hishop_OrderReplace SET AdminRemark=@AdminRemark,HandleStatus = @HandleStatus,HandleTime=@HandleTime,AdminShipAddress = @AdminShipAddress WHERE HandleStatus=0 AND OrderId = @OrderId ");
            if (!string.IsNullOrEmpty(SkuId))
            {
                builder.Append(" AND SkuId = @SkuId");
            }
            else
            {
                builder.Append(" AND (SkuId='' OR SkuId IS NULL)");
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            if (accept)
            {
                builder.AppendFormat("UPDATE Hishop_Orders SET FinishDate = CASE WHEN FinishDate IS NULL THEN GETDATE() ELSE FinishDate END WHERE OrderId = @OrderId AND OrderStatus = {0};", 5);
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 15);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, ReplaceStatus.MerchantsAgreed);
                this.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 0x1f);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 3);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, ReplaceStatus.Refused);
                this.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 0x23);
            }
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
            this.database.AddInParameter(sqlStringCommand, "AdminShipAddress", DbType.String, AdminShipAddress);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DelReplaceApply(int replaceId)
        {
            string query = string.Format("DELETE FROM Hishop_OrderReplace WHERE ReplaceId={0} AND (HandleStatus = {1} OR HandleStatus = {1}) ", replaceId, 2, 1);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool FinishReplace(int ReplaceId, string AdminRemark, string OrderId, string skuId = "")
        {
            OrderInfo orderInfo = new OrderDao().GetOrderInfo(OrderId);
            if (orderInfo == null)
            {
                return false;
            }
            StringBuilder builder = new StringBuilder();
            if (string.IsNullOrEmpty(skuId))
            {
                builder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
            }
            else
            {
                builder.Append("UPDATE Hishop_OrderItems SET Status = @ItemStatus WHERE OrderId = @OrderId AND SkuId = @SkuId;");
                bool flag = true;
                foreach (LineItemInfo info2 in orderInfo.LineItems.Values)
                {
                    if ((info2.SkuId != skuId) && (info2.Status != LineItemStatus.Returned))
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    builder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
                }
            }
            builder.Append("UPDATE Hishop_OrderReplace SET HandleStatus = @HandleStatus,HandleTime = @HandleTime ,AdminRemark = @AdminRemark WHERE ReplaceId = @ReplaceId;");
            builder.AppendFormat("UPDATE Hishop_Orders SET FinishDate = CASE WHEN FinishDate IS NULL THEN GETDATE() ELSE FinishDate END WHERE OrderId = @OrderId AND OrderStatus = {0};", 5);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, AdminRemark);
            this.database.AddInParameter(sqlStringCommand, "ReplaceId", DbType.Int32, ReplaceId);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, OrderStatus.Replaced);
            this.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 0x22);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        //public DbQueryResult GetReplaceApplys(ReplaceApplyQuery query)
        //{
        //    int? nullable;
        //    StringBuilder builder = new StringBuilder();
        //    builder.Append(" 1=1");
        //    if (!string.IsNullOrEmpty(query.OrderId))
        //    {
        //        builder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
        //    }
        //    if (query.HandleStatus.HasValue)
        //    {
        //        builder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
        //    }
        //    if (query.UserId.HasValue)
        //    {
        //        builder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
        //    }
        //    if (query.StoreId.HasValue && (((nullable = query.StoreId).GetValueOrDefault() > 0) && nullable.HasValue))
        //    {
        //        builder.AppendFormat(" AND StoreId = {0}", query.StoreId.Value);
        //    }
        //    return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_OrderReplace", "ReplaceId", builder.ToString(), "*");
        //}

        public DataTable GetReplaceApplysTable(int replaceId, int StoreId = 0)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_OrderReplace WHERE ReplaceId=@ReplaceId");
            this.database.AddInParameter(sqlStringCommand, "ReplaceId", DbType.String, replaceId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public string GetReplaceComments(string orderId, string SkuId = "")
        {
            string query = "SELECT Comments FROM Hishop_OrderReplace WHERE HandleStatus=0 AND OrderId = @OrderId";
            if (!string.IsNullOrEmpty(SkuId))
            {
                query = query + " AND SkuId = @SkuId";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 == null) || (obj2 is DBNull))
            {
                return "";
            }
            return obj2.ToString();
        }

        public ReplaceInfo GetReplaceInfo(int ReplaceId)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT * FROM Hishop_OrderReplace WHERE ReplaceId = @ReplaceId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ReplaceId", DbType.Int32, ReplaceId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<ReplaceInfo>(reader);
            }
        }

        public ReplaceInfo GetReplaceInfo(string OrderId, string SkuId = "")
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT TOP 1 * FROM Hishop_OrderReplace WHERE OrderId = @OrderId");
            if (!string.IsNullOrEmpty(SkuId))
            {
                builder.Append(" AND SkuId = @SkuID");
            }
            else
            {
                builder.Append(" AND (SkuId IS NULL OR SkuId='')");
            }
            builder.Append(" ORDER BY ReplaceId DESC");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<ReplaceInfo>(reader);
            }
        }

        public bool ShopSendGoods(int ReplaceId, string ExpressCompanyAbb, string ExpressCompanyName, string ShipOrderNumber, string OrderId, string SkuId = "")
        {
            StringBuilder builder = new StringBuilder();
            if (string.IsNullOrEmpty(SkuId))
            {
                builder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
            }
            else
            {
                builder.Append("UPDATE Hishop_OrderItems SET Status = @ItemStatus WHERE OrderId = @OrderId AND SkuId = @SkuId;");
            }
            builder.Append("UPDATE Hishop_OrderReplace SET HandleStatus = @HandleStatus,HandleTime = @HandleTime ,ExpressCompanyAbb = @ExpressCompanyAbb,ExpressCompanyName = @ExpressCompanyName,ShipOrderNumber = @ShipOrderNumber WHERE ReplaceId = @ReplaceId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 6);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, ExpressCompanyAbb);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, ExpressCompanyName);
            this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, ShipOrderNumber);
            this.database.AddInParameter(sqlStringCommand, "ReplaceId", DbType.Int32, ReplaceId);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, OrderStatus.MerchantsDeliveryForReplace);
            this.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 0x21);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UserSendGoods(int ReplaceId, string ExpressCompanyAbb, string ExpressCompanyName, string ShipOrderNumber, string OrderId, string SkuId = "")
        {
            StringBuilder builder = new StringBuilder();
            if (string.IsNullOrEmpty(SkuId))
            {
                builder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
            }
            else
            {
                builder.Append("UPDATE Hishop_OrderItems SET Status = @ItemStatus WHERE OrderId = @OrderId AND SkuId = @SkuId;");
            }
            builder.Append("UPDATE Hishop_OrderReplace SET HandleStatus = @HandleStatus,HandleTime = @HandleTime ,UserExpressCompanyAbb = @UserExpressCompanyAbb,UserExpressCompanyName = @UserExpressCompanyName,UserShipOrderNumber = @UserShipOrderNumber WHERE ReplaceId = @ReplaceId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 4);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "UserExpressCompanyAbb", DbType.String, ExpressCompanyAbb);
            this.database.AddInParameter(sqlStringCommand, "UserExpressCompanyName", DbType.String, ExpressCompanyName);
            this.database.AddInParameter(sqlStringCommand, "UserShipOrderNumber", DbType.String, ShipOrderNumber);
            this.database.AddInParameter(sqlStringCommand, "ReplaceId", DbType.Int32, ReplaceId);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, OrderStatus.UserDeliveryForReplace);
            this.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 0x20);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

