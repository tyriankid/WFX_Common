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
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public class ReturnDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AgreedReturns(int returnsId, decimal refundMoney, string adminRemark, string OrderId, string skuId = "", string AdminShipAddress = "")
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
            builder.Append("UPDATE Hishop_OrderReturns SET HandleStatus = @HandleStatus,HandleTime = @HandleTime ,AdminRemark = @AdminRemark,RefundMoney = @RefundMoney,AdminShipAddress = @AdminShipAddress WHERE ReturnsId = @ReturnsId;");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 3);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
            this.database.AddInParameter(sqlStringCommand, "RefundMoney", DbType.Decimal, refundMoney);
            this.database.AddInParameter(sqlStringCommand, "ReturnsId", DbType.Int32, returnsId);
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, OrderStatus.MerchantsAgreedFoReturns);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
            this.database.AddInParameter(sqlStringCommand, "AdminShipAddress", DbType.String, AdminShipAddress);
            this.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 0x15);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool ApplyForReturn(string orderId, string remark, int refundType, string skuId, int Quantity, string GateWay, string GateWayOrderId, string ShopName, decimal RefundMoney, int StoreId, string ReturnReason, string UserCredentials)
        {
            StringBuilder builder = new StringBuilder();
            if (string.IsNullOrEmpty(skuId))
            {
                builder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus,RefundAmount = @RefundAmount WHERE OrderId = @OrderId;");
            }
            else
            {
                builder.Append("UPDATE Hishop_OrderItems SET RefundAmount = @RefundAmount,RealTotalPrice = (ItemAdjustedPrice * Quantity - @RefundAmount),Status = @ItemStatus WHERE OrderId = @OrderId AND SkuId = @SkuId;");
                builder.Append("DELETE FROM Hishop_OrderReturns WHERE OrderId = @OrderId AND SkuId = @SkuId;");
            }
            builder.Append(" INSERT INTO Hishop_OrderReturns(OrderId,ApplyForTime,Comments,HandleStatus,RefundType,RefundMoney,SkuId,Quantity,RefundOrderId,RefundGateWay,ShopName,StoreId,ReturnReason,UserCredentials)values(@OrderId,@ApplyForTime,@Comments,0,@RefundType,@RefundAmount,@SkuId,@Quantity,@RefundOrderId,@RefundGateWay,@ShopName,@StoreId,@ReturnReason,@UserCredentials);");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 7);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "ApplyForTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "Comments", DbType.String, remark);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, Quantity);
            this.database.AddInParameter(sqlStringCommand, "RefundOrderId", DbType.String, GateWayOrderId);
            this.database.AddInParameter(sqlStringCommand, "RefundGateWay", DbType.String, GateWay);
            this.database.AddInParameter(sqlStringCommand, "RefundAmount", DbType.Decimal, RefundMoney);
            this.database.AddInParameter(sqlStringCommand, "ShopName", DbType.String, ShopName);
            this.database.AddInParameter(sqlStringCommand, "RefundType", DbType.Int32, refundType);
            this.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, StoreId);
            this.database.AddInParameter(sqlStringCommand, "ReturnReason", DbType.String, ReturnReason);
            this.database.AddInParameter(sqlStringCommand, "UserCredentials", DbType.String, UserCredentials);
            this.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, LineItemStatus.ReturnApplied);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool CheckReturn(ReturnsInfo returns, bool accept, [Optional, DecimalConstant(0, 0, (uint) 0, (uint) 0, (uint) 1)] decimal PointsRate, [Optional, DecimalConstant(0, 0, (uint) 0, (uint) 0, (uint) 0)] decimal RefundAmount, OrderInfo order = null)
        {
            //if (order == null)
            //{
            //    order = new OrderDao().GetOrderInfo(returns.OrderId);
            //    if (order == null)
            //    {
            //        return false;
            //    }
            //}
            //if (RefundAmount <= 0M)
            //{
            //    RefundAmount = returns.RefundMoney;
            //}
            //decimal num = (int) (returns.RefundMoney / PointsRate);
            //decimal num2 = RefundAmount;
            //StringBuilder builder = new StringBuilder();
            //if (!accept)
            //{
            //    if (string.IsNullOrEmpty(returns.SkuId))
            //    {
            //        builder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus  WHERE OrderId = @OrderId;");
            //    }
            //    else
            //    {
            //        builder.Append("UPDATE Hishop_OrderItems SET Status = @ItemStatus WHERE OrderId = @OrderId AND SkuId = @SkuId;");
            //    }
            //    builder.Append("UPDATE Hishop_OrderReturns set Operator=@Operator, AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime,RefundMoney=@RefundMoney WHERE  OrderId = @OrderId");
            //    if (!(string.IsNullOrEmpty(returns.SkuId) && !(returns.SkuId != "")))
            //    {
            //        builder.Append(" AND SkuId = @SkuId");
            //    }
            //    else
            //    {
            //        builder.Append(" AND (SkuId='' OR SkuId IS NULL)");
            //    }
            //    builder.Append(";");
            //}
            //else
            //{
            //    builder.AppendFormat("UPDATE Hishop_Orders SET FinishDate = CASE WHEN FinishDate IS NULL THEN GETDATE() ELSE FinishDate END WHERE OrderId = @OrderId AND OrderStatus = {0};", 5);
            //    if (string.IsNullOrEmpty(returns.SkuId))
            //    {
            //        builder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus,RefundAmount = @OrderRefundMoney WHERE OrderId = @OrderId;");
            //    }
            //    else
            //    {
            //        bool flag = true;
            //        foreach (LineItemInfo info in order.LineItems.Values)
            //        {
            //            if ((info.Status == LineItemStatus.Refunded) || (info.Status == LineItemStatus.Returned))
            //            {
            //                if ((info.RefundInfo != null) && (info.Status == LineItemStatus.Refunded))
            //                {
            //                    num2 += info.RefundInfo.RefundAmount;
            //                }
            //                if ((info.ReturnInfo != null) && (info.Status == LineItemStatus.Returned))
            //                {
            //                    num2 += info.ReturnInfo.RefundMoney;
            //                }
            //            }
            //            if ((info.SkuId != returns.SkuId) && (info.Status != LineItemStatus.Returned))
            //            {
            //                flag = false;
            //            }
            //        }
            //        if (flag)
            //        {
            //            builder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
            //        }
            //        builder.Append("UPDATE Hishop_Orders SET RefundAmount = @OrderRefundMoney WHERE OrderId = @OrderId;");
            //        builder.Append("UPDATE Hishop_OrderItems SET RefundAmount = @RefundMoney, Status = @ItemStatus, RealTotalPrice = (ItemAdjustedPrice * Quantity - @RefundMoney) WHERE OrderId = @OrderId AND SkuId = @SkuId;");
            //    }
            //    builder.Append(" UPDATE Hishop_OrderReturns SET Operator = @Operator, AdminRemark = @AdminRemark,HandleStatus = @HandleStatus,HandleTime = @HandleTime,RefundMoney = @RefundMoney where OrderId = @OrderId");
            //    if (!string.IsNullOrEmpty(returns.SkuId))
            //    {
            //        builder.Append(" AND SkuId = @SkuId");
            //    }
            //    else
            //    {
            //        builder.Append(" AND (SkuId='' OR SkuId IS NULL)");
            //    }
            //    builder.Append(";");
            //    if ((returns.RefundType == RefundTypes.InBalance) && accept)
            //    {
            //        string orderId = returns.OrderId;
            //        if (!string.IsNullOrEmpty(returns.SkuId))
            //        {
            //            if (!order.LineItems.ContainsKey(returns.SkuId))
            //            {
            //                return false;
            //            }
            //            orderId = returns.OrderId + " 商品 " + order.LineItems[returns.SkuId].ItemDescription + " ";
            //        }
            //        builder.Append(" INSERT INTO Hishop_BalanceDetails(UserId,UserName,TradeDate,TradeType,Income");
            //        builder.AppendFormat(",Balance,Remark) select UserId,Username,getdate() as TradeDate,{0} as TradeType,", 7);
            //        builder.Append("@RefundMoney as Income,ISNULL((SELECT TOP 1 Balance from Hishop_BalanceDetails b");
            //        builder.AppendFormat(" WHERE b.UserId=a.UserId order by JournalNumber desc),0)+@RefundMoney as Balance,'订单{0}退货' as Remark ", orderId);
            //        builder.Append("FROM Hishop_Orders a WHERE OrderId = @OrderId;");
            //    }
            //}
            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            //if (accept)
            //{
            //    this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 10);
            //    this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
            //    this.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 0x18);
            //}
            //else
            //{
            //    this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 3);
            //    this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 2);
            //    this.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 0x19);
            //}
            //this.database.AddInParameter(sqlStringCommand, "OrderRefundMoney", DbType.Decimal, num2);
            //this.database.AddInParameter(sqlStringCommand, "RefundPoint", DbType.Int32, num);
            //this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, returns.OrderId);
            //this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, returns.Operator);
            //this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, returns.AdminRemark);
            //this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, returns.HandleTime);
            //this.database.AddInParameter(sqlStringCommand, "RefundMoney", DbType.Decimal, RefundAmount);
            //this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, returns.SkuId);
            //return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            return false;
        }

        public bool CheckReturn(string orderId, string Operator, decimal refundMoney, string adminRemark, int refundType, bool accept,  [Optional, DecimalConstant(0, 0, (uint) 0, (uint) 0, (uint) 1)] decimal PointsRate,string SkuId = "", OrderInfo order = null)
        {
            //ReturnsInfo returns = new ReturnsInfo {
            //    OrderId = orderId,
            //    Operator = Operator,
            //    RefundMoney = refundMoney,
            //    AdminRemark = adminRemark,
            //    RefundType = (RefundTypes) refundType,
            //    SkuId = SkuId,
            //    HandleTime = DateTime.Now
            //};
            //return this.CheckReturn(returns, accept,  PointsRate, refundMoney ,order);
            return false;
        }

        public bool DelReturnsApply(int returnsId)
        {
            string query = string.Format("DELETE FROM Hishop_OrderReturns WHERE ReturnsId={0} and (HandleStatus ={1} OR HandleStatus = {2}) ", returnsId, 2, 1);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool FinishGetGoods(int ReturnsId, string AdminRemark, string OrderId, string skuId = "")
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
            builder.Append("UPDATE Hishop_OrderReturns SET HandleStatus = @HandleStatus,HandleTime = @HandleTime ,AdminRemark = @AdminRemark WHERE ReturnsId = @ReturnsId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 5);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, AdminRemark);
            this.database.AddInParameter(sqlStringCommand, "ReturnsId", DbType.Int32, ReturnsId);
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, OrderStatus.GetGoodsForReturns);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
            this.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 0x17);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        //public bool FinishReturn(ReturnsInfo returns, decimal RefundMoney, decimal PointsRate)
        //{
        //    if (returns == null)
        //    {
        //        return false;
        //    }
        //    OrderInfo orderInfo = new OrderDao().GetOrderInfo(returns.OrderId);
        //    return this.CheckReturn(returns, true,  PointsRate, 0M,orderInfo);
        //}

        public bool FinishReturn(int ReturnsId, string AdminRemark, string OrderId, string skuId = "")
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
            builder.AppendFormat("UPDATE Hishop_Orders SET FinishDate = CASE WHEN FinishDate IS NULL THEN GETDATE() ELSE FinishDate END WHERE OrderId = @OrderId AND OrderStatus = {0};", 5);
            builder.Append("UPDATE Hishop_OrderReturns SET HandleStatus = @HandleStatus,HandleTime = @HandleTime ,AdminRemark = @AdminRemark WHERE ReturnsId = @ReturnsId;");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, AdminRemark);
            this.database.AddInParameter(sqlStringCommand, "ReturnsId", DbType.Int32, ReturnsId);
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, OrderStatus.Returned);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
            this.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 0x18);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public decimal GetRefundMoney(OrderInfo order, out decimal refundMoney, string SkuId = "")
        {
            decimal num = 0M;
            string query = "SELECT RefundMoney FROM dbo.Hishop_OrderReturns WHERE HandleStatus=1 AND OrderId = @OrderId";
            if (!string.IsNullOrEmpty(SkuId))
            {
                query = query + " AND SkuId = @SkuId";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
            num = Convert.ToDecimal(this.database.ExecuteScalar(sqlStringCommand));
            return (refundMoney = num);
        }

        public void GetRefundTypeFromReturn(string orderId, out int refundType, out string remark, string SkuId = "")
        {
            refundType = 0;
            remark = "";
            string query = "SELECT RefundType,Comments FROM Hishop_OrderReturns WHERE HandleStatus=0 AND OrderId = @OrderId";
            if (!string.IsNullOrEmpty(SkuId))
            {
                query = query + " AND SkuId = @SkuId";
            }
            else
            {
                query = query + " AND (SkuId='' OR SkuId IS NULL)";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
            IDataReader reader = this.database.ExecuteReader(sqlStringCommand);
            if (reader.Read())
            {
                refundType = (int) reader["RefundType"];
                remark = (string) reader["Comments"];
            }
        }

        //public ReturnsInfo GetReturnInfo(int ReturnsId)
        //{
        //    StringBuilder builder = new StringBuilder();
        //    builder.Append("SELECT * FROM Hishop_OrderReturns WHERE ReturnsId = @ReturnsId");
        //    DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
        //    this.database.AddInParameter(sqlStringCommand, "ReturnsId", DbType.Int32, ReturnsId);
        //    using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
        //    {
        //        return ReaderConvert.ReaderToModel<ReturnsInfo>(reader);
        //    }
        //}

        //public ReturnsInfo GetReturnInfo(string OrderId, string SkuId = "")
        //{
        //    StringBuilder builder = new StringBuilder();
        //    builder.Append("SELECT TOP 1 * FROM Hishop_OrderReturns WHERE OrderId = @OrderId");
        //    if (!string.IsNullOrEmpty(SkuId))
        //    {
        //        builder.Append(" AND SkuId = @SkuID");
        //    }
        //    else
        //    {
        //        builder.Append(" AND (SkuId IS NULL OR SkuId='')");
        //    }
        //    builder.Append(" ORDER BY ReturnsId DESC");
        //    DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
        //    this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
        //    this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
        //    using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
        //    {
        //        return ReaderConvert.ReaderToModel<ReturnsInfo>(reader);
        //    }
        //}

        //public ReturnsInfo GetReturnInfoOfRefundOrderId(string RefundOrderId)
        //{
        //    StringBuilder builder = new StringBuilder();
        //    builder.Append("SELECT TOP 1 * FROM Hishop_OrderReturns WHERE RefundOrderId = @RefundOrderId");
        //    builder.Append(" ORDER BY RefundId DESC");
        //    DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
        //    this.database.AddInParameter(sqlStringCommand, "RefundOrderId", DbType.String, RefundOrderId);
        //    using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
        //    {
        //        return ReaderConvert.ReaderToModel<ReturnsInfo>(reader);
        //    }
        //}

        //public DbQueryResult GetReturnsApplys(ReturnsApplyQuery query)
        //{
        //    int? nullable;
        //    StringBuilder builder = new StringBuilder();
        //    builder.Append(" 1=1");
        //    if (!string.IsNullOrEmpty(query.OrderId))
        //    {
        //        builder.AppendFormat(" AND OrderId = '{0}'", query.OrderId);
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
        //    return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_OrderReturns", "ReturnsId", builder.ToString(), "*");
        //}

        public DataTable GetReturnsApplysTable(int returnsId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Hishop_OrderReturns where ReturnsId=@ReturnsId");
            this.database.AddInParameter(sqlStringCommand, "ReturnsId", DbType.String, returnsId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public bool UserSendGoods(int ReturnsId, string ExpressCompanyAbb, string ExpressCompanyName, string ShipOrderNumber, string OrderId, string skuId = "")
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
            builder.Append("UPDATE Hishop_OrderReturns SET HandleStatus = @HandleStatus,HandleTime = @HandleTime ,ExpressCompanyAbb = @ExpressCompanyAbb,ExpressCompanyName = @ExpressCompanyName,ShipOrderNumber = @ShipOrderNumber WHERE ReturnsId = @ReturnsId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 4);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, ExpressCompanyAbb);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, ExpressCompanyName);
            this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, ShipOrderNumber);
            this.database.AddInParameter(sqlStringCommand, "ReturnsId", DbType.Int32, ReturnsId);
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, OrderStatus.DeliveryingForReturns);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
            this.database.AddInParameter(sqlStringCommand, "ItemStatus", DbType.Int32, 0x16);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

