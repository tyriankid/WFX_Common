namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Linq;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;

    [ParseChildren(true)]
    public class VGiftDetails : VshopTemplatedWebControl
    {
        private Common_ExpandAttributes expandAttr;
        private HyperLink linkDescription;
        private Literal litActivities;
        private HtmlInputHidden litCategoryId;
        private Literal litConsultationsCount;
        private Literal litDescription;
        private HtmlInputHidden litHasCollected;
        private Literal litItemParams;
        private Literal litMarketPrice;
        private Literal litGiftName;//礼物名
        private HtmlInputHidden litgiftid;//礼物id
        private Literal litMyPoint;//我的积分
        private Literal litReviewsCount;
        private Literal litSalePrice;
        private Literal litShortDescription;
        private Literal litSoldCount;
        private Literal litStock;//库存
        private int GiftId;
        private VshopTemplatedRepeater rptGiftImages;//商品图轮播
        private Common_SKUSelector skuSelector;
        private Button buyButton;//购买按钮

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["GiftId"], out this.GiftId))
            {
                base.GotoResourceNotFound("");
            }
            this.rptGiftImages = (VshopTemplatedRepeater)this.FindControl("rptGiftImages");//商品展示播
            this.litItemParams = (Literal) this.FindControl("litItemParams");
            this.litGiftName = (Literal)this.FindControl("litGiftName");//礼物名
            this.litMyPoint = (Literal)this.FindControl("litMyPoint");//我的积分
            //this.litActivities = (Literal) this.FindControl("litActivities");
            this.litSalePrice = (Literal) this.FindControl("litSalePrice");
            this.litMarketPrice = (Literal) this.FindControl("litMarketPrice");
            this.litShortDescription = (Literal) this.FindControl("litShortDescription");
            this.litDescription = (Literal) this.FindControl("litDescription");
            this.litStock = (Literal) this.FindControl("litStock");//库存
            this.skuSelector = (Common_SKUSelector) this.FindControl("skuSelector");
            this.linkDescription = (HyperLink) this.FindControl("linkDescription");
            this.expandAttr = (Common_ExpandAttributes) this.FindControl("ExpandAttributes");
            this.litSoldCount = (Literal) this.FindControl("litSoldCount");
            this.litConsultationsCount = (Literal) this.FindControl("litConsultationsCount");
            this.litReviewsCount = (Literal) this.FindControl("litReviewsCount");
            this.litHasCollected = (HtmlInputHidden) this.FindControl("litHasCollected");
            this.litCategoryId = (HtmlInputHidden) this.FindControl("litCategoryId");
            this.litgiftid = (HtmlInputHidden) this.FindControl("litgiftid");
            this.buyButton = (Button)this.FindControl("buyButton");//购买按钮
            buyButton.Attributes.Add("OnClick", "return BuyProduct()");//在后台为按钮绑定前端单击事件
            this.buyButton.Click += new EventHandler(this.buyButton_Click);//给购买按钮绑定后台单击事件

            buyButton.Text = "立即兑换";
            //获取礼品信息
            GiftInfo gift = ProductBrowser.GetGiftDetails(GiftId);
            this.litgiftid.Value = this.GiftId.ToString();//将礼品id放到隐藏控件内
            //获取礼品展示图
            if (this.rptGiftImages != null)
            {
                string locationUrl = "javascript:;";
                SlideImage[] imageArray = new SlideImage[] { new SlideImage(gift.ImageUrl, locationUrl)};
                this.rptGiftImages.DataSource = from item in imageArray
                    where !string.IsNullOrWhiteSpace(item.ImageUrl)
                    select item;
                this.rptGiftImages.DataBind();
            }
            //获取礼品名
            this.litGiftName.Text = gift.Name;
            this.litSalePrice.Text = gift.NeedPoint.ToString();
            //获取库存
            this.litStock.Text = gift.Stock.ToString();
            //获取礼品市场价
            if (gift.MarketPrice.HasValue)
            {
                this.litMarketPrice.SetWhenIsNotNull(gift.MarketPrice.GetValueOrDefault(0M).ToString("F2"));
            }
            //获取礼品描述
            this.litShortDescription.Text = gift.ShortDescription;
            if (this.litDescription != null)
            {
                this.litDescription.Text = gift.LongDescription;
            }
            //获取我的积分
            if (this.litMyPoint != null)
            {
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                this.litMyPoint.Text = currentMember.Points.ToString();
            }
            //页面标题
            PageTitle.AddSiteNameTitle("礼品详情");
   }

        /// <summary>
        /// 单击兑换按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buyButton_Click(object sender, EventArgs e)
        {
            //传递到购物车
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            if (((shoppingCart != null) && (shoppingCart.LineGifts != null)) && (shoppingCart.LineGifts.Count > 0))
            {
                foreach (ShoppingCartGiftInfo info2 in shoppingCart.LineGifts)
                {
                    if (info2.GiftId == this.GiftId)
                    {
                        this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=购物车中已存在该礼品，请删除购物车中已有的礼品或者下次兑换！");
                        return;
                    }
                }
            }
            //获取数量
            int buyNum = Convert.ToInt32(System.Web.HttpContext.Current.Request.Form.Get("buyNum"));
            if (ShoppingCartProcessor.AddGiftItem(this.GiftId,buyNum))
            {
                this.Page.Response.Redirect("/Vshop/ShoppingCart.aspx");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VGiftDetails.html";
            }
            base.OnInit(e);
        }
    }
}

