namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hishop.Plugins;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VFinishOrder : VWeiXinOAuthTemplatedWebControl
    {
        private HtmlAnchor btnToPay;
        private Literal litHelperText;
        private Literal litMessage;
        private Literal litOrderId;
        private Literal litOrderTotal;
        private HtmlInputHidden litPaymentType;
        private string orderId;
        private System.Web.UI.HtmlControls.HtmlInputHidden specialHideShow;

        protected override void AttachChildControls()
        {
            this.specialHideShow = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("specialHideShow");
            //传递爽爽挝啡的特殊名到前端,前端用jquery进行相应的功能隐藏
            if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.AutoShipping)
            {
                specialHideShow.Value = "sswk";//爽爽挝啡
            }
            if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.IsProLa)
            {
                specialHideShow.Value = "prola";//prola
            }
            this.orderId = this.Page.Request.QueryString["orderId"];
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
            if (orderInfo == null)
            {
                base.GotoResourceNotFound("");
            }
            if (!string.IsNullOrEmpty(orderInfo.Gateway) && (orderInfo.Gateway == "hishop.plugins.payment.offlinerequest"))
            {
                this.litMessage = (Literal) this.FindControl("litMessage");
                this.litMessage.SetWhenIsNotNull(SettingsManager.GetMasterSettings(false).OffLinePayContent);
            }
            this.btnToPay = (HtmlAnchor) this.FindControl("btnToPay");
            if (!string.IsNullOrEmpty(orderInfo.Gateway) && (orderInfo.Gateway == "hishop.plugins.payment.weixinrequest"))
            {
                this.btnToPay.Visible = true;
                this.btnToPay.HRef = "~/pay/wx_Submit.aspx?orderId=" + orderInfo.OrderId;
                specialHideShow.Value = "prolaQuickpay";//自动快速支付
            }
            if ((!string.IsNullOrEmpty(orderInfo.Gateway) && (orderInfo.Gateway != "hishop.plugins.payment.podrequest")) && ((orderInfo.Gateway != "hishop.plugins.payment.offlinerequest") && (orderInfo.Gateway != "hishop.plugins.payment.weixinrequest")))
            {
                PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(orderInfo.PaymentTypeId);
                string attach = "";
                string showUrl = string.Format("http://{0}/vshop/", HttpContext.Current.Request.Url.Host);
                PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单支付", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, showUrl, Globals.FullPath("/pay/PaymentReturn_url.aspx"), Globals.FullPath("/pay/PaymentNotify_url.aspx"), attach).SendRequest();
            }
            else
            {
                this.litOrderId = (Literal) this.FindControl("litOrderId");
                this.litOrderTotal = (Literal) this.FindControl("litOrderTotal");
                this.litPaymentType = (HtmlInputHidden) this.FindControl("litPaymentType");
                this.litPaymentType.SetWhenIsNotNull(orderInfo.PaymentTypeId.ToString());
                this.litOrderId.SetWhenIsNotNull(this.orderId);
                this.litOrderTotal.SetWhenIsNotNull(orderInfo.GetTotal().ToString("F2"));
                this.litHelperText = (Literal) this.FindControl("litHelperText");
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.litHelperText.SetWhenIsNotNull(masterSettings.OffLinePayContent);
                PageTitle.AddSiteNameTitle("支付");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VFinishOrder.html";
            }
            base.OnInit(e);
        }
    }
}

