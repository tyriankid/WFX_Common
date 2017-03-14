namespace Hidistro.SqlDal.Orders
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Store;
    using Hidistro.SqlDal.Promotions;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web;
    public class OrderDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool CheckRefund(string orderId, string Operator, string adminRemark, int refundType, bool accept)
        {

            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
            builder.Append(" update Hishop_OrderRefund set Operator=@Operator,AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime where HandleStatus=0 and OrderId = @OrderId;");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            if (accept)
            {
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 9);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 2);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 2);
            }
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, Operator);
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool CreatOrder(OrderInfo orderInfo, DbTransaction dbTran)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_CreateOrder");
            this.database.AddInParameter(storedProcCommand, "OrderId", DbType.String, orderInfo.OrderId);
            this.database.AddInParameter(storedProcCommand, "OrderDate", DbType.DateTime, orderInfo.OrderDate);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, orderInfo.UserId);
            this.database.AddInParameter(storedProcCommand, "UserName", DbType.String, orderInfo.Username);
            this.database.AddInParameter(storedProcCommand, "Wangwang", DbType.String, orderInfo.Wangwang);
            this.database.AddInParameter(storedProcCommand, "RealName", DbType.String, orderInfo.RealName);
            this.database.AddInParameter(storedProcCommand, "EmailAddress", DbType.String, orderInfo.EmailAddress);
            this.database.AddInParameter(storedProcCommand, "Remark", DbType.String, orderInfo.Remark);
            this.database.AddInParameter(storedProcCommand, "AdjustedDiscount", DbType.Currency, orderInfo.AdjustedDiscount);
            this.database.AddInParameter(storedProcCommand, "OrderStatus", DbType.Int32, (int)orderInfo.OrderStatus);
            this.database.AddInParameter(storedProcCommand, "ShippingRegion", DbType.String, orderInfo.ShippingRegion);
            this.database.AddInParameter(storedProcCommand, "Address", DbType.String, orderInfo.Address);
            this.database.AddInParameter(storedProcCommand, "ZipCode", DbType.String, orderInfo.ZipCode);
            this.database.AddInParameter(storedProcCommand, "ShipTo", DbType.String, orderInfo.ShipTo);
            this.database.AddInParameter(storedProcCommand, "TelPhone", DbType.String, orderInfo.TelPhone);
            this.database.AddInParameter(storedProcCommand, "CellPhone", DbType.String, orderInfo.CellPhone);
            this.database.AddInParameter(storedProcCommand, "ShipToDate", DbType.String, orderInfo.ShipToDate);
            this.database.AddInParameter(storedProcCommand, "ShippingModeId", DbType.Int32, orderInfo.ShippingModeId);
            this.database.AddInParameter(storedProcCommand, "ModeName", DbType.String, orderInfo.ModeName);
            this.database.AddInParameter(storedProcCommand, "RegionId", DbType.Int32, orderInfo.RegionId);
            this.database.AddInParameter(storedProcCommand, "Freight", DbType.Currency, orderInfo.Freight);
            this.database.AddInParameter(storedProcCommand, "AdjustedFreight", DbType.Currency, orderInfo.AdjustedFreight);
            this.database.AddInParameter(storedProcCommand, "ShipOrderNumber", DbType.String, orderInfo.ShipOrderNumber);
            this.database.AddInParameter(storedProcCommand, "Weight", DbType.Int32, orderInfo.Weight);
            this.database.AddInParameter(storedProcCommand, "ExpressCompanyName", DbType.String, orderInfo.ExpressCompanyName);
            this.database.AddInParameter(storedProcCommand, "ExpressCompanyAbb", DbType.String, orderInfo.ExpressCompanyAbb);
            this.database.AddInParameter(storedProcCommand, "PaymentTypeId", DbType.Int32, orderInfo.PaymentTypeId);
            this.database.AddInParameter(storedProcCommand, "PaymentType", DbType.String, orderInfo.PaymentType);
            this.database.AddInParameter(storedProcCommand, "PayCharge", DbType.Currency, orderInfo.PayCharge);
            this.database.AddInParameter(storedProcCommand, "RefundStatus", DbType.Int32, (int)orderInfo.RefundStatus);
            this.database.AddInParameter(storedProcCommand, "Gateway", DbType.String, orderInfo.Gateway);
            if (orderInfo.OrderSource == 1)
            {
                //代理商订单总额
                this.database.AddInParameter(storedProcCommand, "OrderTotal", DbType.Currency, orderInfo.GetCostPrice() + orderInfo.AdjustedFreight);//取成本价+快递费
            }
            else
            {
                decimal t = orderInfo.GetTotal();
                this.database.AddInParameter(storedProcCommand, "OrderTotal", DbType.Currency, t);
            }

            this.database.AddInParameter(storedProcCommand, "OrderPoint", DbType.Int32, orderInfo.Points);
            this.database.AddInParameter(storedProcCommand, "OrderCostPrice", DbType.Currency, orderInfo.GetCostPrice());
            this.database.AddInParameter(storedProcCommand, "OrderProfit", DbType.Currency, orderInfo.GetProfit());
            this.database.AddInParameter(storedProcCommand, "Amount", DbType.Currency, orderInfo.GetAmount());
            this.database.AddInParameter(storedProcCommand, "ReducedPromotionId", DbType.Int32, orderInfo.ReducedPromotionId);
            this.database.AddInParameter(storedProcCommand, "ReducedPromotionName", DbType.String, orderInfo.ReducedPromotionName);
            this.database.AddInParameter(storedProcCommand, "ReducedPromotionAmount", DbType.Currency, orderInfo.ReducedPromotionAmount);
            this.database.AddInParameter(storedProcCommand, "IsReduced", DbType.Boolean, orderInfo.IsReduced);
            this.database.AddInParameter(storedProcCommand, "SentTimesPointPromotionId", DbType.Int32, orderInfo.SentTimesPointPromotionId);
            this.database.AddInParameter(storedProcCommand, "SentTimesPointPromotionName", DbType.String, orderInfo.SentTimesPointPromotionName);
            this.database.AddInParameter(storedProcCommand, "TimesPoint", DbType.Currency, orderInfo.TimesPoint);
            this.database.AddInParameter(storedProcCommand, "IsSendTimesPoint", DbType.Boolean, orderInfo.IsSendTimesPoint);
            this.database.AddInParameter(storedProcCommand, "FreightFreePromotionId", DbType.Int32, orderInfo.FreightFreePromotionId);
            this.database.AddInParameter(storedProcCommand, "FreightFreePromotionName", DbType.String, orderInfo.FreightFreePromotionName);
            this.database.AddInParameter(storedProcCommand, "IsFreightFree", DbType.Boolean, orderInfo.IsFreightFree);
            this.database.AddInParameter(storedProcCommand, "CouponName", DbType.String, orderInfo.CouponName);
            this.database.AddInParameter(storedProcCommand, "CouponCode", DbType.String, orderInfo.CouponCode);
            this.database.AddInParameter(storedProcCommand, "CouponAmount", DbType.Currency, orderInfo.CouponAmount);
            this.database.AddInParameter(storedProcCommand, "CouponValue", DbType.Currency, orderInfo.CouponValue);
            this.database.AddInParameter(storedProcCommand, "RedPagerActivityName", DbType.String, orderInfo.RedPagerActivityName);
            this.database.AddInParameter(storedProcCommand, "RedPagerID", DbType.String, orderInfo.RedPagerID);
            this.database.AddInParameter(storedProcCommand, "RedPagerOrderAmountCanUse", DbType.Currency, orderInfo.RedPagerOrderAmountCanUse);
            this.database.AddInParameter(storedProcCommand, "RedPagerAmount", DbType.Currency, orderInfo.RedPagerAmount);
            if (orderInfo.GroupBuyId > 0)
            {
                this.database.AddInParameter(storedProcCommand, "GroupBuyId", DbType.Int32, orderInfo.GroupBuyId);
                this.database.AddInParameter(storedProcCommand, "NeedPrice", DbType.Currency, orderInfo.NeedPrice);
                this.database.AddInParameter(storedProcCommand, "GroupBuyStatus", DbType.Int32, 1);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "GroupBuyId", DbType.Int32, DBNull.Value);
                this.database.AddInParameter(storedProcCommand, "NeedPrice", DbType.Currency, DBNull.Value);
                this.database.AddInParameter(storedProcCommand, "GroupBuyStatus", DbType.Int32, DBNull.Value);
            }
            if (orderInfo.CountDownBuyId > 0)//限时抢购
            {
                this.database.AddInParameter(storedProcCommand, "CountDownBuyId ", DbType.Int32, orderInfo.CountDownBuyId);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "CountDownBuyId ", DbType.Int32, DBNull.Value);
            }
            if (orderInfo.CutDownBuyId > 0)//砍价购买
            {
                this.database.AddInParameter(storedProcCommand, "CutDownBuyId ", DbType.Int32, orderInfo.CutDownBuyId);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "CutDownBuyId ", DbType.Int32, DBNull.Value);
            }
            if (orderInfo.BundlingID > 0)
            {
                this.database.AddInParameter(storedProcCommand, "BundlingID ", DbType.Int32, orderInfo.BundlingID);
                this.database.AddInParameter(storedProcCommand, "BundlingPrice", DbType.Currency, orderInfo.BundlingPrice);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "BundlingID ", DbType.Int32, DBNull.Value);
                this.database.AddInParameter(storedProcCommand, "BundlingPrice", DbType.Currency, DBNull.Value);
            }
            this.database.AddInParameter(storedProcCommand, "Tax", DbType.Currency, orderInfo.Tax);
            this.database.AddInParameter(storedProcCommand, "InvoiceTitle", DbType.String, orderInfo.InvoiceTitle);
            this.database.AddInParameter(storedProcCommand, "ReferralUserId", DbType.Int32, orderInfo.ReferralUserId);
            this.database.AddInParameter(storedProcCommand, "DiscountAmount", DbType.Decimal, orderInfo.DiscountAmount);
            this.database.AddInParameter(storedProcCommand, "ActivitiesId", DbType.String, orderInfo.ActivitiesId);
            this.database.AddInParameter(storedProcCommand, "ActivitiesName", DbType.String, orderInfo.ActivitiesName);
            this.database.AddInParameter(storedProcCommand, "FirstCommission", DbType.Decimal, orderInfo.FirstCommission);
            this.database.AddInParameter(storedProcCommand, "SecondCommission", DbType.Decimal, orderInfo.SecondCommission);
            this.database.AddInParameter(storedProcCommand, "ThirdCommission", DbType.Decimal, orderInfo.ThirdCommission);
            if (!string.IsNullOrEmpty(orderInfo.Sender))
                this.database.AddInParameter(storedProcCommand, "Sender", DbType.String, orderInfo.Sender);
            return (this.database.ExecuteNonQuery(storedProcCommand, dbTran) == 1);
        }

        public int DeleteOrders(string orderIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_Orders WHERE OrderId IN({0})", orderIds));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool EditOrderShipNumber(string orderId, string shipNumber)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET ShipOrderNumber=@ShipOrderNumber WHERE OrderId =@OrderId");
            this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, shipNumber);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public decimal GetCommossionByOrderId(string orderId, int userId)
        {
            string query = "select CommTotal from Hishop_Commissions WHERE OrderId=@OrderId AND UserId=@UserId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int16, userId);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 == null) || (obj2 is DBNull))
            {
                return 0M;
            }
            return (decimal)obj2;
        }

        /// <summary>
        /// 获取分销商的所有订单
        /// update@20150923 by hj:增加了对orderGifts表的信息获取
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataSet GetDistributorOrder(OrderQuery query)
        {
            string str = string.Empty;
            if (query.Status == OrderStatus.Finished)
            {
                str = str + " AND OrderStatus=" + ((int)query.Status);
            }
            if (query.selectAgentId != null && query.selectAgentId > 0)
            {
                str = str + " OR RedPagerID = " + query.selectAgentId;
            }
            string str2 = "SELECT Amount,RedPagerId,OrderId, OrderDate, OrderStatus,PaymentTypeId, OrderTotal,Gateway,FirstCommission,SecondCommission,ThirdCommission,ShippingModeId,ModeName,UserName FROM Hishop_Orders o WHERE ReferralUserId = @UserId";
            str2 = (str2 + str + " ORDER BY OrderDate DESC") + " SELECT OrderId,SkuId, ThumbnailsUrl=p.ThumbnailUrl180, ItemDescription, SKUContent, SKU, OI.ProductId,Quantity,ItemListPrice,ItemAdjustedCommssion,OrderItemsStatus,ItemsCommission FROM Hishop_OrderItems OI Left Join Hishop_Products P on oi.ProductId=p.ProductId WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE ReferralUserId = @UserId" + str + ");";
            str2 += "select * from Hishop_OrderGifts G WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE ReferralUserId =@UserId " + str + ")";

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, query.UserId);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = set.Tables[0].Columns["OrderId"];
            DataColumn childColumn = set.Tables[1].Columns["OrderId"];
            DataRelation relation = new DataRelation("OrderItems", parentColumn, childColumn);
            DataRelation relationGift = new DataRelation("OrderGifts", parentColumn, set.Tables[2].Columns["OrderId"], false);
            set.Relations.Add(relation);
            set.Relations.Add(relationGift);
            return set;
        }

        public int GetDistributorOrderCount(OrderQuery query)
        {
            string str = string.Empty;
            switch (query.Status)
            {
                case OrderStatus.Finished:
                    str = str + " AND OrderStatus=" + ((int)query.Status);
                    break;

                case OrderStatus.Today:
                    {
                        string str2 = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
                        str = str + " AND OrderDate>='" + str2 + "'";
                        break;
                    }
            }
            string str3 = "SELECT COUNT(*)  FROM Hishop_Orders o WHERE ReferralUserId = @ReferralUserId";
            str3 = str3 + str;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str3);
            sqlStringCommand.CommandType = CommandType.Text;
            this.database.AddInParameter(sqlStringCommand, "ReferralUserId", DbType.Int32, query.UserId);
            return (int)this.database.ExecuteScalar(sqlStringCommand);
        }

        public DataSet GetOrderGoods(string orderIds)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT OrderId, ItemDescription AS ProductName, SKU, SKUContent, ShipmentQuantity,");
            builder.Append(" (SELECT Stock FROM Hishop_SKUs WHERE SkuId = oi.SkuId) + oi.ShipmentQuantity AS Stock, (SELECT Remark FROM Hishop_Orders WHERE OrderId = oi.OrderId) AS Remark");
            builder.Append(" FROM Hishop_OrderItems oi WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))");
            builder.Append(" AND (OrderItemsStatus=2 OR OrderItemsStatus=1)");
            builder.AppendFormat(" AND OrderId IN ({0}) ORDER BY OrderId;", orderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public OrderInfo GetOrderInfo(string orderId)
        {
            OrderInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Orders Where OrderId=@OrderId;  SELECT *,p.CategoryId FROM Hishop_OrderItems OI inner join Hishop_Products P on oi.ProductId=p.ProductId Where OrderId= @OrderId ; Select * from Hishop_OrderGifts where OrderID = @OrderId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateOrder(reader);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.LineItems.Add((string)reader["SkuId"], DataMapper.PopulateLineItem(reader));
                }
                //新增礼物
                reader.NextResult();
                while (reader.Read())
                {
                    OrderGiftInfo item = DataMapper.PopulateOrderGift(reader);
                    info.Gifts.Add(item);
                }
            }
            return info;
        }

        public List<OrderInfo> GetOrderInfoList(string orderIds)
        {
            /*OrderInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM Hishop_Orders Where OrderId IN ({0});  SELECT *,p.CategoryId FROM Hishop_OrderItems OI inner join Hishop_Products P on oi.ProductId=p.ProductId Where OrderId IN ({0})", orderIds));
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderIds);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                 if (reader.Read())
                {
                    info = DataMapper.PopulateOrder(reader);
                }
                reader.NextResult();
                while (reader.Read())
                {                   
                    info.LineItems.Add((string)reader["SkuId"], DataMapper.PopulateLineItem(reader));
                }
                //新增礼物
                reader.NextResult();
                while (reader.Read())
                {
                    OrderGiftInfo item = DataMapper.PopulateOrderGift(reader);
                    info.Gifts.Add(item);
                }
            }
            return info;*/
            List<OrderInfo> listInfo = new List<OrderInfo>();
            string[] arrayOrderid = orderIds.Split(',');
            foreach (string orderid in arrayOrderid)
            {
                if (string.IsNullOrEmpty(orderid)) continue;
                listInfo.Add(GetOrderInfo(orderid));
            }
            return listInfo;
        }
           
        //获取简单的订单信息,一般用于快速收银
        public DataTable GetSimpleOrderInfo(string orderId)
        {
            DbCommand cmd = this.database.GetSqlStringCommand("SELECT * FROM vw_Hishop_Order Where OrderId = @OrderId");
            this.database.AddInParameter(cmd, "OrderId", DbType.String, orderId);
            using (IDataReader reader = this.database.ExecuteReader(cmd))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DbQueryResult GetOrders(OrderQuery query)
        {
            StringBuilder builder = new StringBuilder("1=1");
            if (query.Type.HasValue)
            {
                if (((OrderQuery.OrderType)query.Type.Value) == OrderQuery.OrderType.GroupBuy)
                {
                    builder.Append(" And GroupBuyId > 0 ");
                }
                else
                {
                    builder.Append(" And GroupBuyId is null ");
                }
            }
            if ((query.OrderId != string.Empty) && (query.OrderId != null))
            {
                builder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
            }
            if (query.UserId.HasValue)
            {
                builder.AppendFormat(" AND UserId = '{0}'", query.UserId.Value);
            }
            if (query.referralUserId.HasValue)
            {
                builder.AppendFormat(" AND ReferralUserId = '{0}'", query.referralUserId.Value);
            }
            if (!string.IsNullOrEmpty(query.PayTypeName))
            {
                builder.AppendFormat(" AND PaymentType = '{0}'", query.PayTypeName);
            }
            if (query.PaymentType.HasValue)
            {
                builder.AppendFormat(" AND PaymentTypeId = '{0}'", query.PaymentType.Value);
            }
            if (query.GroupBuyId.HasValue)
            {
                builder.AppendFormat(" AND GroupBuyId = {0}", query.GroupBuyId.Value);
            }
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
            }
            if (!string.IsNullOrEmpty(query.ExpressCompanyName))
            {
                builder.AppendFormat("AND ExpressCompanyName='{0}'", query.ExpressCompanyName);
            }
            if (!string.IsNullOrEmpty(query.ShipTo))
            {
                builder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
            }
            if (query.RegionId.HasValue)
            {
                builder.AppendFormat(" AND ShippingRegion like '%{0}%'", DataHelper.CleanSearchString(RegionHelper.GetFullRegion(query.RegionId.Value, "，")));
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                builder.AppendFormat(" AND  UserName  = '{0}' ", DataHelper.CleanSearchString(query.UserName));
            }

            if (!string.IsNullOrEmpty(query.ChannelName))
            {
                builder.AppendFormat(" AND  ChannelName  Like '%{0}%' ", DataHelper.CleanSearchString(query.ChannelName));
            }

            if (query.Status == OrderStatus.History)
            {
                builder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2} AND OrderDate < '{3}'", new object[] { 1, 4, 9, DateTime.Now.AddMonths(-3) });
            }
            if (query.Status == OrderStatus.All)
            {
                builder.AppendFormat(" AND OrderStatus != {0}", 88);
            }
            else if (query.Status == OrderStatus.BuyerAlreadyPaid)
            {
                builder.AppendFormat(" AND (OrderStatus = {0} OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest'))", (int)query.Status);
            }
            else if (query.Status != OrderStatus.All)
            {
                builder.AppendFormat(" AND OrderStatus = {0}", (int)query.Status);
            }
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (query.ShippingModeId.HasValue)
            {
                builder.AppendFormat(" AND ShippingModeId = {0}", query.ShippingModeId.Value);
            }
            if (query.IsPrinted.HasValue)
            {
                builder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
            }
            if (query.ShippingModeId > 0)
            {
                builder.AppendFormat(" AND ShippingModeId={0}", query.ShippingModeId);
            }
            if (query.OrderAgent > 0)
            {
                switch (query.OrderAgent)
                {
                    case 1:
                        builder.AppendFormat(" AND CHARINDEX('_',OrderId,0)=0", query.ShippingModeId);
                        break;
                    case 2:
                        builder.AppendFormat(" AND CHARINDEX('_',OrderId,0)>0");
                        break;
                }
            }
            if (!string.IsNullOrEmpty(query.modeName))//根据配送店铺过滤
            {
                builder.AppendFormat(" AND ( ModeName = '{0}' Or Sender = '{1}')", DataHelper.CleanSearchString(query.modeName),query.Sender);
            }
            if (!string.IsNullOrEmpty(query.RealName))
            {
                builder.AppendFormat(" AND RealName like '%{0}%'", DataHelper.CleanSearchString(query.RealName));
            }
            if (!string.IsNullOrEmpty(query.Sender))
            {
                if (query.ClientUserId==0)
                    builder.AppendFormat(" AND Sender='{0}'", query.Sender);
                else
                    builder.AppendFormat(" AND (Sender='{0}' Or ReferralUserId={1} Or ModeName = '{2}')", query.Sender, query.ClientUserId, query.modeName);
            }
            if (!string.IsNullOrEmpty(query.StoreType))
            {
                string md1 = "[堂食用户]"; string md2 = "[活动用户]";
                if (query.StoreType == "门店" || query.StoreType == "活动")
                {
                    builder.AppendFormat(" AND Username= '{0}'", (query.StoreType == "门店") ? md1 : md2);
                }
                else
                    builder.AppendFormat(" AND Username<> '{0}' and  username <> '{1}'", md1, md2);
            }
            if (query.AgentUserId.HasValue)
            {
                builder.AppendFormat(" AND CHARINDEX('_',orderid) > 0 and SUBSTRING(orderid,CHARINDEX('_',orderid)+1,LEN(orderid)) = {0}", query.AgentUserId);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_Order vho left join Hishop_ChannelList hc on vho.ChannelId =hc.id", "OrderId", builder.ToString(), "vho.*,hc.ChannelName");
        }

        /// <summary>
        /// 获取天使下面所有分销商的所有订单信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetUnderOrders(OrderQuery query)
        {
            StringBuilder builder = new StringBuilder("1=1");
            if (query.Type.HasValue)
            {
                if (((OrderQuery.OrderType)query.Type.Value) == OrderQuery.OrderType.GroupBuy)
                {
                    builder.Append(" And GroupBuyId > 0 ");
                }
                else
                {
                    builder.Append(" And GroupBuyId is null ");
                }
            }
            if ((query.OrderId != string.Empty) && (query.OrderId != null))
            {
                builder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
            }
            if (query.UserId.HasValue)//获取改天使下面所有分销商的订单信息
            {
                builder.AppendFormat(" AND (charindex('|{0}|','|'+ad.AgentPath+'|')>0)", query.UserId.Value);
            }
            if (!string.IsNullOrEmpty(query.StoreName))//店铺名,模糊查询
            {
                builder.AppendFormat(" AND StoreName like '%{0}%'", query.StoreName);
            }
            if (query.customKeyword != null && query.customKeyword.Count > 0)//如果自定义查找内大于零
            {
                for (int i = 0; i < query.customKeyword.Count; i++)
                    builder.AppendFormat(" AND( ad.StoreName like '%{0}%' or ShipTo LIKE '%{0}%' or UserName LIKE '%{0}%' or Address LIKE '%{0}%' or CellPhone LIKE '%{0}%')", query.customKeyword[i]);
            }
            if (query.PaymentType.HasValue)
            {
                builder.AppendFormat(" AND PaymentTypeId = '{0}'", query.PaymentType.Value);
            }
            if (query.GroupBuyId.HasValue)
            {
                builder.AppendFormat(" AND GroupBuyId = {0}", query.GroupBuyId.Value);
            }
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
            }
            if (!string.IsNullOrEmpty(query.ShipTo))
            {
                builder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
            }
            if (query.RegionId.HasValue)
            {
                builder.AppendFormat(" AND ShippingRegion like '%{0}%'", DataHelper.CleanSearchString(RegionHelper.GetFullRegion(query.RegionId.Value, "，")));
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                builder.AppendFormat(" AND  UserName  = '{0}' ", DataHelper.CleanSearchString(query.UserName));
            }
            if (query.Status == OrderStatus.History)
            {
                builder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2} AND OrderDate < '{3}'", new object[] { 1, 4, 9, DateTime.Now.AddMonths(-3) });
            }
            else if (query.Status == OrderStatus.BuyerAlreadyPaid)
            {
                builder.AppendFormat(" AND (OrderStatus = {0} OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest'))", (int)query.Status);
            }
            else if (query.Status != OrderStatus.All)
            {
                builder.AppendFormat(" AND OrderStatus = {0}", (int)query.Status);
            }
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (query.ShippingModeId.HasValue)
            {
                builder.AppendFormat(" AND ShippingModeId = {0}", query.ShippingModeId.Value);
            }
            if (query.IsPrinted.HasValue)
            {
                builder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
            }
            if (query.ShippingModeId > 0)
            {
                builder.AppendFormat(" AND ShippingModeId={0}", query.ShippingModeId);
            }

            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_Order VO inner join aspnet_Distributors AD on vo.ReferralUserId=ad.UserId", "OrderId", builder.ToString(), "ad.agentpath,ad.ReferralUserId as parentId,vo.* ,ParentName=''");
        }

        public DataSet GetOrdersAndLines(string orderIds)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT * FROM Hishop_Orders WHERE OrderStatus > 0 AND OrderStatus < 4 AND OrderId IN ({0}) order by OrderDate desc ", orderIds);
            builder.AppendFormat(" SELECT * FROM Hishop_OrderItems WHERE OrderId IN ({0});", orderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public DataSet GetProductGoods(string orderIds)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT ItemDescription AS ProductName, SKU, SKUContent, sum(ShipmentQuantity) as ShipmentQuantity,");
            builder.Append(" (SELECT Stock FROM Hishop_SKUs WHERE SkuId = oi.SkuId) + sum(ShipmentQuantity) AS Stock FROM Hishop_OrderItems oi");
            builder.Append(" WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))");
            builder.Append(" AND OrderItemsStatus=2");
            builder.AppendFormat(" AND OrderId in ({0}) GROUP BY ItemDescription, SkuId, SKU, SKUContent;", orderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public string GetReplaceComments(string orderId)
        {
            string query = "select Comments from Hishop_OrderReplace where HandleStatus=0 and OrderId='" + orderId + "'";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 == null) || (obj2 is DBNull))
            {
                return "";
            }
            return obj2.ToString();
        }

        public DataTable GetSendGoodsOrders(string orderIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM Hishop_Orders WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest')) AND OrderId IN ({0}) order by OrderDate desc", orderIds));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }
        
        public DataSet GetUserOrder(int userId, OrderQuery query)
        {
            string str = string.Empty;
            if (query.Status == OrderStatus.WaitBuyerPay)
            {
                str = str + " AND OrderStatus = 1 AND Gateway <> 'hishop.plugins.payment.podrequest'";
            }
            else if (query.Status == OrderStatus.SellerAlreadySent)
            {
                str = str + " AND (OrderStatus = 2 OR OrderStatus = 3 OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest'))";
            }
            string str2 = "SELECT payDate,OrderId, OrderDate, OrderStatus,PaymentTypeId, OrderTotal, Gateway,(SELECT count(0) FROM vshop_OrderRedPager WHERE OrderId = o.OrderId and ExpiryDays<getdate() and AlreadyGetTimes<MaxGetTimes) as HasRedPage,(SELECT SUM(Quantity) FROM Hishop_OrderItems WHERE OrderId = o.OrderId) as ProductSum,(select SUM(Quantity) from Hishop_OrderGifts G WHERE OrderId = o.OrderId)as GiftSum,(select SUM(costPoint)from Hishop_OrderGifts G WHERE OrderId = o.OrderId)as PointSum FROM Hishop_Orders o WHERE UserId = @UserId";
            str2 = (str2 + str + " ORDER BY OrderDate DESC") + " SELECT OrderId, ThumbnailsUrl=p.ThumbnailUrl180, ItemDescription, SKUContent, SKU,OrderItemsStatus, OI.ProductId,Quantity,ItemAdjustedPrice FROM Hishop_OrderItems OI Left Join Hishop_Products P on oi.ProductId=p.ProductId WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE UserId = @UserId" + str + ");";
            str2 += "select * from Hishop_OrderGifts G WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE UserId =@UserId " + str + ")";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = set.Tables[0].Columns["OrderId"];
            DataColumn childColumn = set.Tables[1].Columns["OrderId"];
            DataRelation relation = new DataRelation("OrderItems", parentColumn, childColumn, false);
            DataRelation relationGift = new DataRelation("OrderGifts", parentColumn, set.Tables[2].Columns["OrderId"], false);
            set.Relations.Add(relation);
            set.Relations.Add(relationGift);
            return set;
        }

        public DataSet GetUserOrderGift(string orderId)
        {
            string str = string.Empty;

            str = " SELECT * FROM Hishop_OrderGifts a join Hishop_Gifts b on b.GiftId = a.giftid WHERE a.OrderId = '" + orderId + "')";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            return set;
        }

        public int[] getMemberOrderNums(int uid)
        {
            int[] result = new int[2];
            
            string sqlWaitToPay = string.Format("select COUNT(*) from Hishop_Orders where UserId={0} and OrderStatus = 1", uid);
            string sqlPaid = string.Format("select COUNT(*) from Hishop_Orders where UserId={0} and OrderStatus = 2", uid);
            result[0] = this.database.ExecuteScalar(this.database.GetSqlStringCommand(sqlWaitToPay)).ToInt();
            result[1] = this.database.ExecuteScalar(this.database.GetSqlStringCommand(sqlPaid)).ToInt();
            return result;
        }


        public int GetUserOrderCount(int userId, OrderQuery query)
        {
            string str = string.Empty;
            if (query.Status == OrderStatus.WaitBuyerPay)
            {
                str = str + " AND OrderStatus = 1 AND Gateway <> 'hishop.plugins.payment.podrequest'";
            }
            else if (query.Status == OrderStatus.SellerAlreadySent)
            {
                str = str + " AND (OrderStatus = 2 OR OrderStatus = 3 OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest'))";
            }
            string str2 = "SELECT COUNT(1)  FROM Hishop_Orders o WHERE UserId = @UserId";
            str2 = str2 + str;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            sqlStringCommand.CommandType = CommandType.Text;
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (int)this.database.ExecuteScalar(sqlStringCommand);
        }

        public DataSet GetUserOrderReturn(int userId, OrderQuery query)
        {
            string str = string.Empty + " AND (OrderStatus = 2 OR OrderStatus = 3)";
            string str2 = "SELECT OrderId, OrderDate, OrderStatus,PaymentTypeId, OrderTotal, (SELECT SUM(Quantity) FROM  Hishop_OrderItems WHERE OrderId = o.OrderId) as ProductSum FROM Hishop_Orders o WHERE UserId = @UserId";
            str2 = (str2 + str + " ORDER BY OrderDate DESC") + " SELECT OrderId, ThumbnailsUrl=p.ThumbnailUrl180,Quantity,  ItemDescription,OrderItemsStatus, SKUContent, SKU, OI.ProductId FROM Hishop_OrderItems OI Left Join Hishop_Products P on  oi.ProductId=p.ProductId  WHERE (OrderItemsStatus=2 OR OrderItemsStatus=3) AND OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE  UserId = @UserId" + str + ")";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = set.Tables[0].Columns["OrderId"];
            DataColumn childColumn = set.Tables[1].Columns["OrderId"];
            DataRelation relation = new DataRelation("OrderItems", parentColumn, childColumn);
            set.Relations.Add(relation);
            return set;
        }


        public int GetUserOrderReturnCount(int userId)
        {
            object obj2 = string.Empty;
            string str = string.Concat(new object[] { obj2, " AND (OrderItemsStatus = ", 6, " OR OrderItemsStatus =", 7, ")" });
            string query = "SELECT COUNT(*) FROM Hishop_OrderItems WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE UserId=@UserId)";
            query = query + str;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (int)this.database.ExecuteScalar(sqlStringCommand);
        }

        public bool InsertCalculationCommission(ArrayList UserIdList, ArrayList ReferralBlanceList, string orderid, ArrayList OrdersTotalList, string userid)
        {
            string query = "";
            query = query + "begin try  " + "  begin tran TranUpdate";
            for (int i = 0; i < UserIdList.Count; i++)
            {
                object obj2 = query;
                query = string.Concat(new object[] { obj2, " INSERT INTO [Hishop_Commissions]([UserId],[ReferralUserId],[OrderId],[OrderTotal],[CommTotal],[CommType],[State])VALUES(", UserIdList[i], ",", userid, ",'", orderid, "',", OrdersTotalList[i], ",", ReferralBlanceList[i], ",1,0);" });
            }
            query = query + " COMMIT TRAN TranUpdate" + "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool SetOrderExpressComputerpe(string orderIds, string expressCompanyName, string expressCompanyAbb)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Orders SET ExpressCompanyName=@ExpressCompanyName,ExpressCompanyAbb=@ExpressCompanyAbb WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest')) AND OrderId IN ({0})", orderIds));
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, expressCompanyName);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, expressCompanyAbb);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool SetOrderShippingMode(string orderIds, int realShippingModeId, string realModeName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Orders SET RealShippingModeId=@RealShippingModeId,RealModeName=@RealModeName WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest')) AND OrderId IN ({0})", orderIds));
            this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", DbType.Int32, realShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "RealModeName", DbType.String, realModeName);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateCommossionByOrderId(string orderId, decimal adjustcommssion, int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_Commissions Set CommTotal =CommTotal-@AdjustCommssion Where OrderId =@OrderId AND UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "AdjustCommssion", DbType.Decimal, adjustcommssion);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int64, userId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public void UpdateItemsStatus(string orderId, int status, string ItemStr)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_OrderItems Set OrderItemsStatus=@OrderItemsStatus Where OrderId =@OrderId and SkuId IN (" + ItemStr + ")");
            this.database.AddInParameter(sqlStringCommand, "OrderItemsStatus", DbType.Int32, status);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool UpdateOrder(OrderInfo order, DbTransaction dbTran = null)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET  OrderStatus = @OrderStatus, CloseReason=@CloseReason, PayDate = @PayDate, ShippingDate=@ShippingDate, FinishDate = @FinishDate, RegionId = @RegionId, ShippingRegion = @ShippingRegion, Address = @Address, ZipCode = @ZipCode,ShipTo = @ShipTo, TelPhone = @TelPhone, CellPhone = @CellPhone, ShippingModeId=@ShippingModeId ,ModeName=@ModeName, RealShippingModeId = @RealShippingModeId, RealModeName = @RealModeName, ShipOrderNumber = @ShipOrderNumber,  ExpressCompanyName = @ExpressCompanyName,ExpressCompanyAbb = @ExpressCompanyAbb, PaymentTypeId=@PaymentTypeId,PaymentType=@PaymentType, Gateway = @Gateway, ManagerMark=@ManagerMark,ManagerRemark=@ManagerRemark,IsPrinted=@IsPrinted, OrderTotal = @OrderTotal, OrderProfit=@OrderProfit,Amount=@Amount,OrderCostPrice=@OrderCostPrice, AdjustedFreight = @AdjustedFreight, PayCharge = @PayCharge, AdjustedDiscount=@AdjustedDiscount,OrderPoint=@OrderPoint,GatewayOrderId=@GatewayOrderId WHERE OrderId = @OrderId");
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, (int)order.OrderStatus);
            this.database.AddInParameter(sqlStringCommand, "CloseReason", DbType.String, order.CloseReason);
            this.database.AddInParameter(sqlStringCommand, "PayDate", DbType.DateTime, order.PayDate);
            this.database.AddInParameter(sqlStringCommand, "ShippingDate", DbType.DateTime, order.ShippingDate);
            this.database.AddInParameter(sqlStringCommand, "FinishDate", DbType.DateTime, order.FinishDate);
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.String, order.RegionId);
            this.database.AddInParameter(sqlStringCommand, "ShippingRegion", DbType.String, order.ShippingRegion);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, order.Address);
            this.database.AddInParameter(sqlStringCommand, "ZipCode", DbType.String, order.ZipCode);
            this.database.AddInParameter(sqlStringCommand, "ShipTo", DbType.String, order.ShipTo);
            this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, order.TelPhone);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, order.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "ShippingModeId", DbType.Int32, order.ShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "ModeName", DbType.String, order.ModeName);
            this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", DbType.Int32, order.RealShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "RealModeName", DbType.String, order.RealModeName);
            this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, order.ShipOrderNumber);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, order.ExpressCompanyName);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, order.ExpressCompanyAbb);
            this.database.AddInParameter(sqlStringCommand, "PaymentTypeId", DbType.Int32, order.PaymentTypeId);
            this.database.AddInParameter(sqlStringCommand, "PaymentType", DbType.String, order.PaymentType);
            this.database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, order.Gateway);
            this.database.AddInParameter(sqlStringCommand, "ManagerMark", DbType.Int32, order.ManagerMark);
            this.database.AddInParameter(sqlStringCommand, "ManagerRemark", DbType.String, order.ManagerRemark);
            this.database.AddInParameter(sqlStringCommand, "IsPrinted", DbType.Boolean, order.IsPrinted);
            this.database.AddInParameter(sqlStringCommand, "OrderTotal", DbType.Currency, order.GetTotal());
            this.database.AddInParameter(sqlStringCommand, "OrderProfit", DbType.Currency, order.GetProfit());
            this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, order.GetAmount());
            this.database.AddInParameter(sqlStringCommand, "OrderCostPrice", DbType.Currency, order.GetCostPrice());
            this.database.AddInParameter(sqlStringCommand, "AdjustedFreight", DbType.Currency, order.AdjustedFreight);
            this.database.AddInParameter(sqlStringCommand, "PayCharge", DbType.Currency, order.PayCharge);
            this.database.AddInParameter(sqlStringCommand, "AdjustedDiscount", DbType.Currency, order.AdjustedDiscount);
            this.database.AddInParameter(sqlStringCommand, "OrderPoint", DbType.Int32, order.Points);
            this.database.AddInParameter(sqlStringCommand, "GatewayOrderId", DbType.String, order.GatewayOrderId);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public void UpdatePayOrderStock(string orderId)
        {
            string updateSql = "Update Hishop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM Hishop_OrderItems Where OrderId =@OrderId);";
            updateSql += "Update Hishop_Gifts Set Stock=Stock-OG.Quantity From Hishop_OrderGifts OG  Where Hishop_Gifts.GiftId=OG.GiftId And OG.OrderID=@OrderId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(updateSql);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool UpdateRefundOrderStock(string orderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = Stock + (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId) WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM Hishop_OrderItems Where OrderId =@OrderId)");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public bool IsOrderRemind(int UserID, int time)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select Count(*) FROM Hishop_Orders WHERE  DATEDIFF(mi,OrderDate,GETDATE())>=@time and UserId=@UserID and OrderStatus =1");
            this.database.AddInParameter(sqlStringCommand, "UserID", DbType.Int32, UserID);
            this.database.AddInParameter(sqlStringCommand, "time", DbType.Int32, UserID);
            int count = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取佣金详细信息
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public DataTable GetCommissionDetails(string orderid, string storename = "", string starttime = "", string endtime = "")
        {
            string sql = string.Format(@"
            select oi.OrderId,L1=ad.storename+'：'+RTRIM(ItemsCommission),
            case when dbo.Fn_jhb_ReferralName(ho.UserId,2) != '' then dbo.Fn_jhb_ReferralName(ho.UserId,2)+'：'+RTRIM(SecondItemsCommission) else '无' end L2,
            case when dbo.Fn_jhb_ReferralName(ho.UserId,1) != '' then dbo.Fn_jhb_ReferralName(ho.UserId,1)+'：'+RTRIM(ThirdItemsCommission) else '无' end L3,
	            productname
	            ,ItemAdjustedPrice,Quantity,CONVERT(char(20),ho.OrderDate,20)as Time
             from Hishop_OrderItems OI inner join Hishop_Products P on oi.ProductId=p.ProductId inner join Hishop_Orders HO on oi.OrderId=ho.OrderId 
             inner join aspnet_Distributors AD on ho.UserId=ad.UserId where 1 = 1 ");
            if (!string.IsNullOrEmpty(orderid))
            {
                sql += string.Format(" and oi.OrderId='{0}'  ", orderid);
            }
            if (storename != "")
            {
                sql += string.Format(" and (ad.storename='{0}' or dbo.Fn_jhb_ReferralName(ho.UserId,2)='{0}' or  dbo.Fn_jhb_ReferralName(ho.UserId,1)='{0}')", storename);
            }
            if (starttime != "")
            {
                sql += string.Format(" AND datediff(dd,'{0}',CONVERT(char(20),ho.OrderDate,20))>=0", starttime);
            }
            if (endtime != "")
            {
                sql += string.Format(" AND datediff(dd,'{0}',CONVERT(char(20),ho.OrderDate,20))<=0", endtime);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 导出Excel查询数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetOrder(OrderQuery query)
        {
            StringBuilder builder = new StringBuilder("1=1");
            if (query.Type.HasValue)
            {
                if (((OrderQuery.OrderType)query.Type.Value) == OrderQuery.OrderType.GroupBuy)
                {
                    builder.Append(" And GroupBuyId > 0 ");
                }
                else
                {
                    builder.Append(" And GroupBuyId is null ");
                }
            }
            if ((query.OrderId != string.Empty) && (query.OrderId != null))
            {
                builder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
            }
            if (query.UserId.HasValue)
            {
                builder.AppendFormat(" AND UserId = '{0}'", query.UserId.Value);
            }
            if (query.PaymentType.HasValue)
            {
                builder.AppendFormat(" AND PaymentTypeId = '{0}'", query.PaymentType.Value);
            }
            if (query.GroupBuyId.HasValue)
            {
                builder.AppendFormat(" AND GroupBuyId = {0}", query.GroupBuyId.Value);
            }
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
            }
            if (!string.IsNullOrEmpty(query.ShipTo))
            {
                builder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
            }
            if (query.RegionId.HasValue)
            {
                builder.AppendFormat(" AND ShippingRegion like '%{0}%'", DataHelper.CleanSearchString(RegionHelper.GetFullRegion(query.RegionId.Value, "，")));
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                builder.AppendFormat(" AND  UserName  = '{0}' ", DataHelper.CleanSearchString(query.UserName));
            }
            if (query.Status == OrderStatus.History)
            {
                builder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2} AND OrderDate < '{3}'", new object[] { 1, 4, 9, DateTime.Now.AddMonths(-3) });
            }
            else if (query.Status == OrderStatus.BuyerAlreadyPaid)
            {
                builder.AppendFormat(" AND (OrderStatus = {0} OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest'))", (int)query.Status);
            }
            else if (query.Status != OrderStatus.All)
            {
                builder.AppendFormat(" AND OrderStatus = {0}", (int)query.Status);
            }
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (query.ShippingModeId.HasValue)
            {
                builder.AppendFormat(" AND ShippingModeId = {0}", query.ShippingModeId.Value);
            }
            if (query.IsPrinted.HasValue)
            {
                builder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
            }
            if (query.ShippingModeId > 0)
            {
                builder.AppendFormat(" AND ShippingModeId={0}", query.ShippingModeId);
            }

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * FROM vw_Hishop_Order WHERE  " + builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];

        }

        public DataTable GetOrderSanZuo(OrderQuery query)
        {
            StringBuilder builder = new StringBuilder("1=1");
            if (query.Type.HasValue)
            {
                if (((OrderQuery.OrderType)query.Type.Value) == OrderQuery.OrderType.GroupBuy)
                {
                    builder.Append(" And GroupBuyId > 0 ");
                }
                else
                {
                    builder.Append(" And GroupBuyId is null ");
                }
            }
            if ((query.OrderId != string.Empty) && (query.OrderId != null))
            {
                builder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
            }
            if (query.UserId.HasValue)
            {
                builder.AppendFormat(" AND UserId = '{0}'", query.UserId.Value);
            }
            if (query.PaymentType.HasValue)
            {
                builder.AppendFormat(" AND PaymentTypeId = '{0}'", query.PaymentType.Value);
            }
            if (query.GroupBuyId.HasValue)
            {
                builder.AppendFormat(" AND GroupBuyId = {0}", query.GroupBuyId.Value);
            }
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
            }
            if (!string.IsNullOrEmpty(query.ShipTo))
            {
                builder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
            }
            if (query.RegionId.HasValue)
            {
                builder.AppendFormat(" AND ShippingRegion like '%{0}%'", DataHelper.CleanSearchString(RegionHelper.GetFullRegion(query.RegionId.Value, "，")));
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                builder.AppendFormat(" AND  UserName  = '{0}' ", DataHelper.CleanSearchString(query.UserName));
            }
            if (query.Status == OrderStatus.History)
            {
                builder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2} AND OrderDate < '{3}'", new object[] { 1, 4, 9, DateTime.Now.AddMonths(-3) });
            }
            else if (query.Status == OrderStatus.BuyerAlreadyPaid)
            {
                builder.AppendFormat(" AND (OrderStatus = {0} OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest'))", (int)query.Status);
            }
            else if (query.Status != OrderStatus.All)
            {
                builder.AppendFormat(" AND OrderStatus = {0}", (int)query.Status);
            }
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (query.ShippingModeId.HasValue)
            {
                builder.AppendFormat(" AND ShippingModeId = {0}", query.ShippingModeId.Value);
            }
            if (query.IsPrinted.HasValue)
            {
                builder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
            }
            if (query.ShippingModeId > 0)
            {
                builder.AppendFormat(" AND ShippingModeId={0}", query.ShippingModeId);
            }
            if (!string.IsNullOrEmpty(query.Sender))
            {
                if (query.ClientUserId == 0)
                    builder.AppendFormat(" AND Sender='{0}'", query.Sender);
                else
                    builder.AppendFormat(" AND (Sender='{0}' Or ReferralUserId={1} Or ModeName = '{2}')", query.Sender, query.ClientUserId, query.modeName);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select * FROM  vw_Hishop_Order as Ho left Join Hishop_OrderItems as HOI on ho.OrderId=hoi.OrderId  Join Hishop_Products as HP on hoi.ProductId=hp.ProductId  WHERE {0} AND OrderStatus=5 ",builder.ToString()));
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];

        }

        //订单回收
        public int removeOrders(string orderIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("update Hishop_Orders set OrderStatus='88' WHERE OrderId IN({0})", orderIds));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }
        /// <summary>
        /// 根据时间段获取orderId列表
        /// </summary>
        public System.Collections.Generic.IList<string> GetTodayOrders(string startDate, string endDate, int senderId = 0, string storeName = "", int userid = 0, string modeName= "")
        {
            System.Collections.Generic.IList<string> orderids = new System.Collections.Generic.List<string>();
            string sql = string.Format("select OrderId from Hishop_Orders where OrderDate >= '{0}' and OrderDate<='{1}' and OrderStatus=5", startDate, endDate);
            if (userid != 0)
            {
                sql += string.Format("and (sender={0} or (PaymentType='微信支付' and ModeName='{1}'))", userid, modeName);
            }
            if (senderId > 0)
            {
                sql += string.Format(" and (sender = '{0}' or modename='{1}')",senderId,storeName);
            }
            
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            DataTable dt = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            foreach(DataRow row in dt.Rows)
            {
                orderids.Add(row["OrderId"].ToString());
            }
            return orderids;
        }

        /// <summary>
        /// 根据时间段获取商品列表
        /// </summary>
        public DataTable GetTodayProducts(string startDate, string endDate, int senderId = 0, string storeName = "", int userid = 0, string modeName = "")
        {
            string sql = string.Format("select distinct hp.ProductId,hp.ProductName, 0 as quantity,0.00 as price from Hishop_Products HP left join Hishop_OrderItems HO on hp.ProductId = ho.ProductId left join Hishop_Orders HOD on ho.OrderId=hod.OrderId  where hod.OrderDate >= '{0}' and hod.OrderDate<='{1}' and hod.OrderStatus=5", startDate, endDate);
            if (userid != 0)
            {
                sql += string.Format("and (sender={0} or (PaymentType='微信支付' and ModeName='{1}'))", userid, modeName);
            }
            if (senderId > 0 && storeName != "")
            {
                sql += string.Format(" and( hod.sender = '{0}' or hod.modename = '{1}')", senderId, storeName);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format(sql));
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 根据时间段获取礼品列表
        /// </summary>
        public DataTable GetTodayGifts(string startDate, string endDate, int senderId = 0, string storeName = "", int userid = 0, string modeName = "")
        {
            string sql = string.Format("select distinct og.giftid,og.giftname,0 as quantity,0 as costpoint from Hishop_Gifts HG left join Hishop_OrderGifts OG on hg.giftid = og.giftid  left join Hishop_Orders HOD on OG.OrderId=hod.OrderId  where hod.OrderDate >= '{0}' and hod.OrderDate<='{1}' and hod.OrderStatus=5", startDate, endDate);
            if (userid != 0)
            {
                sql += string.Format("and (sender={0} or (PaymentType='微信支付' and ModeName='{1}'))", userid, modeName);
            }
            if (senderId > 0 && storeName != "")
            {
                sql += string.Format(" and( hod.sender = '{0}' or hod.modename = '{1}')", senderId, storeName);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format(sql));
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 修改订单的价钱信息,用于退单功能(截止20160309,该功能仅用于爽爽挝啡)
        /// </summary>
        public bool UpdateOrderAmountInfo(string orderId,decimal OrderTotal, int OrderPoint, decimal OrderCostPrice, decimal OrderProfit, decimal Amount)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("Update Hishop_Orders Set OrderTotal=@OrderTotal,OrderPoint=@OrderPoint,OrderCostPrice=@OrderCostPrice,OrderProfit=@OrderProfit,Amount=@Amount Where OrderId=@OrderId"));
            this.database.AddInParameter(sqlStringCommand, "OrderTotal", DbType.Decimal,OrderTotal);
            this.database.AddInParameter(sqlStringCommand, "OrderPoint", DbType.Int16, OrderPoint);
            this.database.AddInParameter(sqlStringCommand, "OrderCostPrice", DbType.Decimal, OrderCostPrice);
            this.database.AddInParameter(sqlStringCommand, "OrderProfit", DbType.Decimal, OrderProfit);
            this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Decimal, Amount);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);

            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public bool isActing()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("slelect * from aspnet_ActingPresent"));
            return this.database.ExecuteScalar(sqlStringCommand).ToInt() == 0;
        }

        public bool UpdateOrderInfoList(string orderidss, int OrderTotal, int OrderPoint, int OrderCostPrice, int OrderProfit, int Amount)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("Update Hishop_Orders Set OrderTotal=@OrderTotal,OrderPoint=@OrderPoint,OrderCostPrice=@OrderCostPrice,OrderProfit=@OrderProfit,Amount=@Amount Where OrderId IN ({0})", orderidss));
            this.database.AddInParameter(sqlStringCommand, "OrderTotal", DbType.Int16, OrderTotal);
            this.database.AddInParameter(sqlStringCommand, "OrderPoint", DbType.Int16, OrderPoint);
            this.database.AddInParameter(sqlStringCommand, "OrderCostPrice", DbType.Int16, OrderCostPrice);
            this.database.AddInParameter(sqlStringCommand, "OrderProfit", DbType.Int16, OrderProfit);
            this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Int16, Amount);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderidss);

            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool UpdateOrderItemsQuantity(string orderidss)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("Update Hishop_OrderItems Set Quantity=0  WHERE OrderId IN ({0})", orderidss));
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderidss);

            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        
        public string PackOrderInfos(string startDate, string endDate)
        {
            string orderInfoSql = string.Format("select * from Hishop_Orders where FinishDate between '{0}' and '{1}'", startDate, endDate);
            string orderItemsInfoSql = string.Format("select * from Hishop_OrderItems where orderid in (select orderid from Hishop_Orders where FinishDate between '{0}' and '{1}')",startDate,endDate);
            string path = HttpContext.Current.Server.MapPath("/Storage/temp/" +"pack_"+ DateTime.Now.DayOfYear + "的订单导出文件.dat");
            DataSet dsOrderInfo = this.database.ExecuteDataSet(CommandType.Text,orderInfoSql+";"+orderItemsInfoSql);

            if(ObjectSerializerHelper.DataSetSerializer(dsOrderInfo,path))
            {
                return path;
            }
            else
            {
                return null;
            }
        }
      
        /// <summary>
        /// 获取当天的订单数量
        /// </summary>
        public string GetTodayOrderNum(DbTransaction dbTran = null,int userId=0)
        {
            string where = "";
            if (userId != 0)
            {
                where = "and Sender='" + userId + "' ";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select top 1 orderid from Hishop_Orders where 1=1 " + where + " and OrderDate > convert(varchar(10),getdate()) order by OrderDate desc ");
            object o = this.database.ExecuteScalar(sqlStringCommand, dbTran);
            return (o == null) ? "" : this.database.ExecuteScalar(sqlStringCommand, dbTran).ToString();
        }

        public DataTable GetOrderItemSkuInfo(string orderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select skuid,Quantity from Hishop_OrderItems where OrderId=@OrderId", orderId));
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public bool UpdateOrderQuantity(string sql)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public bool UpdateQuantity(string sql)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
    }
}

