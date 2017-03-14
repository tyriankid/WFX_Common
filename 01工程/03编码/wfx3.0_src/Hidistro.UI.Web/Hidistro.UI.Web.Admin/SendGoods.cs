using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Plugins;
using Hishop.Weixin.MP.Api;
using Hishop.Weixin.MP.Domain;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.OrderSendGoods)]
	public class SendGoods : AdminPage
	{
		protected System.Web.UI.WebControls.Button btnSendGoods;
		protected ExpressRadioButtonList expressRadioButtonList;
		protected Order_ItemsList itemsList;
		protected System.Web.UI.WebControls.Label lblOrderId;
		protected FormatedTimeLabel lblOrderTime;
		protected System.Web.UI.WebControls.Literal litReceivingInfo;
		protected System.Web.UI.WebControls.Label litRemark;
		protected System.Web.UI.WebControls.Literal litShippingModeName;
		protected System.Web.UI.WebControls.Label litShipToDate;
        protected System.Web.UI.HtmlControls.HtmlInputHidden specialHideShow;
		private string orderId;
		protected ShippingModeRadioButtonList radioShippingMode;
		protected System.Web.UI.WebControls.TextBox txtShipOrderNumber;
        protected System.Web.UI.WebControls.TextBox txtArriveTime;//预计送达时间
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtShipOrderNumberTip;
        protected System.Web.UI.HtmlControls.HtmlGenericControl txtArriveTimeTip;
		private void BindExpressCompany(int modeId)
		{
			this.expressRadioButtonList.ExpressCompanies = SalesHelper.GetExpressCompanysByMode(modeId);
			this.expressRadioButtonList.DataBind();
		}
		private void BindOrderItems(OrderInfo order)
		{
			this.lblOrderId.Text = order.OrderId;
			this.lblOrderTime.Time = order.OrderDate;
			this.itemsList.Order = order;
		}
		private void BindShippingAddress(OrderInfo order)
		{
			string shippingRegion = string.Empty;
			if (!string.IsNullOrEmpty(order.ShippingRegion))
			{
				shippingRegion = order.ShippingRegion;
			}
			if (!string.IsNullOrEmpty(order.Address))
			{
				shippingRegion += order.Address;
			}
			if (!string.IsNullOrEmpty(order.ShipTo))
			{
				shippingRegion = shippingRegion + "  " + order.ShipTo;
			}
			if (!string.IsNullOrEmpty(order.ZipCode))
			{
				shippingRegion = shippingRegion + "  " + order.ZipCode;
			}
			if (!string.IsNullOrEmpty(order.TelPhone))
			{
				shippingRegion = shippingRegion + "  " + order.TelPhone;
			}
			if (!string.IsNullOrEmpty(order.CellPhone))
			{
				shippingRegion = shippingRegion + "  " + order.CellPhone;
			}
			this.litReceivingInfo.Text = shippingRegion;
		}
		private void btnSendGoods_Click(object sender, System.EventArgs e)
		{
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
			if (orderInfo != null)
			{
				ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
				if (currentManager != null)
				{
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
							if (!this.radioShippingMode.SelectedValue.HasValue)
							{
								this.ShowMsg("请选择配送方式", false);
							}
							else
                            {
                                //if (CustomConfigHelper.Instance.BrandShow == false)
                                //{
                                //if (string.IsNullOrEmpty(this.txtShipOrderNumber.Text.Trim()) || this.txtShipOrderNumber.Text.Trim().Length > 20)
                                //{
                                //    this.ShowMsg("运单号码不能为空，在1至20个字符之间", false);
                                //}
                                //}
                                //else
                                //{
                                    if (string.IsNullOrEmpty(this.expressRadioButtonList.SelectedValue))
                                    {
                                        this.ShowMsg("请选择物流公司", false);
                                    }
                                    else
                                    {
                                        ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(this.radioShippingMode.SelectedValue.Value, true);

                                        orderInfo.RealShippingModeId = this.radioShippingMode.SelectedValue.Value;
                                        orderInfo.RealModeName = shippingMode.Name;
                                        ExpressCompanyInfo info4 = ExpressHelper.FindNode(this.expressRadioButtonList.SelectedValue);
                                        if (info4 != null)
                                        {
                                            orderInfo.ExpressCompanyAbb = info4.Kuaidi100Code;
                                            orderInfo.ExpressCompanyName = info4.Name;
                                        }
                                        orderInfo.ShipOrderNumber = this.txtShipOrderNumber.Text;
                                        if (OrderHelper.SendGoods(orderInfo))
                                        {
                                            SendNoteInfo info5 = new SendNoteInfo();
                                            info5.NoteId = Globals.GetGenerateId();
                                            info5.OrderId = this.orderId;
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
                                                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
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
                                            if (CustomConfigHelper.Instance.IsSanzuo)//三作咖啡特殊需求:发货时,打印小票(配送票)
                                            {
                                                //this.ShowMsgAndReUrl("发货成功", true,"ManageOrders.aspx");
                                                CloseWindowAndRedirect();
                                            }
                                            else if(CustomConfigHelper.Instance.IsProLa)//pro辣特殊需求,发货时增加消息推送
                                            {
                                                WriteLog("进入");
                                                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                                                TemplateMessage templateMessage = new TemplateMessage();
                                                templateMessage.Url = Request.Url.Host +"/vshop/MemberOrders.aspx?status=3";//单击URL
                                                templateMessage.TemplateId = "pPCCurFLdpgnI0ZWpr5eFJQ5bxS7xboaEW2ScBoOY4U";//Globals.GetMasterSettings(true).WX_Template_01;// "b1_ARggaBzbc5owqmwrZ15QPj9Ksfs0p5i64C6MzXKw";//消息模板ID
                                                templateMessage.Touser = member.OpenId;//用户OPENID
                                                string productsDes = ""; int c=0;
                                                foreach (LineItemInfo info in orderInfo.LineItems.Values)
                                                {
                                                    if(c>3) break;
                                                    productsDes += info.ItemDescription + ",";
                                                        c++;
                                                }
                                                productsDes = productsDes.TrimEnd(',') + "等菜品";

                                                TemplateMessage.MessagePart[] messateParts = new TemplateMessage.MessagePart[]{
                                                new TemplateMessage.MessagePart{Name = "first",Value = "亲，您的菜品已备好，配送在途！"},
                                                new TemplateMessage.MessagePart{Name = "keyword1",Value = "订单"+orderInfo.OrderId+"开始配送"},
                                                new TemplateMessage.MessagePart{Name = "keyword2",Value =productsDes},
                                                new TemplateMessage.MessagePart{Name = "remark",Color = "#FF0000",Value = "预计到达时间："+txtArriveTime.Text/*orderInfo.ShipToDate*/}};
                                                                        templateMessage.Data = messateParts;
                                                TemplateApi.SendMessage(TokenApi.GetToken_Message(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret), templateMessage);
                                                this.ShowMsg("发货成功", true);
                                            }
                                            else
                                            {
                                                this.ShowMsg("发货成功", true);
                                            }

                                        }
                                        else
                                        {
                                            this.ShowMsg("发货失败", false);
                                        }
                                    }
                                //}
							}
						}
					}
				}
			}
		}


        private void WriteLog(string log)
        {
            System.IO.StreamWriter writer = System.IO.File.AppendText(System.Web.HttpContext.Current.Server.MapPath("~/error.txt"));
            writer.WriteLine(System.DateTime.Now);
            writer.WriteLine(log);
            writer.Flush();
            writer.Close();
        }
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.orderId = this.Page.Request.QueryString["OrderId"];

                //判断当前订单的状态,是否处理申请退款/退货中
                System.Data.DataTable dtReturns = RefundHelper.GetOrderReturnsBySwr(string.Format("orderId='{0}'", this.orderId));
                if (dtReturns.Rows.Count > 0 && (dtReturns.Rows[0]["HandleStatus"].ToInt() == 4 || dtReturns.Rows[0]["HandleStatus"].ToInt() == 6))
                {
                    btnSendGoods.Enabled = false;
                    this.ShowMsg(string.Format("当前订单已申请{0}", dtReturns.Rows[0]["HandleStatus"].ToInt() == 4 ? "退货" : "退款"), false);
                }

				OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
				this.BindOrderItems(orderInfo);
				this.btnSendGoods.Click += new System.EventHandler(this.btnSendGoods_Click);
				this.radioShippingMode.SelectedIndexChanged += new System.EventHandler(this.radioShippingMode_SelectedIndexChanged);
				if (!this.Page.IsPostBack)
				{
					if (orderInfo == null)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						this.radioShippingMode.DataBind();
						this.radioShippingMode.SelectedValue = new int?(orderInfo.ShippingModeId);
						this.BindExpressCompany(orderInfo.ShippingModeId);
						this.expressRadioButtonList.SelectedValue = orderInfo.ExpressCompanyAbb;
						this.BindShippingAddress(orderInfo);
						this.litShippingModeName.Text = orderInfo.ModeName;
						this.litShipToDate.Text = orderInfo.ShipToDate;
						this.litRemark.Text = orderInfo.Remark;
						this.txtShipOrderNumber.Text = orderInfo.ShipOrderNumber;
					}
                    checkRadio();
                    if (CustomConfigHelper.Instance.IsSanzuo)
                    {
                        radioShippingMode.SelectedIndex=0;
                        expressRadioButtonList.SelectedIndex = 0;
                        txtShipOrderNumber.Text = "店内配送";
                        btnSendGoods_Click(null,null);
                    }
                    if (CustomConfigHelper.Instance.IsProLa)
                    {
                        radioShippingMode.SelectedIndex = 0;
                        expressRadioButtonList.SelectedIndex = 0;
                        txtShipOrderNumber.Text = "店内配送";
                        this.specialHideShow.Value = "prola";
                    }
				}
                
			}
		}
		private void radioShippingMode_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            checkRadio();
		}


        private void checkRadio()
        {
            
                if (this.radioShippingMode.SelectedValue.HasValue)
                {
                    this.BindExpressCompany(this.radioShippingMode.SelectedValue.Value);
                    //新增判断:如果选择是上门自提,则不需要绑定数据,并且快递和快递单号无效
                    if (this.radioShippingMode.SelectedItem.Text.IndexOf("自提") > -1)
                    {
                        this.expressRadioButtonList.SelectedIndex = 0;
                        this.expressRadioButtonList.Enabled = false;
                        this.txtShipOrderNumber.Text = "上门自提";
                        this.txtShipOrderNumber.Enabled = false;
                    }
                    else
                    {
                        this.expressRadioButtonList.Enabled = true;
                        this.txtShipOrderNumber.Text = "";
                        this.BindExpressCompany(this.radioShippingMode.SelectedValue.Value);
                        this.expressRadioButtonList.SelectedIndex = 0;
                        this.txtShipOrderNumber.Enabled = true;
                    }
                }
            
        }

	}
}
