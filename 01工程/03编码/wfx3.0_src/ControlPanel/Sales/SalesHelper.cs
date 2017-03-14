namespace Hidistro.ControlPanel.Sales
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Sales;
    using Hidistro.SqlDal.Sales;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public sealed class SalesHelper
    {
        private SalesHelper()
        {
        }

        public static bool AddExpressTemplate(string expressName, string xmlFile)
        {
            return new ExpressTemplateDao().AddExpressTemplate(expressName, xmlFile);
        }

        public static bool AddShipper(ShippersInfo shipper)
        {
            Globals.EntityCoding(shipper, true);
            return new ShipperDao().AddShipper(shipper);
        }

        public static PaymentModeActionStatus CreatePaymentMode(PaymentModeInfo paymentMode)
        {
            if (paymentMode == null)
            {
                return PaymentModeActionStatus.UnknowError;
            }
            Globals.EntityCoding(paymentMode, true);
            return new PaymentModeDao().CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Create);
        }

        public static bool CreateShippingMode(ShippingModeInfo shippingMode)
        {
            if (shippingMode == null)
            {
                return false;
            }
            return new ShippingModeDao().CreateShippingMode(shippingMode);
        }

        public static bool CreateShippingTemplate(ShippingModeInfo shippingMode)
        {
            return new ShippingModeDao().CreateShippingTemplate(shippingMode);
        }

        public static bool DeleteExpressTemplate(int expressId)
        {
            return new ExpressTemplateDao().DeleteExpressTemplate(expressId);
        }

        public static bool DeletePaymentMode(int modeId)
        {
            PaymentModeInfo paymentMode = new PaymentModeInfo
            {
                ModeId = modeId
            };
            return (new PaymentModeDao().CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Delete) == PaymentModeActionStatus.Success);
        }

        public static bool DeleteShipper(int shipperId)
        {
            return new ShipperDao().DeleteShipper(shipperId);
        }

        public static bool DeleteShippingMode(int modeId)
        {
            return new ShippingModeDao().DeleteShippingMode(modeId);
        }

        public static bool DeleteShippingTemplate(int templateId)
        {
            return new ShippingModeDao().DeleteShippingTemplate(templateId);
        }

        public static DataTable GetDaySaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
        {
            return new DateStatisticDao().GetDaySaleTotal(year, month, saleStatisticsType);
        }

        public static decimal GetDaySaleTotal(int year, int month, int day, SaleStatisticsType saleStatisticsType)
        {
            return new DateStatisticDao().GetDaySaleTotal(year, month, day, saleStatisticsType);
        }

        public static IList<string> GetExpressCompanysByMode(int modeId)
        {
            return new ShippingModeDao().GetExpressCompanysByMode(modeId);
        }

        public static DataTable GetExpressTemplates()
        {
            return new ExpressTemplateDao().GetExpressTemplates(null);
        }

        public static DataTable GetIsUserExpressTemplates()
        {
            return new ExpressTemplateDao().GetExpressTemplates(true);
        }

        public static DataTable GetMemberStatistics(SaleStatisticsQuery query, out int totalProductSales)
        {
            return new SaleStatisticDao().GetMemberStatistics(query, out totalProductSales);
        }

        public static DataTable GetMemberStatisticsNoPage(SaleStatisticsQuery query)
        {
            return new SaleStatisticDao().GetMemberStatisticsNoPage(query);
        }

        public static DataTable GetMonthSaleTotal(int year, SaleStatisticsType saleStatisticsType)
        {
            return new DateStatisticDao().GetMonthSaleTotal(year, saleStatisticsType);
        }

        public static decimal GetMonthSaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
        {
            return new DateStatisticDao().GetMonthSaleTotal(year, month, saleStatisticsType);
        }

        public static void GetNewlyOrdersCountAndPayCount(DateTime lastTime, out int ordersCount, out int payCount, out int RefundOrderCount, out int ReplacementOrderCount, out int ReturnsOrderCount, string shipDistributorStoreName = "",int agentUserId=0)
        {
            ordersCount = 0;
            payCount = 0;
            RefundOrderCount = 0;
            ReplacementOrderCount = 0;
            ReturnsOrderCount = 0;
            new SaleStatisticDao().GetNewlyOrdersCountAndPayCount(lastTime, out ordersCount, out payCount, out RefundOrderCount, out ReplacementOrderCount, out ReturnsOrderCount,shipDistributorStoreName,agentUserId);
        }

        public static PaymentModeInfo GetPaymentMode(int modeId)
        {
            return new PaymentModeDao().GetPaymentMode(modeId);
        }

        public static PaymentModeInfo GetPaymentMode(string gateway)
        {
            return new PaymentModeDao().GetPaymentMode(gateway);
        }

        public static IList<PaymentModeInfo> GetPaymentModes()
        {
            return new PaymentModeDao().GetPaymentModes();
        }

        public static DataTable GetProductSales(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            if (productSale == null)
            {
                totalProductSales = 0;
                return null;
            }
            return new SaleStatisticDao().GetProductSales(productSale, out totalProductSales);
        }

        public static DataTable GetProductSalesNoPage(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            if (productSale == null)
            {
                totalProductSales = 0;
                return null;
            }
            return new SaleStatisticDao().GetProductSalesNoPage(productSale, out totalProductSales);
        }

        public static DataTable GetProductVisitAndBuyStatistics(SaleStatisticsQuery query, out int totalProductSales)
        {
            return new SaleStatisticDao().GetProductVisitAndBuyStatistics(query, out totalProductSales);
        }

        public static DataTable GetProductVisitAndBuyStatisticsNoPage(SaleStatisticsQuery query, out int totalProductSales)
        {
            return new SaleStatisticDao().GetProductVisitAndBuyStatisticsNoPage(query, out totalProductSales);
        }

        public static DbQueryResult GetSaleOrderLineItemsStatistics(SaleStatisticsQuery query)
        {
            return new SaleStatisticDao().GetSaleOrderLineItemsStatistics(query);
        }
        public static DbQueryResult GetSaleOrderLineItemsStatisticsNoPage(SaleStatisticsQuery query)
        {
            return new SaleStatisticDao().GetSaleOrderLineItemsStatisticsNoPage(query);
        }

        public static DbQueryResult GetSaleTargets()
        {
            return new SaleStatisticDao().GetSaleTargets();
        }

        public static ShippersInfo GetShipper(int shipperId)
        {
            return new ShipperDao().GetShipper(shipperId);
        }
        public static ShippersInfo GetIsDefault(int isDefault)
        {
            return new ShipperDao().GetIsDefault(isDefault);
        }

        public static IList<ShippersInfo> GetShippers(bool includeDistributor)
        {
            return new ShipperDao().GetShippers(includeDistributor);
        }

        public static DataTable GetShippingAllTemplates()
        {
            return new ShippingModeDao().GetShippingAllTemplates();
        }

        public static ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
        {
            return new ShippingModeDao().GetShippingMode(modeId, includeDetail);
        }

        public static IList<ShippingModeInfo> GetShippingModes()
        {
            return new ShippingModeDao().GetShippingModes();
        }

        public static ShippingModeInfo GetShippingTemplate(int templateId, bool includeDetail)
        {
            return new ShippingModeDao().GetShippingTemplate(templateId, includeDetail);
        }

        public static DbQueryResult GetShippingTemplates(Pagination pagin)
        {
            return new ShippingModeDao().GetShippingTemplates(pagin);
        }

        public static StatisticsInfo GetStatistics()
        {
            return new SaleStatisticDao().GetStatistics();
        }

        public static IList<UserStatisticsForDate> GetUserAdd(int? year, int? month, int? days)
        {
            return new DateStatisticDao().GetUserAdd(year, month, days);
        }

        public static OrderStatisticsInfo GetUserOrders(OrderQuery userOrder)
        {
            return new SaleStatisticDao().GetUserOrders(userOrder);
        }

        public static OrderStatisticsInfo GetUserOrdersNoPage(OrderQuery userOrder)
        {
            return new SaleStatisticDao().GetUserOrdersNoPage(userOrder);
        }

        public static IList<UserStatisticsInfo> GetUserStatistics(Pagination page, out int totalProductSaleVisits)
        {
            if (page == null)
            {
                totalProductSaleVisits = 0;
                return null;
            }
            return new SaleStatisticDao().GetUserStatistics(page, out totalProductSaleVisits);
        }

        public static DataTable GetWeekSaleTota(SaleStatisticsType saleStatisticsType)
        {
            return new DateStatisticDao().GetWeekSaleTota(saleStatisticsType);
        }

        public static decimal GetYearSaleTotal(int year, SaleStatisticsType saleStatisticsType)
        {
            return new DateStatisticDao().GetYearSaleTotal(year, saleStatisticsType);
        }

        public static void SetDefalutShipper(int shipperId)
        {
            new ShipperDao().SetDefalutShipper(shipperId);
        }

        public static bool SetExpressIsUse(int expressId)
        {
            return new ExpressTemplateDao().SetExpressIsUse(expressId);
        }

        public static void SwapPaymentModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
        {
            new PaymentModeDao().SwapPaymentModeSequence(modeId, replaceModeId, displaySequence, replaceDisplaySequence);
        }

        public static void SwapShippingModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
        {
            new ShippingModeDao().SwapShippingModeSequence(modeId, replaceModeId, displaySequence, replaceDisplaySequence);
        }

        public static bool UpdateExpressTemplate(int expressId, string expressName)
        {
            return new ExpressTemplateDao().UpdateExpressTemplate(expressId, expressName);
        }

        public static PaymentModeActionStatus UpdatePaymentMode(PaymentModeInfo paymentMode)
        {
            if (paymentMode == null)
            {
                return PaymentModeActionStatus.UnknowError;
            }
            Globals.EntityCoding(paymentMode, true);
            return new PaymentModeDao().CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Update);
        }

        public static bool UpdateShipper(ShippersInfo shipper)
        {
            Globals.EntityCoding(shipper, true);
            return new ShipperDao().UpdateShipper(shipper);
        }

        public static bool UpdateShippingTemplate(ShippingModeInfo shippingMode)
        {
            return new ShippingModeDao().UpdateShippingTemplate(shippingMode);
        }

        public static bool UpdateShippMode(ShippingModeInfo shippingMode)
        {
            if (shippingMode == null)
            {
                return false;
            }
            Globals.EntityCoding(shippingMode, true);
            return new ShippingModeDao().UpdateShippingMode(shippingMode);
        }

        public static bool AddStreetInfo(string regionCode, string streetName)
        {
            return new ShippingModeDao().AddStreetInfo(regionCode, streetName);
        }

        public static DbQueryResult GetStreetInfo(StreetInfoQuery query)
        {
            return new ShippingModeDao().GetStreetInfo(query);
        }

        public static DataTable GetStreetsInfo()
        {
            return new ShippingModeDao().GetStreetsInfo();
        }

        public static bool DeleteDistributorRegions(int distributorId)
        {
            return new ShippingModeDao().DeleteDistributorRegions(distributorId);
        }

        public static DataTable GetAllStreetRegionId()
        {
            return new ShippingModeDao().GetAllStreetRegionId();
        }

        public static DataTable GetStreetInfo(string regionCode)
        {
            return new ShippingModeDao().GetStreetInfo(regionCode);
        }

        public static bool DeleteStreetInfo(string streetId)
        {
            return new ShippingModeDao().DeleteStreetInfo(streetId);
        }

        public static bool EditStreetInfo(string streetId, string streetName)
        {
            return new ShippingModeDao().EditStreetInfo(streetId, streetName);
        }

        public static DataTable GetDistributorRegions(int distributorId)
        {
            return new ShippingModeDao().GetDistributorRegions(distributorId);
        }

        public static DataTable GetDistributorRegions(string streetId)
        {
            return new ShippingModeDao().GetDistributorRegions(streetId);
        }

        public static bool AddDistributorRegions(int distributorId, string streetIds)
        {
            DataTable dt = new ShippingModeDao().GetDistributorRegions(distributorId);
            IList<string> listStreetId = streetIds.Split(',');
            //循环判断是否有重复值
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int o = 0; o < listStreetId.Count; o++)
                {
                    if (dt.Rows[0]["streetId"].ToString() == listStreetId[o])//如果重复就返回false
                    {
                        return false;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return new ShippingModeDao().AddDistributorRegions(distributorId, streetIds);
        }

        public static DataTable GetProductQuantity(string StartTime = "", string endTime = "", int managerId = 0, string modename="")
        {
            return new SaleStatisticDao().GetProductQuantity(StartTime, endTime, managerId, modename);
        }

        public static DataSet GetSaleInfoTables(string StartTime="",string endTime="",int managerId = 0)
        {
            return new SaleStatisticDao().GetSaleInfoTables(StartTime, endTime,managerId);
        }

        public static DataTable GetSaleTimesegment(DateTime? StartTime, int managerId = 0, int CategoryId = 0, int ProductId = 0)
        {
           return new SaleStatisticDao().GetSaleTimesegment(StartTime,  managerId, CategoryId, ProductId);
        }
    }
}

