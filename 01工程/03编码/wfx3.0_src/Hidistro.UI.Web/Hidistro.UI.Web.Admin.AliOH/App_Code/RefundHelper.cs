namespace Hidistro.UI.Web.App_Code
{
    using Hidistro.Core;
    using Hishop.Plugins;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Sales;
    //using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Member;
    using Hishop.Weixin.Pay;
    using Hishop.Weixin.Pay.Domain;
    using System;
    using System.Collections.Generic;
    using System.Web;

    public class RefundHelper
    {
        public static string GenerateRefundOrderId()
        {
            string str = string.Empty;
            Random random = new Random();
            for (int i = 0; i < 7; i++)
            {
                int num = random.Next();
                str = str + ((char) (0x30 + ((ushort) (num % 10)))).ToString();
            }
            return (DateTime.Now.ToString("yyyyMMdd") + str);
        }

        public static bool IsBackReturn(string refundGateway)
        {
            return AllowRefundGateway.Contains(refundGateway);
        }

        public static string SendAlipayRefundRequest(OrderInfo order, decimal RefundMoney, string RefundOrderId)
        {
            string str = "backnotify";
            PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(order.PaymentTypeId);
            RefundMoney = (RefundMoney == 0M) ? order.GetTotal() : RefundMoney;
            string str2 = "hishop.plugins.refund.alipaydirect.directrequest";
            string returnUrl = string.Format("http://{0}/pay/RefundReturn.aspx?HIGW={1}", HttpContext.Current.Request.Url.Host, str2);
            string notifyUrl = string.Format("http://{0}/pay/RefundNotify.aspx?HIGW={1}", HttpContext.Current.Request.Url.Host, order.Gateway);
            string[] orderId = new string[] { order.GatewayOrderId };
            decimal[] amount = new decimal[] { order.GetTotal() };
            decimal[] refundaAmount = new decimal[] { RefundMoney };
            string[] body = new string[] { order.RefundRemark };
            try
            {
                //RefundRequest.CreateInstance(str2, HiCryptographer.Decrypt(paymentMode.Settings), orderId, RefundOrderId, amount, refundaAmount, body, order.EmailAddress, DateTime.Now, returnUrl, notifyUrl, "退款").SendRequest();
            }
            catch (Exception exception)
            {
                //Globals.WriteLog("/log/backreturn.txt", string.Format("时间：{0}    支付宝原路返回错误:{1}    订单号：{2}      金额：{3}    网关：{4}<br><br>", new object[] { DateTime.Now, exception.Message, order.OrderId, RefundMoney, order.Gateway }));
                str = "ERROR";
            }
            return str;
        }

        public static string SendRefundRequest(OrderInfo order, decimal RefundMoney, string RefundOrderId)
        {
            string str = "";
            string str2 = order.Gateway.ToLower();
            if (str2 == null)
            {
                return str;
            }
            if (!(str2 == "hishop.plugins.payment.weixinrequest") && !(str2 == "hishop.plugins.payment.wxqrcode.wxqrcoderequest"))
            {
                if (((str2 != "hishop.plugins.payment.alipaydirect.directrequest") && (str2 != "hishop.plugins.payment.alipay_bank.bankrequest")) && ((str2 != "hishop.plugins.payment.alipayassure.assurerequest") && (str2 != "hishop.plugins.payment.alipayqrcode.arcoderequest")))
                {
                    return str;
                }
            }
            else
            {
                return SendWxRefundRequest(order, RefundMoney, RefundOrderId);
            }
            return SendAlipayRefundRequest(order, RefundMoney, RefundOrderId);
        }

        public static string SendWxRefundRequest(OrderInfo order, decimal RefundMoney, string RefundOrderId)
        {
            //if (RefundMoney == 0M)
            //{
            //    RefundMoney = order.GetTotal();
            //}
            //Hishop.Weixin.Pay.Domain.RefundInfo info = new Hishop.Weixin.Pay.Domain.RefundInfo();
            //SiteSettings masterSettings = Hidistro.Core.SettingsManager.GetMasterSettings(true);
            //info.out_refund_no = RefundOrderId;
            //info.out_trade_no = order.OrderId;
            //info.RefundFee = new decimal?((int) (RefundMoney * 100M));
            //info.TotalFee = new decimal?((int) (order.GetTotal() * 100M));
            //info.NotifyUrl = string.Format("http://{0}/pay/wxRefundNotify.aspx", HttpContext.Current.Request.Url.Host);
            //PayConfig config = new PayConfig {
            //    AppId = masterSettings.WeixinAppId,
            //    AppSecret = masterSettings.WeixinAppSecret,
            //    Key = masterSettings.WeixinPartnerKey,
            //    MchID = masterSettings.WeixinPartnerID,
            //    SSLCERT_PATH = masterSettings.WeixinCertPath,
            //    SSLCERT_PASSWORD = masterSettings.WeixinCertPassword
            //};
            string str = "";
            //try
            //{
            //    str = Refund.SendRequest(info, config);
            //}
            //catch (Exception exception)
            //{
            //    //Globals.WriteLog("/log/backreturn.txt", string.Format("时间：{0}    微信原路返回错误:{1}    订单号：{2}      金额：{3}    网关：{4}<br><br>", new object[] { DateTime.Now, exception.Message, order.OrderId, RefundMoney, order.Gateway }));
            //    str = "ERROR";
            //}
            //if (str.ToUpper() == "SUCCESS")
            //{
            //    str = "";
            //}
            return str;
        }

        public static IList<string> AllowRefundGateway
        {
            get
            {
                return new List<string> { "hishop.plugins.payment.weixinrequest", "hishop.plugins.payment.wxqrcode.wxqrcoderequest", "hishop.plugins.payment.alipaydirect.directrequest", "hishop.plugins.payment.alipay_bank.bankrequest", "hishop.plugins.payment.alipayassure.assurerequest", "hishop.plugins.payment.alipayqrcode.arcoderequest" };
            }
        }
    }
}

