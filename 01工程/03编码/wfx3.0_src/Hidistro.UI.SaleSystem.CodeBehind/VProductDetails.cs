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
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;

    [ParseChildren(true)]
    public class VProductDetails : VshopTemplatedWebControl
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
        private Literal litMemberGradeInfo;
        private Literal litShortDescription;
        private Literal litSoldCount;
        private Literal litStock;
        private int productId;
        private VshopTemplatedRepeater rptProductImages;
        private Common_SKUSelector skuSelector;
        private System.Web.UI.HtmlControls.HtmlInputHidden specialHideShow;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
            {
                base.GotoResourceNotFound("");
            }
            this.rptProductImages = (VshopTemplatedRepeater) this.FindControl("rptProductImages");
            this.litItemParams = (Literal) this.FindControl("litItemParams");
            this.litProdcutName = (Literal) this.FindControl("litProdcutName");
            this.litActivities = (Literal) this.FindControl("litActivities");
            this.litSalePrice = (Literal) this.FindControl("litSalePrice");
            this.litMemberGradeInfo = (Literal)this.FindControl("litMemberGradeInfo");
            this.litMarketPrice = (Literal) this.FindControl("litMarketPrice");
            this.litShortDescription = (Literal) this.FindControl("litShortDescription");
            this.litDescription = (Literal) this.FindControl("litDescription");
            this.litStock = (Literal) this.FindControl("litStock");
            this.skuSelector = (Common_SKUSelector) this.FindControl("skuSelector");
            this.linkDescription = (HyperLink) this.FindControl("linkDescription");
            this.expandAttr = (Common_ExpandAttributes) this.FindControl("ExpandAttributes");
            this.litSoldCount = (Literal) this.FindControl("litSoldCount");
            this.litConsultationsCount = (Literal) this.FindControl("litConsultationsCount");
            this.litReviewsCount = (Literal) this.FindControl("litReviewsCount");
            this.litHasCollected = (HtmlInputHidden) this.FindControl("litHasCollected");
            this.litCategoryId = (HtmlInputHidden) this.FindControl("litCategoryId");
            this.litproductid = (HtmlInputHidden) this.FindControl("litproductid");
            this.specialHideShow = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("specialHideShow");
            ProductInfo product = ProductBrowser.GetProduct(MemberProcessor.GetCurrentMember(), this.productId);
            this.litproductid.Value = this.productId.ToString();
            if (product == null)
            {
                base.GotoResourceNotFound("重复购买失败！该商品不存在或已下架");
            }
            if (!string.IsNullOrEmpty(product.MainCategoryPath))
            {
                DataTable allFull = ProductBrowser.GetAllFull(int.Parse(product.MainCategoryPath.Split(new char[] { '|' })[0].ToString()));
                this.litActivities.Text = "<div class=\"price clearfix\"><span class=\"title\">促销活动：</span><div class=\"all-action\">";
                if (allFull.Rows.Count > 0)
                {
                    for (int i = 0; i < allFull.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            object text = this.litActivities.Text;
                            this.litActivities.Text = string.Concat(new object[] { text, "<div class=\"action\"><span class=\"purchase\"><a href=\"/Vshop/ActivityDetail.aspx?ActivitiesId=", allFull.Rows[i]["ActivitiesId"], "&CategoryId=", allFull.Rows[i]["ActivitiesType"], "\">", allFull.Rows[i]["ActivitiesName"].ToString(), "满", decimal.Parse(allFull.Rows[i]["MeetMoney"].ToString()).ToString("0"), "减", decimal.Parse(allFull.Rows[i]["ReductionMoney"].ToString()).ToString("0"), "</a>&nbsp;&nbsp;</span></div>" });
                        }
                        else
                        {
                            object obj3 = this.litActivities.Text;
                            this.litActivities.Text = string.Concat(new object[] { obj3, "<div class=\"action actionnone\"><span class=\"purchase\"><a href=\"/Vshop/ActivityDetail.aspx?ActivitiesId=", allFull.Rows[i]["ActivitiesId"], "&CategoryId=", allFull.Rows[i]["ActivitiesType"], "\">", allFull.Rows[i]["ActivitiesName"].ToString(), "满", decimal.Parse(allFull.Rows[i]["MeetMoney"].ToString()).ToString("0"), "减", decimal.Parse(allFull.Rows[i]["ReductionMoney"].ToString()).ToString("0"), "</a>&nbsp;&nbsp;</span></div>" });
                        }
                    }
                    this.litActivities.Text = this.litActivities.Text + "</div><em>&nbsp;more</em></div>";
                }
                else
                {
                    this.litActivities.Text = "";
                }
            }
            //传递爽爽挝啡的特殊名到前端,前端用jquery进行相应的功能隐藏
            if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.AutoShipping)
            {
                specialHideShow.Value = "sswk";//爽爽挝啡
            }
            else if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.CouponRecharge)
            {
                if (product.ProductName=="会员充值")
                {
                    specialHideShow.Value = "JZ";//轿子洗衣
                }
                
            }

            if (!string.IsNullOrEmpty(this.litActivities.Text) && (product == null))
            {
                base.GotoResourceNotFound("此商品已不存在");
            }
            Uri u1 = HttpContext.Current.Request.UrlReferrer;
            if (product.SaleStatus != ProductSaleStatus.OnSale && (u1 == null || u1.ToString().ToLower().IndexOf("admin")==-1))
            {
                base.GotoResourceNotFound("此商品已下架");//20160505,增加处理，后台查看商品时允许显示
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
            //this.litSalePrice.Text = product.MinSalePrice.ToString("F2");
            string currentMemberGradeName = (MemberProcessor.GetCurrentMember() == null) ? "" :
                string.Format("<span style='font-size:12px; background:#F90; color:#FFF; border-radius:3px; padding:3px 5px; margin-right:5px;'>{0}</span>"
                , Hidistro.ControlPanel.Members.MemberHelper.GetMemberGrade(MemberProcessor.GetCurrentMember().GradeId).Name);
            
            this.litMemberGradeInfo.Text = currentMemberGradeName;
            //九信渐佳需求：如果是通过后台代理商点击商品链接跳转到此页面，显示成本价。
            //判断当前登陆的用户是不是代理商;
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            if (currentManager != null && ManagerHelper.GetRole(currentManager.RoleId).RoleName == "代理商")
            {
                //是代理商就显示成本价
                this.litSalePrice.Text = "¥" + product.MinCostPrice.ToString("F2");
            }
            else
            {
                //不是就显示原价
                this.litSalePrice.Text = "¥" + product.MinSalePrice.ToString("F2");
            }

            if (product.MarketPrice.HasValue)
            {
                this.litMarketPrice.SetWhenIsNotNull(product.MarketPrice.GetValueOrDefault(0M).ToString("F2"));
            }
            this.litShortDescription.Text = product.ShortDescription;
            if (this.litDescription != null)
            {
                this.litDescription.Text = product.Description;
            }
            this.litSoldCount.SetWhenIsNotNull(product.ShowSaleCounts.ToString());
            this.litStock.Text = product.Stock.ToString();
            this.skuSelector.ProductId = this.productId;
            if (this.expandAttr != null)
            {
                this.expandAttr.ProductId = this.productId;
            }
            if (this.linkDescription != null)
            {
                this.linkDescription.NavigateUrl = "/Vshop/ProductDescription.aspx?productId=" + this.productId;
            }
            this.litConsultationsCount.SetWhenIsNotNull(ProductBrowser.GetProductConsultationsCount(this.productId, false).ToString());
            this.litReviewsCount.SetWhenIsNotNull(ProductBrowser.GetProductReviewsCount(this.productId).ToString());
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            bool flag = false;
            if (currentMember != null)
            {
                flag = ProductBrowser.CheckHasCollect(currentMember.UserId, this.productId);
            }
            this.litHasCollected.SetWhenIsNotNull(flag ? "1" : "0");
            ProductBrowser.UpdateVisitCounts(this.productId);
            PageTitle.AddSiteNameTitle("商品详情");
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string str3 = "";
            if (!string.IsNullOrEmpty(masterSettings.GoodsPic))
            {
                str3 = Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.GoodsPic;
            }
            this.litItemParams.Text = string.Concat(new object[] { str3, "|", masterSettings.GoodsName, "|", masterSettings.GoodsDescription, "$", Globals.HostPath(HttpContext.Current.Request.Url), product.ImageUrl1, "|", this.litProdcutName.Text, "|", product.ShortDescription, "|", HttpContext.Current.Request.Url });
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VProductDetails.html";
            }
            base.OnInit(e);
        }
    }
}

