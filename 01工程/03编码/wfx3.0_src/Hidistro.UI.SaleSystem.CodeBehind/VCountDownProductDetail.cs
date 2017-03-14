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

    [ParseChildren(true)]
    public class VCountDownProductDetail : VWeiXinOAuthTemplatedWebControl
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
        private Literal litProdcutName;
        private HtmlInputHidden litproductid;
        private Literal litReviewsCount;
        private Literal litSalePrice;
        private Literal litShortDescription;
        private HtmlInputControl litGroupbuyId;
        private Literal litSoldCount;
        private Literal litStock;
        private int countDownId;//抢购id
        private VshopTemplatedRepeater rptProductImages;
        private Common_SKUSelector skuSelector;
        private Literal litEndtime;//抢购结束时间

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["countDownId"], out this.countDownId))
            {
                base.GotoResourceNotFound("");
            }
            this.rptProductImages = (VshopTemplatedRepeater)this.FindControl("rptProductImages");
            this.litItemParams = (Literal)this.FindControl("litItemParams");
            this.litProdcutName = (Literal)this.FindControl("litProdcutName");
            this.litActivities = (Literal)this.FindControl("litActivities");
            this.litSalePrice = (Literal)this.FindControl("litSalePrice");
            this.litMarketPrice = (Literal)this.FindControl("litMarketPrice");
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
            this.litGroupbuyId = (HtmlInputControl)this.FindControl("litGroupbuyId");
            this.litEndtime = (Literal)this.FindControl("litEndtime");//抢购结束时间
            //ProductInfo product = ProductBrowser.GetProduct(MemberProcessor.GetCurrentMember(), this.productId);
            CountDownInfo countDownInfoByCountDownId = ProductBrowser.GetCountDownInfoByCountDownId(countDownId);
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            //ProductBrowseInfo info2 = ProductBrowser.GetProductBrowseInfo(countDownInfoByCountDownId.ProductId, null, null, masterSettings.StoreStockValidateType, masterSettings.OpenMultStore);
            ProductInfo product = ProductBrowser.GetProduct(MemberProcessor.GetCurrentMember(), countDownInfoByCountDownId.ProductId);
            this.litproductid.Value = this.countDownId.ToString();
            
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
            this.litSalePrice.Text = countDownInfoByCountDownId.CountDownPrice.ToString("F2");//现价是该商品的抢购价
            if (product.MarketPrice.HasValue)
            {
                this.litMarketPrice.SetWhenIsNotNull(product.MinSalePrice.ToString("F2"));//原价是该商品的一口价
            }
            this.litShortDescription.Text = product.ShortDescription;
            if (this.litDescription != null)
            {
                this.litDescription.Text = product.Description;
            }
            this.litSoldCount.SetWhenIsNotNull(product.ShowSaleCounts.ToString());
            this.litStock.Text = countDownInfoByCountDownId.MaxCount.ToString();//库存为限购数量
            this.skuSelector.ProductId = countDownInfoByCountDownId.ProductId;//为型号选择器绑定productid
            if (this.litEndtime != null)//倒计时
            {
                this.litEndtime.Text = Convert.ToDateTime(countDownInfoByCountDownId.EndDate).ToString();
            }
            this.skuSelector.ProductId = countDownInfoByCountDownId.ProductId;
            if (this.expandAttr != null)
            {
                this.expandAttr.ProductId = countDownInfoByCountDownId.ProductId;
            }
            if (this.linkDescription != null)
            {
                this.linkDescription.NavigateUrl = "/Vshop/ProductDescription.aspx?productId=" + countDownInfoByCountDownId.ProductId;
            }
            this.litConsultationsCount.SetWhenIsNotNull(ProductBrowser.GetProductConsultationsCount(countDownInfoByCountDownId.ProductId, false).ToString());
            this.litReviewsCount.SetWhenIsNotNull(ProductBrowser.GetProductReviewsCount(countDownInfoByCountDownId.ProductId).ToString());
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            bool flag = false;
            if (currentMember != null)
            {
                flag = ProductBrowser.CheckHasCollect(currentMember.UserId, countDownInfoByCountDownId.ProductId);
            }
            this.litHasCollected.SetWhenIsNotNull(flag ? "1" : "0");
            ProductBrowser.UpdateVisitCounts(countDownInfoByCountDownId.ProductId);
            PageTitle.AddSiteNameTitle("商品详情");
            string str3 = "";
            if (!string.IsNullOrEmpty(masterSettings.GoodsPic))
            {
                str3 = Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.GoodsPic;
            }
            string str4 = "";
            DistributorsInfo userIdDistributors = new DistributorsInfo();
            userIdDistributors = DistributorsBrower.GetUserIdDistributors(base.referralId);
            if ((userIdDistributors != null) && (userIdDistributors.UserId > 0))
            {
                PageTitle.AddSiteNameTitle(userIdDistributors.StoreName);
            }
            string str5 = (userIdDistributors == null) ? masterSettings.SiteName : userIdDistributors.StoreName;
            if (!string.IsNullOrEmpty(masterSettings.DistributorBackgroundPic))
            {
                str4 = Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.DistributorBackgroundPic.Split(new char[] { '|' })[0];
            }
            string strDes = masterSettings.ShopHomeDescription;
            if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.BrandShow)
            {
                strDes = "低价抢正品，马上有优惠，就在考拉萌购！";
            }

            this.litItemParams.Text = str3 + "|" + masterSettings.ShopHomeName + "|" + strDes + "$";
            this.litItemParams.Text = string.Concat(new object[] { this.litItemParams.Text, str4, "|好店推荐之", str5, "商城|" + strDes + "|", HttpContext.Current.Request.Url });
            this.litGroupbuyId.SetWhenIsNotNull(countDownInfoByCountDownId.CountDownId.ToString());
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VCountDownProductDetail.html";
            }
            base.OnInit(e);
        }
        /// <summary>
        /// 拆分时间,便于计算,用于抢购倒计时
        /// </summary>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static string FormatDate(DateTime endTime)
        {
            TimeSpan ts = endTime - DateTime.Now;
            return string.Format("<b>{0}</b>天<b>{1}</b>小时<b>{2}</b>分<b>{3}</b>秒", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
        }
        
    }
}

