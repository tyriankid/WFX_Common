namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMemberOrderDetails : VWeiXinOAuthTemplatedWebControl
    {
        private HyperLink hlinkGetRedPager;
        private Literal litActualPrice;
        private Literal litAddress;
        private Literal litBuildPrice;
        private Literal litCounponPrice;
        private Literal litDisCountPrice;
        private Literal litExemption;
        private Literal litOrderDate;
        private Literal litOrderId;
        private OrderStatusLabel litOrderStatus;
        private Literal litPayTime;
        private Literal litPhone;
        private Literal litRedPagerAmount;
        private Literal litRemark;
        private Literal litShippingCost;
        private Literal litShipTo;
        private Literal litShipToDate;
        private Literal litTotalPrice;
        private string orderId;
        private HtmlInputHidden orderStatus;
        private VshopTemplatedRepeater rptOrderProducts;//商品列表
        private VshopTemplatedRepeater rptOrderGifts;//礼品列表
        private HtmlControl itemList;//商品区域
        private HtmlControl giftList;//礼品区域
        private HtmlControl costPointArea;//所需积分区域
        private Literal litPoint;//所需积分
        private HtmlControl DistributionInfoList;//配送信息区域
        private HtmlControl snCodeArea;//sn码的展示区域
        private Image litSnCode;//sn码的展示lit
        private HtmlInputHidden txtOrderId;

        protected override void AttachChildControls()
        {
            this.orderId = this.Page.Request.QueryString["orderId"];
            this.litShipTo = (Literal) this.FindControl("litShipTo");
            this.litPhone = (Literal) this.FindControl("litPhone");
            this.litAddress = (Literal) this.FindControl("litAddress");
            this.litOrderId = (Literal) this.FindControl("litOrderId");
            this.litOrderDate = (Literal) this.FindControl("litOrderDate");
            this.litOrderStatus = (OrderStatusLabel) this.FindControl("litOrderStatus");
            this.DistributionInfoList = (HtmlControl)this.FindControl("DistributionInfoList");//配送信息区域
            this.snCodeArea = (HtmlControl)this.FindControl("snCodeArea");//sn码展示区域
            this.litSnCode = (Image)this.FindControl("litSnCode");//sn码展示
            this.rptOrderProducts = (VshopTemplatedRepeater) this.FindControl("rptOrderProducts");//商品列表
            this.rptOrderGifts = (VshopTemplatedRepeater)this.FindControl("rptOrderGifts");//礼品列表
            this.itemList = (HtmlControl)this.FindControl("itemList");//商品列表区域
            this.giftList = (HtmlControl)this.FindControl("giftList");//礼品列表区域
            this.costPointArea = (HtmlControl)this.FindControl("costPoint");//所需积分区域
            this.litPoint = (Literal)this.FindControl("litPoint");//所需积分
            this.litTotalPrice = (Literal) this.FindControl("litTotalPrice");//所需金钱
            this.litPayTime = (Literal) this.FindControl("litPayTime");
            this.hlinkGetRedPager = (HyperLink) this.FindControl("hlinkGetRedPager");
            this.orderStatus = (HtmlInputHidden) this.FindControl("orderStatus");
            this.txtOrderId = (HtmlInputHidden) this.FindControl("txtOrderId");
            this.litRemark = (Literal) this.FindControl("litRemark");
            this.litShipToDate = (Literal) this.FindControl("litShipToDate");
            this.litShippingCost = (Literal) this.FindControl("litShippingCost");
            this.litCounponPrice = (Literal) this.FindControl("litCounponPrice");
            this.litRedPagerAmount = (Literal) this.FindControl("litRedPagerAmount");
            this.litExemption = (Literal) this.FindControl("litExemption");
            this.litBuildPrice = (Literal) this.FindControl("litBuildPrice");
            this.litDisCountPrice = (Literal) this.FindControl("litDisCountPrice");
            this.litActualPrice = (Literal) this.FindControl("litActualPrice");
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
            //礼品订单数据获取
            DataTable orderGiftInfo = GiftProcessor.GetOrderGiftsThumbnailsUrl(this.orderId);
            if (orderInfo == null && orderGiftInfo == null)
            {
                base.GotoResourceNotFound("此订单已不存在");
            }
            this.litShipTo.Text = orderInfo.ShipTo;
            this.litPhone.Text = orderInfo.CellPhone;
            this.litAddress.Text = orderInfo.ShippingRegion + orderInfo.Address;
            this.litOrderId.Text = this.orderId;
            this.litOrderDate.Text = orderInfo.OrderDate.ToString();
            this.litTotalPrice.SetWhenIsNotNull(orderInfo.GetAmount().ToString("F2"));
            this.litOrderStatus.OrderStatusCode = orderInfo.OrderStatus;
            this.litPayTime.SetWhenIsNotNull(orderInfo.PayDate.HasValue ? orderInfo.PayDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "");
            OrderRedPagerInfo orderRedPagerInfo = OrderRedPagerBrower.GetOrderRedPagerInfo(this.orderId);
            if ((orderRedPagerInfo != null) && (orderRedPagerInfo.MaxGetTimes > orderRedPagerInfo.AlreadyGetTimes))
            {
                this.hlinkGetRedPager.NavigateUrl = "/vshop/GetRedShare.aspx?orderid=" + this.orderId;
                this.hlinkGetRedPager.Visible = true;
            }
            this.orderStatus.SetWhenIsNotNull(((int) orderInfo.OrderStatus).ToString());
            this.txtOrderId.SetWhenIsNotNull(this.orderId.ToString());
            this.litCounponPrice.SetWhenIsNotNull(orderInfo.CouponValue.ToString("F2"));
            this.litRedPagerAmount.SetWhenIsNotNull(orderInfo.CouponValue.ToString("F2"));
            this.litShippingCost.SetWhenIsNotNull(orderInfo.AdjustedFreight.ToString("F2"));
            this.litShipToDate.SetWhenIsNotNull(orderInfo.ShipToDate);
            this.litBuildPrice.SetWhenIsNotNull(orderInfo.GetAmount().ToString("F2"));
            this.litDisCountPrice.SetWhenIsNotNull(orderInfo.GetAdjustCommssion().ToString("F2"));
            this.litActualPrice.SetWhenIsNotNull(orderInfo.TotalPrice.ToString("F2"));
            this.litRemark.SetWhenIsNotNull(orderInfo.Remark);
            this.litExemption.SetWhenIsNotNull(orderInfo.DiscountAmount.ToString("F2"));
            //绑定礼品列表数据源
            this.rptOrderGifts.DataSource = orderGiftInfo;
            this.rptOrderGifts.DataBind();
            this.rptOrderProducts.DataSource = orderInfo.LineItems.Values;
            this.rptOrderProducts.DataBind();
            PageTitle.AddSiteNameTitle("订单详情");
            //显示消耗掉的总积分
            int costPoint = 0;
            for (int i = 0; i < orderGiftInfo.Rows.Count; i++)
            {
                costPoint +=Convert.ToInt32(orderGiftInfo.Rows[i]["costPoint"]);
            }
            this.litPoint.Text = costPoint.ToString();

            //隐藏判断
            if (orderGiftInfo.Rows.Count == 0)
            {
                giftList.Visible = false;
                costPointArea.Visible = false;
            }
            if (orderInfo.LineItems.Count == 0)
            {
                itemList.Visible = false;
            }
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (masterSettings.EnableQuickPay && this.orderId.Length == 15)//如果打开了快速收银,那么隐藏物流配送板块,显示sn码板块并且载入sn码
            {
                DistributionInfoList.Visible = false;
                snCodeArea.Visible = true;
                this.litSnCode.ImageUrl = "Img.aspx?type=snCode&id=" + this.orderId; 
                this.litSnCode.DataBind();
            }
            else
            {
                DistributionInfoList.Visible = true;
                snCodeArea.Visible = false;
            }

        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberOrderDetails.html";
            }
            base.OnInit(e);
        }
    }
}

