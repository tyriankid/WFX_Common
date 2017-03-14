using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using Hishop.Plugins;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.Web.Admin
{
	public class QuickPay : AdminPage
	{
        protected Button btnSuccess;
        protected DataList dlstOrders;
        protected TextBox snCode;
        protected Button btnSubmit;

        private DataTable orders=new DataTable();
        public DataTable Orders
        {
            get {
                return (DataTable)ViewState["orders"];
            }
            set {
                ViewState["orders"] = orders;
            }
        }

        protected static string orderID;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.Orders = orders;
                this.btnSuccess.Click += new System.EventHandler(this.btnSuccess_Click);
                this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
			} 
		}

        [System.Web.Services.WebMethod]
        public static string AjaxServiceTest(string str)
        {
            if (OrderHelper.GetSimpleOrderInfo(str).Rows.Count > 0 && Convert.ToInt32(OrderHelper.GetSimpleOrderInfo(str).Rows[0]["OrderStatus"]) == 2) //只有当订单状态为待发货时才会出现在列表中
            {
                orderID = str;
                return string.Format("success");
            }
            else
            {
                return string.Format("fail");
            }
        }

        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            for (int i = 0; i < this.Orders.Rows.Count; i++)
            {
                OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.Orders.Rows[i]["OrderId"].ToString());
                if (orderInfo != null)
                {
                    ManagerInfo currentManager = ManagerHelper.GetCurrentManager();//获取目前的管理员信息
                    if (orderInfo.GroupBuyId > 0 && orderInfo.GroupBuyStatus != GroupBuyStatus.Success)
                    {
                        this.ShowMsg("当前订单为团购订单，团购活动还未成功结束，所以不能发货", false);
                    }
                    else
                    {
                        if (!orderInfo.CheckAction(OrderActions.SELLER_SEND_GOODS))
                        {
                            this.ShowMsg("当前订单状态没有付款或不是等待发货的订单，所以不能发货", false);
                        }
                        else
                        {
                            orderInfo.RealShippingModeId = 1;//固定为1,为第一种快递方式(店家初始化时自己配置的第一种方式是什么这里就是什么)
                            orderInfo.RealModeName = "快速收银";
                            orderInfo.ShipOrderNumber = "";
                            if (OrderHelper.SendGoods(orderInfo))
                            {
                                SendNoteInfo info5 = new SendNoteInfo();
                                info5.NoteId = Globals.GetGenerateId();
                                info5.OrderId = orderInfo.OrderId;
                                info5.Operator = currentManager.UserName;
                                info5.Remark = "后台" + info5.Operator + "发货成功";
                                OrderHelper.SaveSendNote(info5);
                                MemberInfo member = MemberHelper.GetMember(orderInfo.UserId);
                                Messenger.OrderShipping(orderInfo, member);
                                if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && orderInfo.GatewayOrderId.Trim().Length > 0)
                                {
                                    if (orderInfo.Gateway == "hishop.plugins.payment.ws_wappay.wswappayrequest")
                                    {
                                        PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(orderInfo.PaymentTypeId);
                                        if (paymentMode != null)
                                        {
                                            PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单发货", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[]
														{
															paymentMode.Gateway
														})), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[]
														{
															paymentMode.Gateway
														})), "").SendGoods(orderInfo.GatewayOrderId, orderInfo.RealModeName, orderInfo.ShipOrderNumber, "EXPRESS");
                                        }
                                    }
                                    if (orderInfo.Gateway == "hishop.plugins.payment.weixinrequest")
                                    {
                                        //SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                                        PayClient client = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey);
                                        DeliverInfo deliver = new DeliverInfo
                                        {
                                            TransId = orderInfo.GatewayOrderId,
                                            OutTradeNo = orderInfo.OrderId,
                                            OpenId = MemberHelper.GetMember(orderInfo.UserId).OpenId
                                        };
                                        client.DeliverNotify(deliver);
                                    }
                                }
                                orderInfo.OnDeliver();
                                //this.ShowMsg("发货成功", true);

                                //发送成功后,确认收货
                                bool flag = false;
                                orderInfo = ShoppingProcessor.GetOrderInfo(this.Orders.Rows[i]["OrderId"].ToString());
                                Dictionary<string, LineItemInfo> lineItems = orderInfo.LineItems;
                                LineItemInfo lineItemInfo = new LineItemInfo();
                                foreach (KeyValuePair<string, LineItemInfo> lineItem in lineItems)
                                {
                                    lineItemInfo = lineItem.Value;
                                    if (lineItemInfo.OrderItemsStatus != OrderStatus.ApplyForRefund && lineItemInfo.OrderItemsStatus != OrderStatus.ApplyForReturns)
                                    {
                                        continue;
                                    }
                                    flag = true;
                                }
                                if (flag)
                                {
                                    Response.Write("<script>alert('订单中商品有退货(款)不允许完成');</script>");
                                    return;
                                }
                                if (orderInfo == null || !MemberProcessor.ConfirmOrderFinish(orderInfo))
                                {
                                    Response.Write("<script>alert('订单当前状态不允许完成');</script>");
                                    return;
                                }
                                DistributorsBrower.UpdateCalculationCommission(orderInfo);//增加佣金记录、更新分销商的有效推广佣金和订单总额
                                MemberInfo currentMember = MemberProcessor.GetMember(orderInfo.UserId);
                                int num = 0;
                                if (masterSettings.IsRequestDistributor && !string.IsNullOrEmpty(masterSettings.FinishedOrderMoney.ToString()) && currentMember.Expenditure >= masterSettings.FinishedOrderMoney)
                                {
                                    num = 1;
                                }
                                foreach (LineItemInfo value in orderInfo.LineItems.Values)
                                {
                                    if (value.OrderItemsStatus.ToString() != OrderStatus.SellerAlreadySent.ToString())
                                    {
                                        continue;
                                    }
                                    ShoppingProcessor.UpdateOrderGoodStatu(orderInfo.OrderId, value.SkuId, 5);
                                }
                                DistributorsInfo distributorsInfo = new DistributorsInfo();
                                distributorsInfo = DistributorsBrower.GetUserIdDistributors(orderInfo.UserId);
                                if (distributorsInfo != null && distributorsInfo.UserId > 0)
                                {
                                    num = 0;
                                }
                                this.Orders.Clear();
                                dlstOrders.DataSource = this.Orders;
                                dlstOrders.DataBind();
                                this.ShowMsg("收银成功", true);
                                
                            }
                            else
                            {
                                this.ShowMsg("发货失败", false);
                            }


                        }
                    }
                }
            }
        }

        protected void btnSuccess_Click(object sender, System.EventArgs e)
        {
            if (this.Orders.Rows.Count == 0)
            {
                this.orders = OrderHelper.GetSimpleOrderInfo(orderID).Clone();
                this.Orders = this.orders;
            }
            if (Orders.Rows.Count > 0)
            {
                for (int i = 0; i < Orders.Rows.Count; i++)
                {
                    if (Orders.Rows[i]["OrderId"].ToString() != orderID)
                    {
                        this.Orders.Rows.Add(OrderHelper.GetSimpleOrderInfo(orderID).Rows[0].ItemArray);
                        break;
                    }
                    else
                    {
                        Response.Write("<script>alert('请勿重复扫码!');</script>");
                        break;
                    }
                }
            }
            else
            {
                this.Orders.Rows.Add(OrderHelper.GetSimpleOrderInfo(orderID).Rows[0].ItemArray);
            }
                
            dlstOrders.DataSource = this.Orders;
            dlstOrders.DataBind();
        }



	}
}
