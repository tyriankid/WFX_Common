using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Notify;
using System;
using System.Web.UI;
namespace Hidistro.UI.Web.Pay
{
    public class wx_Pay : System.Web.UI.Page
    {
        protected OrderInfo Order;
        protected string OrderId;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            WxPayAPI.ResultNotify resultNotify = new WxPayAPI.ResultNotify(this);
            string[] strValues = resultNotify.ProcessNotify();
            if (strValues.Length == 2 && !string.IsNullOrEmpty(strValues[0]) && !string.IsNullOrEmpty(strValues[1]))
            {
                WxPayAPI.Log.Info("wx_Pay", "**************开始支付回调***************");
                this.OrderId = strValues[0];
                WxPayAPI.Log.Info("wx_Pay", "**************订单号：" + this.OrderId);
                this.Order = ShoppingProcessor.GetOrderInfo(this.OrderId);
                if (this.Order == null)
                {
                    WxPayAPI.Log.Info("wx_Pay", "**************根据支付回调的订单号得到订单实体失败***************");
                    base.Response.Write("success");
                    //ResponseWrite(true, "success");
                }
                else
                {
                    WxPayAPI.Log.Info("wx_Pay", "**************根据支付回调的订单号得到订单实体成功***************");
                    this.Order.GatewayOrderId = strValues[1];
                    WxPayAPI.Log.Info("wx_Pay", "**************交易流水号：" + this.Order.GatewayOrderId);
                    this.UserPayOrder();
                }
            }
            else
                base.Response.Write("fail");

            /*
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			PayNotify payNotify = new NotifyClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey).GetPayNotify(base.Request.InputStream);
			if (payNotify != null)
			{
                //WxPayAPI.Log.Debug(this.GetType().ToString(), payNotify.nonce_str);
                //WxPayAPI.Log.Debug(this.GetType().ToString(), payNotify.PayInfo.OutTradeNo);
				this.OrderId = payNotify.PayInfo.OutTradeNo;
				this.Order = ShoppingProcessor.GetOrderInfo(this.OrderId);
				if (this.Order == null)
				{
					base.Response.Write("success");
				}
				else
				{
					this.Order.GatewayOrderId = payNotify.PayInfo.TransactionId;
					this.UserPayOrder();
				}
            }
                */
        }
        private void UserPayOrder()
        {
            if (this.Order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
            {
                base.Response.Write("success");
            }
            else
            {
                if (this.Order.CheckAction(OrderActions.BUYER_PAY) && MemberProcessor.UserPayOrder(this.Order))
                {
                    if (this.Order.UserId != 0 && this.Order.UserId != 1100)
                    {
                        MemberInfo member = MemberProcessor.GetMember(this.Order.UserId);
                        if (member != null)
                        {
                            Messenger.OrderPayment(member, this.OrderId, this.Order.GetTotal());
                        }
                    }
                    this.Order.OnPayment();
                    base.Response.Write("success");
                }
            }
        }


        private void ResponseWrite(bool isSuccess, string msg)
        {
            WxPayAPI.WxPayData res = new WxPayAPI.WxPayData();
            res.SetValue("return_code", isSuccess ? "SUCCESS" : "FAIL");
            res.SetValue("return_msg", msg);
            base.Response.Write(res.ToXml());
            base.Response.End();
        }

    }
}
