namespace Hidistro.SqlDal.Sales
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Sales;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web;

    public class SaleStatisticDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        private static string BuildMemberStatisticsQuery(SaleStatisticsQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT UserId, UserName ");
            if (query.StartDate.HasValue || query.EndDate.HasValue)
            {
                builder.AppendFormat(",  ( select isnull(SUM(OrderTotal),0) from Hishop_Orders where OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
                if (query.StartDate.HasValue)
                {
                    builder.AppendFormat(" and OrderDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                }
                if (query.EndDate.HasValue)
                {
                    builder.AppendFormat(" and OrderDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                }
                builder.Append(" and userId = aspnet_Members.UserId) as SaleTotals");
                builder.AppendFormat(",(select Count(OrderId) from Hishop_Orders where OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
                if (query.StartDate.HasValue)
                {
                    builder.AppendFormat(" and OrderDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                }
                if (query.EndDate.HasValue)
                {
                    builder.AppendFormat(" and OrderDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                }
                builder.Append(" and userId = aspnet_Members.UserId) as OrderCount ");
            }
            else
            {
                builder.Append(",ISNULL(Expenditure,0) as SaleTotals,ISNULL(OrderNumber,0) as OrderCount ");
            }
            builder.Append(" from aspnet_Members where Expenditure > 0");
            if (query.StartDate.HasValue || query.EndDate.HasValue)
            {
            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        private static string BuildOrdersQuery(OrderQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT OrderId FROM Hishop_Orders WHERE 1 = 1 ", new object[0]);
            if ((query.OrderId != string.Empty) && (query.OrderId != null))
            {
                builder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
            }
            else
            {
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
                    builder.AppendFormat(" AND (OrderStatus = {0} OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest'))", (int) query.Status);
                }
                else if (query.Status != OrderStatus.All)
                {
                    builder.AppendFormat(" AND OrderStatus = {0}", (int) query.Status);
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
            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        private static string BuildProductSaleQuery(SaleStatisticsQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT ProductId, SUM(o.Quantity) AS ProductSaleCounts, SUM(o.ItemAdjustedPrice * o.Quantity) AS ProductSaleTotals,");
            builder.Append("  (SUM(o.ItemAdjustedPrice * o.Quantity) - SUM(o.CostPrice * o.ShipmentQuantity) )AS ProductProfitsTotals ");
            builder.AppendFormat(" FROM Hishop_OrderItems o  WHERE 0=0 ", new object[0]);
            builder.AppendFormat(" AND OrderId IN (SELECT  OrderId FROM Hishop_Orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2})", 1, 4, 9);
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderDate >= '{0}')", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderDate <= '{0}')", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            builder.Append(" GROUP BY ProductId HAVING ProductId IN");
            builder.Append(" (SELECT ProductId FROM Hishop_Products)");
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        private static string BuildProductVisitAndBuyStatisticsQuery(SaleStatisticsQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT ProductId,(SaleCounts*100/(case when VistiCounts=0 then 1 else VistiCounts end)) as BuyPercentage");
            builder.Append(" FROM Hishop_products where SaleCounts>0");
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        private static string BuildRegionsUserQuery(Pagination page)
        {
            if (null == page)
            {
                throw new ArgumentNullException("page");
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(" SELECT r.RegionId, r.RegionName, SUM(au.UserCount) AS Usercounts,");
            builder.Append(" (SELECT (SELECT SUM(COUNT) FROM aspnet_Members)) AS AllUserCounts ");
            builder.Append(" FROM vw_Allregion_Members au, Hishop_Regions r ");
            builder.Append(" WHERE (r.AreaId IS NOT NULL) AND ((au.path LIKE r.path + LTRIM(RTRIM(STR(r.RegionId))) + ',%') OR au.RegionId = r.RegionId)");
            builder.Append(" group by r.RegionId, r.RegionName ");
            builder.Append(" UNION SELECT 0, '0', sum(au.Usercount) AS Usercounts,");
            builder.Append(" (SELECT (SELECT count(*) FROM aspnet_Members)) AS AllUserCounts ");
            builder.Append(" FROM vw_Allregion_Members au, Hishop_Regions r  ");
            builder.Append(" WHERE au.regionid IS NULL OR au.regionid = 0 group by r.RegionId, r.RegionName");
            if (!string.IsNullOrEmpty(page.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(page.SortBy), page.SortOrder.ToString());
            }
            return builder.ToString();
        }

        private static string BuildUserOrderQuery(OrderQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT OrderId FROM Hishop_Orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                builder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
                return builder.ToString();
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                builder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
            }
            if (!string.IsNullOrEmpty(query.ShipTo))
            {
                builder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
            }
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND  OrderDate >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND  OrderDate <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (!string.IsNullOrEmpty(query.Sender))
            {
                builder.AppendFormat(" AND sender = '{0}'", DataHelper.CleanSearchString(query.Sender));
            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }

            return builder.ToString();
        }

        public DataTable GetMemberStatistics(SaleStatisticsQuery query, out int totalProductSales)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_MemberStatistics_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildMemberStatisticsQuery(query));
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            totalProductSales = (int) this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return table;
        }

        public DataTable GetMemberStatisticsNoPage(SaleStatisticsQuery query)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(BuildMemberStatisticsQuery(query));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public void GetNewlyOrdersCountAndPayCount(DateTime dt, out int ordersCount, out int payCount, out int RefundOrderCount, out int ReplacementOrderCount, out int ReturnsOrderCount ,string shipDistributorStoreName="",int agentUserId = 0)
        {
            string whereSql=string.Empty;
            if (shipDistributorStoreName != "")//(爽爽挝啡)对每个门店配送的账号进行数量的过滤
            {
                whereSql = string.Format(" AND ModeName='{0}' ", shipDistributorStoreName);
            }
            if (agentUserId != 0)
            {
                whereSql = string.Format(" And  CHARINDEX('_',orderid) > 0 and SUBSTRING(orderid,CHARINDEX('_',orderid)+1,LEN(orderid)) = {0}", agentUserId);
            }
             
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT Count(*) as OrderCount ");
            builder.Append(string.Concat(new object[] { "FROM Hishop_Orders WHERE OrderStatus= ", 1, " and  DATEDIFF(s,OrderDate,'", dt.ToString(), "')<=0 " + whereSql + ";" }));
            builder.Append(string.Concat(new object[] { "SELECT  Count(*) as PayOrderCount FROM Hishop_Orders WHERE OrderStatus=", 2, " and DATEDIFF(s,payDate,'", dt.ToString(), "')<=0 " + whereSql + ";" }));
            builder.Append("SELECT  Count(*) as RefundOrderCount FROM Hishop_OrderRefund WHERE HandleStatus = 0 and DATEDIFF(s,ApplyForTime,'" + dt.ToString() + "')<=0 ;");
            builder.Append("SELECT  Count(*) as ReplacementOrderCount FROM Hishop_OrderReplace WHERE HandleStatus = 0 and DATEDIFF(s,ApplyForTime,'" + dt.ToString() + "')<=0 ;");
            builder.Append("SELECT  Count(*) as ReturnsOrderCount FROM Hishop_OrderReturns WHERE HandleStatus = 0 and DATEDIFF(s,ApplyForTime,'" + dt.ToString() + "')<=0 ;");
            sqlStringCommand.CommandText = builder.ToString();
            ordersCount = 0;
            payCount = 0;
            RefundOrderCount = 0;
            ReplacementOrderCount = 0;
            ReturnsOrderCount = 0;
            try
            {
                using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
                {
                    if (reader.Read())
                    {
                        ordersCount = reader.GetInt32(0);
                    }
                    if (reader.NextResult() && reader.Read())
                    {
                        payCount = reader.GetInt32(0);
                    }
                    if (reader.NextResult() && reader.Read())
                    {
                        RefundOrderCount = reader.GetInt32(0);
                    }
                    if (reader.NextResult() && reader.Read())
                    {
                        ReplacementOrderCount = reader.GetInt32(0);
                    }
                    if (reader.NextResult() && reader.Read())
                    {
                        ReturnsOrderCount = reader.GetInt32(0);
                    }
                }
            }
            catch (Exception exception)
            {
                HttpContext.Current.Response.Write(builder.ToString() + "<br>" + exception.Message);
                HttpContext.Current.Response.End();
            }
        }

        public DataTable GetProductSales(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductSales_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, productSale.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, productSale.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, productSale.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildProductSaleQuery(productSale));
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            totalProductSales = (int) this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return table;
        }

        public DataTable GetProductSalesNoPage(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductSalesNoPage_Get");
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildProductSaleQuery(productSale));
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            totalProductSales = (int) this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return table;
        }

        public DataTable GetProductVisitAndBuyStatistics(SaleStatisticsQuery query, out int totalProductSales)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductVisitAndBuyStatistics_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildProductVisitAndBuyStatisticsQuery(query));
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            totalProductSales = (int) this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return table;
        }

        public DataTable GetProductVisitAndBuyStatisticsNoPage(SaleStatisticsQuery query, out int totalProductSales)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT ProductName,VistiCounts,SaleCounts as BuyCount ,(SaleCounts/(case when VistiCounts=0 then 1 else VistiCounts end))*100 as BuyPercentage ");
            builder.Append("FROM Hishop_Products WHERE SaleCounts>0 ORDER BY BuyPercentage DESC;");
            builder.Append("SELECT COUNT(*) as TotalProductSales FROM Hishop_Products WHERE SaleCounts>0;");
            sqlStringCommand.CommandText = builder.ToString();
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    table = DataHelper.ConverDataReaderToDataTable(reader);
                }
                if (reader.NextResult() && reader.Read())
                {
                    totalProductSales = (int) reader["TotalProductSales"];
                    return table;
                }
                totalProductSales = 0;
            }
            return table;
        }

        public DbQueryResult GetSaleOrderLineItemsStatistics(SaleStatisticsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat("orderDate >= '{0}'", query.StartDate.Value);
            }
            if (query.EndDate.HasValue)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("orderDate <= '{0}'", query.EndDate.Value.AddDays(1.0));
            }
            if (builder.Length > 0)
            {
                builder.Append(" AND ");
            }
            builder.AppendFormat("OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_SaleDetails", "OrderId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }
        public DbQueryResult GetSaleOrderLineItemsStatisticsNoPage(SaleStatisticsQuery query)
        {
            DbQueryResult result = new DbQueryResult();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_Hishop_SaleDetails WHERE 1=1");
            if (query.StartDate.HasValue)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" AND OrderDate >= '{0}'", query.StartDate);
            }
            if (query.EndDate.HasValue)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" AND OrderDate <= '{0}'", query.EndDate.Value.AddDays(1.0));
            }
            sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format("AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                result.Data = DataHelper.ConverDataReaderToDataTable(reader);
            }
            return result;
        }

        public DbQueryResult GetSaleTargets()
        {
            DbQueryResult result = new DbQueryResult();
            string query = string.Empty;
            query = string.Format("select (select Count(OrderId) from Hishop_orders WHERE OrderStatus != {0} AND OrderStatus != {1}  AND OrderStatus != {2}) as OrderNumb,", 1, 4, 9) + string.Format("(select isnull(sum(OrderTotal),0) - isnull(sum(RefundAmount),0) from hishop_orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}) as OrderPrice, ", 1, 4, 9) + " (select COUNT(*) from aspnet_Members) as UserNumb,  (select count(*) from aspnet_Members where UserID in (select userid from Hishop_orders)) as UserOrderedNumb,  ISNULL((select sum(VistiCounts) from Hishop_products),0) as ProductVisitNumb ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                result.Data = DataHelper.ConverDataReaderToDataTable(reader);
            }
            return result;
        }

        public StatisticsInfo GetStatistics()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT  (SELECT COUNT(OrderId) FROM Hishop_Orders WHERE OrderStatus = 2 OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest')) AS orderNumbWaitConsignment, (select count(GroupBuyId) from Hishop_GroupBuy where Status = 5) as groupBuyNumWaitRefund,  isnull((select sum(OrderTotal)-isnull(sum(RefundAmount),0) from hishop_orders where (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)   and OrderDate>='" + DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date) + "'),0) as orderPriceToday, isnull((select sum(OrderProfit) from Hishop_Orders where  (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)  and OrderDate>='" + DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date) + "'),0) as orderProfitToday, (select count(*) from aspnet_Members where CreateDate>='" + DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date) + "' ) as userNewAddToday,(select count(*) from Hishop_Orders where datediff(dd,getdate(),OrderDate)=0 and (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)) as todayFinishOrder,(select count(*) from Hishop_Orders where datediff(dd,getdate()-1,OrderDate)=0 and (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)) as yesterdayFinishOrder, isnull((select sum(OrderTotal)-isnull(sum(RefundAmount),0) from hishop_orders where (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)   and datediff(dd,getdate()-1,OrderDate)=0),0) as orderPriceYesterDay,(select count(*) from aspnet_Members where datediff(dd,getdate()-1,CreateDate)=0) as userNewAddYesterToday,(select count(*) from aspnet_Members) as TotalMembers,(select count(*) from Hishop_Products where SaleStatus!=0) as TotalProducts,(select count(*) from aspnet_Members where datediff(dd,getdate(),VipCardDate)=0 and VipCardNumber IS NOT NULL) as TodayVipCardNumber,(select count(*) from aspnet_Members where datediff(dd,getdate()-1,VipCardDate)=0 and VipCardNumber IS NOT NULL) as YesterTodayVipCardNumber, isnull((select sum(OrderTotal)-isnull(sum(RefundAmount),0) from hishop_orders where (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)   and datediff(dd,OrderDate,getdate())<=30),0) as orderPriceMonth");
            StatisticsInfo info = new StatisticsInfo();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info.OrderNumbWaitConsignment = (int) reader["orderNumbWaitConsignment"];
                    info.OrderPriceToday = (decimal) reader["orderProfitToday"];
                    info.OrderProfitToday = (decimal) reader["orderPriceToday"];
                    info.UserNewAddToday = (int) reader["userNewAddToday"];
                    info.TodayFinishOrder = (int) reader["todayFinishOrder"];
                    info.YesterdayFinishOrder = (int) reader["yesterdayFinishOrder"];
                    info.UserNewAddYesterToday = (int) reader["userNewAddYesterToday"];
                    info.TotalMembers = (int) reader["TotalMembers"];
                    info.TotalProducts = (int) reader["TotalProducts"];
                    info.TodayVipCardNumber = (int) reader["TodayVipCardNumber"];
                    info.YesterTodayVipCardNumber = (int) reader["YesterTodayVipCardNumber"];
                    info.OrderPriceMonth = (decimal) reader["OrderPriceMonth"];
                    info.GroupBuyNumWaitRefund = (int) reader["groupBuyNumWaitRefund"];
                    info.OrderPriceYesterday = (decimal) reader["orderPriceYesterDay"];
                }
            }
            return info;
        }

        public OrderStatisticsInfo GetUserOrders(OrderQuery userOrder)
        {
            OrderStatisticsInfo info = new OrderStatisticsInfo();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_OrderStatistics_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, userOrder.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, userOrder.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, userOrder.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildUserOrderQuery(userOrder));
            this.database.AddOutParameter(storedProcCommand, "TotalUserOrders", DbType.Int32, 4);

            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                info.OrderTbl = DataHelper.ConverDataReaderToDataTable(reader);
                if (reader.NextResult())
                {
                    reader.Read();
                    if (reader["OrderTotal"] != DBNull.Value)
                    {
                        info.TotalOfPage += (decimal) reader["OrderTotal"];
                    }
                    if (reader["Profits"] != DBNull.Value)
                    {
                        info.ProfitsOfPage += (decimal) reader["Profits"];
                    }
                }
                if (reader.NextResult())
                {
                    reader.Read();
                    if (reader["OrderTotal"] != DBNull.Value)
                    {
                        info.TotalOfSearch += (decimal) reader["OrderTotal"];
                    }
                    if (reader["Profits"] != DBNull.Value)
                    {
                        info.ProfitsOfSearch += (decimal) reader["Profits"];
                    }
                }
            }
            info.TotalCount = (int) this.database.GetParameterValue(storedProcCommand, "TotaluserOrders");
            return info;
        }

        public OrderStatisticsInfo GetUserOrdersNoPage(OrderQuery userOrder)
        {
            OrderStatisticsInfo info = new OrderStatisticsInfo();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_OrderStatisticsNoPage_Get");
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildUserOrderQuery(userOrder));
            this.database.AddOutParameter(storedProcCommand, "TotalUserOrders", DbType.Int32, 4);
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                info.OrderTbl = DataHelper.ConverDataReaderToDataTable(reader);
                if (reader.NextResult())
                {
                    reader.Read();
                    if (reader["OrderTotal"] != DBNull.Value)
                    {
                        info.TotalOfSearch += (decimal) reader["OrderTotal"];
                    }
                    if (reader["Profits"] != DBNull.Value)
                    {
                        info.ProfitsOfSearch += (decimal) reader["Profits"];
                    }
                }
            }
            info.TotalCount = (int) this.database.GetParameterValue(storedProcCommand, "TotaluserOrders");
            return info;
        }

        public IList<UserStatisticsInfo> GetUserStatistics(Pagination page, out int totalRegionsUsers)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TopRegionId as RegionId,COUNT(UserId) as UserCounts,(select count(*) from aspnet_Members) as AllUserCounts FROM aspnet_Members  GROUP BY TopRegionId ");
            IList<UserStatisticsInfo> list = new List<UserStatisticsInfo>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                UserStatisticsInfo item = null;
                while (reader.Read())
                {
                    item = DataMapper.PopulateUserStatistics(reader);
                    list.Add(item);
                }
                if (item != null)
                {
                    totalRegionsUsers = int.Parse(item.AllUserCounts.ToString());
                    return list;
                }
                totalRegionsUsers = 0;
            }
            return list;
        }

        public DataTable GetProductQuantity(string StartTime = "", string EndTime = "", int managerId = 0, string modename="")
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(StartTime))
            {
                builder.AppendFormat(" AND OrderDate >='{0}'", StartTime);
            }
            if (!string.IsNullOrEmpty(EndTime)) 
            {
                builder.AppendFormat(" AND OrderDate <='{0}'", EndTime);
            }
            if (managerId != 0)
            {
                builder.AppendFormat(" AND(Sender ='{0}' or (PaymentType='微信支付' and  ModeName='{1}'))", managerId, modename);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select(select productname from Hishop_Products where ProductId=hoi.productid)as ProductName,SUM(hoi.Quantity)as Quantity,SUM(HOI.GiveQuantity) as GiveQuantity from Hishop_Orders HO inner join  Hishop_OrderItems  HOI on HO.OrderId =HOI.OrderId where ho.OrderStatus=5 {0} GROUP BY ProductId", builder));           
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];          
        }
        public DataTable GetSaleTimesegment(DateTime? startTime, int sender = 0, int CategoryId = 0, int ProductId = 0)
        {
            string totalsql = "select ";

            for (int i = 0; i < 24; i++)
            {
                string sql = ("select ISNULL( sum(ItemAdjustedPrice*Quantity - (ItemAdjustedPrice/2)*HalfQuantity - (ItemAdjustedPrice*GiveQuantity) ) ,0) from Hishop_OrderItems hoi  left join Hishop_Orders ho on hoi.OrderId=ho.OrderId  left join   (select hp.CategoryId,hp.ProductId,hp.ProductName,hc.Name from Hishop_Products hp    left join Hishop_Categories hc  on hp.CategoryId=hc.CategoryId) as hu  on hoi.ProductId=hu.ProductId   where HO.OrderStatus=5 {0}");

                string senderWhere="",categoryWhere="",productWhere = "";
                if (sender != 0)
                {
                    senderWhere = string.Format(" AND(Sender ='{0}' or  (ho.ModeName =  (select storename from aspnet_Distributors where UserId= (select clientuserid from aspnet_Managers where UserId={0}) ) ) )", sender);
                }

                if (CategoryId != 0)
                {
                    categoryWhere = string.Format(" AND CategoryId ='{0}'", CategoryId);
                }

                if (ProductId != 0)
                {
                    productWhere = string.Format(" AND  hoi.ProductId ={0}", ProductId);
                }
            
                DateTime timeHour = DateTime.Parse(startTime.ToString()).AddHours(i);
                string timewhere = string.Format(" AND Ho.OrderDate >='{0}' and  HO.OrderDate<= '{1}' ", timeHour, timeHour.AddHours(1));
                sql = string.Format(" (" + sql, senderWhere + categoryWhere + productWhere) + timewhere + ") as t_" + i +",";

                totalsql += sql;
            }
            totalsql = totalsql.TrimEnd(',');
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(totalsql);

            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return  DataHelper.ConverDataReaderToDataTable(reader);
            }

        }

        public DataSet GetSaleInfoTables(string StartTime="", string EndTime="", int managerId = 0)
        {          
            DataSet dsTables = new DataSet();
            //查询条件拼接
            StringBuilder time = new StringBuilder();
            if (!string.IsNullOrEmpty(StartTime))
            {
                time.AppendFormat(" AND OrderDate >='{0}'", StartTime);
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                time.AppendFormat(" AND OrderDate <='{0}'", EndTime);
            }
            string today = " and orderdate > convert(varchar(10), getdate() , 120)";
            string pcorder = " and (username in ('[堂食用户]','[匿名用户]','[活动用户]') and (RealModeName != '微信扫码支付' or RealModeName is null) )";
            string wxorder = string.Format(" and (modename = (select storename from aspnet_Distributors where UserId= (select clientuserid from aspnet_Managers where UserId={0})) and sender is null )", managerId);
            string pcwxorder = " and ( username in ('[堂食用户]','[匿名用户]','[活动用户]') and RealModeName = '微信扫码支付' )";
            string sender = managerId == 0 ? "" : string.Format("and Sender = {0} ",managerId);
            string total = string.Format("or (modename = (select storename from aspnet_Distributors where UserId= (select clientuserid from aspnet_Managers where UserId={0})) and sender is null )",managerId);

            string sql = @"select isnull(sum(HOI.productTotal),0)productTotal,COUNT(*)orderCount,ISNULL(SUM(OrderTotal),0)OrderTotal,ISNULL(SUM(CouponValue),0)CouponTotal,ISNULL(SUM(DiscountAmount),0)DiscountTotal 
                        from Hishop_Orders HO left join (select orderid,ISNULL(SUM(quantity),0) as productTotal from Hishop_OrderItems group by OrderId) HOI on ho.OrderId=hoi.OrderId where OrderStatus = 5  {0}/*此处拼接对订单时间进行过滤的条件*/ {1}/*此处拼接对订单来源进行过滤的条件*/
                          {2} {3}";

            string sqlTotal = string.Format(sql, time, "", "and (Sender = " + managerId, total + ")");
            string sqlTotalPc = string.Format(sql, time, pcorder, sender, "");
            string sqlTotalWx = string.Format(sql, time, wxorder, "", "");

            string sqlTotalToday = string.Format(sql, today, "", "and (Sender = " + managerId, total + ")");
            string sqlTotalPcToday = string.Format(sql, today, pcorder, sender, "");
            string sqlTotalWxToday = string.Format(sql, today, wxorder, "" , "");

            string sqlTotalPcWx = string.Format(sql, time, pcwxorder, sender, "");//pc端的微信扫码支付订单
            string sqlTotalPcWxToday = string.Format(sql, today, pcwxorder, sender, "");//今日pc端的微信扫码支付订单          

            string sqlAllTables = sqlTotal + ";" + sqlTotalPc + ";" + sqlTotalWx + ";" + sqlTotalToday + ";" + sqlTotalPcToday + ";" + sqlTotalWxToday + ";" + sqlTotalPcWx + ";" + sqlTotalPcWxToday;

            dsTables = this.database.ExecuteDataSet(CommandType.Text,sqlAllTables);
            ////查询内容拼接
            //string orderCount = " Select COUNT(*)todayOrderCount From Hishop_Orders where OrderStatus = 5 ";
            //string orderTotal = " Select ISNULL(SUM(OrderTotal),0)todayOrderTotal From Hishop_Orders where OrderStatus = 5 ";
            //string couponTotal = "Select ISNULL(SUM(CouponValue),0)todayCouponTotal From Hishop_Orders where OrderStatus = 5 ";


            ///********总店统计********/
            ////今日订单总数
            //string sqlTodayOrderCount =  orderCount + today;
            ////今日订单总金额
            //string sqlTodayOrderTotal =  orderTotal + today;
            ////今日优惠券总金额
            //string sqlTodayCouponTotal = couponTotal + today;
            ///********总店统计End******/

            ///*pc端点餐总统计*/
            //string sqlTodayOrderCountPc = orderCount + today + pcOrder;
            //string sqlTodayOrderTotalPc = orderTotal + today + pcOrder;
            //string sqlTodayCouponTotalPc = couponTotal + today + pcOrder;
            ///*pc端点餐总统计End*/

            ///*移动端点餐总统计*/
            //string sqlTodayOrderCountWx = orderCount + today + wxOrder;
            //string sqlTodayOrderTotalWx = orderTotal + today + wxOrder;
            //string sqlTodayCouponTotalWx = couponTotal + today + wxOrder;
            ///*移动端点餐总统计End*/

            //if (managerId != 0)
            //{
            //    string senderSql = string.Format(" And Sender in (select UserId from aspnet_Managers where UserId = {0})",managerId);
            //    /*子账号的总统计*/
            //    //今日pc端订单总数
            //    string sqlTodayOrderCountPcBySender = orderCount + today + pcOrder + senderSql;
            //    //今日pc端订单总额
            //    string sqlTodayOrderTotalPcBySender = orderTotal + today + pcOrder + senderSql;
            //    //今日pc端优惠券总额
            //    string sqlTodayCouponTotalPcBySender = couponTotal + today + pcOrder + senderSql;
            //    //今日wx端订单总数
            //    string sqlTodayOrderCountWxBySender = orderCount + today + wxOrder + senderSql;
            //    //今日wx端订单总额
            //    string sqlTodayOrderTotalWxBySender = orderTotal + today + wxOrder + senderSql;
            //    //今日wx端优惠券总额
            //    string sqlTodayCouponTotalWxBySender = couponTotal + today + wxOrder + senderSql;
            //}
            
            //dsTables = this.database.ExecuteDataSet(
            return dsTables;
        }
    }
}

