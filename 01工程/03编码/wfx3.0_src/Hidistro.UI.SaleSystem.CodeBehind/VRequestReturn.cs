namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VRequestReturn : VshopTemplatedWebControl
    {
        private HtmlInputHidden hidorderid;
        private HtmlInputHidden hidOrderStatus;
        private HtmlInputHidden hidproductid;
        private HtmlInputHidden hidskuid;
        private Literal litimage;
        private Literal litItemAdjustedPrice;
        private Literal litname;
        private Literal litQuantity;
        private string orderId;
        private string ProductId;
        private VshopTemplatedRepeater rptOrderProducts;

        protected override void AttachChildControls()
        {
            this.hidOrderStatus = (HtmlInputHidden) this.FindControl("OrderStatus");
            this.hidskuid = (HtmlInputHidden) this.FindControl("skuid");
            this.hidorderid = (HtmlInputHidden) this.FindControl("orderid");
            this.hidproductid = (HtmlInputHidden) this.FindControl("productid");
            this.orderId = this.Page.Request.QueryString["orderId"].Trim();
            this.ProductId = this.Page.Request.QueryString["ProductId"].Trim();
            this.hidorderid.Value = this.orderId;
            this.hidproductid.Value = this.ProductId;
            this.litimage = (Literal) this.FindControl("litimage");
            this.litname = (Literal) this.FindControl("litname");
            this.litItemAdjustedPrice = (Literal) this.FindControl("litItemAdjustedPrice");
            this.litQuantity = (Literal) this.FindControl("litQuantity");
            this.rptOrderProducts = (VshopTemplatedRepeater) this.FindControl("rptOrderProducts");
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
            this.hidOrderStatus.Value = ((int) orderInfo.OrderStatus).ToString();
            if (orderInfo == null)
            {
                base.GotoResourceNotFound("此订单已不存在");
            }
            bool flag = false;
            foreach (LineItemInfo info2 in orderInfo.LineItems.Values)
            {
                if (info2.ProductId.ToString() == this.ProductId)
                {
                    this.litimage.Text = "<image src=\"" + info2.ThumbnailsUrl + "\"></image>";
                    this.litname.Text = info2.ItemDescription;
                    this.litItemAdjustedPrice.Text = info2.ItemAdjustedPrice.ToString("0.00");
                    this.litQuantity.Text = info2.Quantity.ToString();
                    this.hidskuid.Value = info2.SkuId;
                    flag = true;
                }
            }
            if (!flag)
            {
                base.GotoResourceNotFound("此订单商品不存在");
            }
            PageTitle.AddSiteNameTitle("申请退货");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VRequestReturn.html";
            }
            base.OnInit(e);
        }
    }
}

