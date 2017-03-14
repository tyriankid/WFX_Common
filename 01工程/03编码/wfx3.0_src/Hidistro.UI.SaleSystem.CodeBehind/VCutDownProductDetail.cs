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
    using Hidistro.ControlPanel.Promotions;

    public class VCutDownProductDetail : VshopTemplatedWebControl
    {
        private Common_ExpandAttributes expandAttr;
        private HyperLink linkDescription;
        private Literal litActivities;
        private HtmlInputHidden litCategoryId;
        private Literal litConsultationsCount;
        private Literal litDescription;
        private HtmlInputHidden litHasCollected;
        private Literal litItemParams;
        private Literal litCurrentPrice;
        private Literal litProdcutName;
        private HtmlInputHidden litproductid;
        private Literal litReviewsCount;
        private Literal litSalePrice;
        private Literal litShortDescription;
        private HtmlInputControl litCutDownId;
        private Literal litSoldCount;
        private Literal litStock;
        private int cutDownId;//抢购id
        private VshopTemplatedRepeater rptProductImages;
        private Common_SKUSelector skuSelector;
        private Literal litEndtime;//抢购结束时间

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["cutDownId"], out this.cutDownId))
            {
                base.GotoResourceNotFound("");
            }
            this.rptProductImages = (VshopTemplatedRepeater)this.FindControl("rptProductImages");
            this.litItemParams = (Literal)this.FindControl("litItemParams");
            this.litProdcutName = (Literal)this.FindControl("litProdcutName");
            this.litActivities = (Literal)this.FindControl("litActivities");
            this.litSalePrice = (Literal)this.FindControl("litSalePrice");//原价
            this.litCurrentPrice = (Literal)this.FindControl("litCurrentPrice");//当前价格
            this.litShortDescription = (Literal)this.FindControl("litShortDescription");
            this.litDescription = (Literal)this.FindControl("litDescription");
            this.litStock = (Literal)this.FindControl("litStock");
            this.skuSelector = (Common_SKUSelector)this.FindControl("skuSelector");
            this.linkDescription = (HyperLink)this.FindControl("linkDescription");
            this.expandAttr = (Common_ExpandAttributes)this.FindControl("ExpandAttributes");
            this.litSoldCount = (Literal)this.FindControl("litSoldCount");
            this.litConsultationsCount = (Literal)this.FindControl("litConsultationsCount");
            this.litReviewsCount = (Literal)this.FindControl("litReviewsCount");
            this.litHasCollected = (HtmlInputHidden)this.FindControl("litHasCollected");
            this.litCategoryId = (HtmlInputHidden)this.FindControl("litCategoryId");
            this.litproductid = (HtmlInputHidden)this.FindControl("litproductid");
            this.litCutDownId = (HtmlInputControl)this.FindControl("litCutDownId");
            this.litEndtime = (Literal)this.FindControl("litEndtime");//抢购结束时间
            //ProductInfo product = ProductBrowser.GetProduct(MemberProcessor.GetCurrentMember(), this.productId);
            CutDownInfo cutDownInfo = PromoteHelper.GetCutDown(this.cutDownId);
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            //ProductBrowseInfo info2 = ProductBrowser.GetProductBrowseInfo(countDownInfoByCountDownId.ProductId, null, null, masterSettings.StoreStockValidateType, masterSettings.OpenMultStore);
            ProductInfo product = ProductBrowser.GetProduct(MemberProcessor.GetCurrentMember(), cutDownInfo.ProductId);
            this.litproductid.Value = product.ProductId.ToString();

            if (!string.IsNullOrEmpty(this.litActivities.Text) && (product == null))
            {
                base.GotoResourceNotFound("此商品已不存在");
            }
            if (product.SaleStatus != ProductSaleStatus.OnSale)
            {
                base.GotoResourceNotFound("此商品已下架");
            }
            if (this.rptProductImages != null)
            {
                string locationUrl = "javascript:;";
                SlideImage[] imageArray = new SlideImage[] { new SlideImage(product.ImageUrl1, locationUrl), new SlideImage(product.ImageUrl2, locationUrl), new SlideImage(product.ImageUrl3, locationUrl), new SlideImage(product.ImageUrl4, locationUrl), new SlideImage(product.ImageUrl5, locationUrl) };
                this.rptProductImages.DataSource = from item in imageArray
                                                   where !string.IsNullOrWhiteSpace(item.ImageUrl)
                                                   select item;
                this.rptProductImages.DataBind();
            }
            string mainCategoryPath = product.MainCategoryPath;
            if (!string.IsNullOrEmpty(mainCategoryPath))
            {
                this.litCategoryId.Value = mainCategoryPath.Split(new char[] { '|' })[0];
            }
            else
            {
                this.litCategoryId.Value = "0";
            }
            this.litProdcutName.Text = product.ProductName;
            this.litSalePrice.Text = cutDownInfo.FirstPrice.ToString("F2");//原价是该活动的初始价
            if (product.MarketPrice.HasValue)
            {
                this.litCurrentPrice.SetWhenIsNotNull(cutDownInfo.CurrentPrice.ToString("F2"));//现价是该商品的被砍后的价格
            }
            this.litShortDescription.Text = product.ShortDescription;
            if (this.litDescription != null)
            {
                this.litDescription.Text = product.Description;
            }
            this.litSoldCount.SetWhenIsNotNull(product.ShowSaleCounts.ToString());
            this.litStock.Text = cutDownInfo.Count.ToString();//库存为限购数量
            this.skuSelector.ProductId = cutDownInfo.ProductId;//为型号选择器绑定productid
            if (this.litEndtime != null)//倒计时
            {
                this.litEndtime.Text = Convert.ToDateTime(cutDownInfo.EndDate).ToString();
            }
            this.skuSelector.ProductId = cutDownInfo.ProductId;
            if (this.expandAttr != null)
            {
                this.expandAttr.ProductId = cutDownInfo.ProductId;
            }
            if (this.linkDescription != null)
            {
                this.linkDescription.NavigateUrl = "/Vshop/ProductDescription.aspx?productId=" + cutDownInfo.ProductId;
            }
            this.litConsultationsCount.SetWhenIsNotNull(ProductBrowser.GetProductConsultationsCount(cutDownInfo.ProductId, false).ToString());
            this.litReviewsCount.SetWhenIsNotNull(ProductBrowser.GetProductReviewsCount(cutDownInfo.ProductId).ToString());
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            bool flag = false;
            if (currentMember != null)
            {
                flag = ProductBrowser.CheckHasCollect(currentMember.UserId, cutDownInfo.ProductId);
            }
            this.litHasCollected.SetWhenIsNotNull(flag ? "1" : "0");
            ProductBrowser.UpdateVisitCounts(cutDownInfo.ProductId);
            PageTitle.AddSiteNameTitle("商品详情");
            string str3 = "";
            if (!string.IsNullOrEmpty(masterSettings.GoodsPic))
            {
                str3 = Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.GoodsPic;
            }
            this.litItemParams.Text = string.Concat(new object[] { str3, "|", masterSettings.GoodsName, "|", masterSettings.GoodsDescription, "$", Globals.HostPath(HttpContext.Current.Request.Url), product.ImageUrl1, "|", this.litProdcutName.Text, "|", product.ShortDescription, "|", HttpContext.Current.Request.Url });
            this.litCutDownId.SetWhenIsNotNull(cutDownInfo.CutDownId.ToString());
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VCutDownProductDetail.html";
            }
            base.OnInit(e);
        }
    }
}

