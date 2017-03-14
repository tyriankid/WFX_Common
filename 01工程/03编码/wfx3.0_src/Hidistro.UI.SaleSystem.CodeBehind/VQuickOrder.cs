namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Hidistro.Entities.Commodities;
    using System.Data;
    using Hidistro.Entities.Members;
    using Hidistro.ControlPanel.Members;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Linq;
    public class VQuickOrder : VWeiXinOAuthTemplatedWebControl
    {
        private int categoryId;
        private VshopTemplatedRepeater rptCategories;
        //private VshopTemplatedRepeater rptProducts;
        private System.Web.UI.HtmlControls.HtmlInputHidden litCategoryId;
        private string keyWord;


        protected override void AttachChildControls()
        {
            int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
            this.keyWord = this.Page.Request.QueryString["keyWord"];
            //this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
            this.rptCategories = (VshopTemplatedRepeater)this.FindControl("rptCategories");
            this.litCategoryId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("litCategoryId");
            this.litCategoryId.SetWhenIsNotNull(this.categoryId.ToString());


            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                this.Page.Response.Redirect("UserLogin.aspx");
            }
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
            int ReferralUserId=0;
            if ((cookie != null) && !string.IsNullOrEmpty(cookie.Value))
            {
                ReferralUserId=Convert.ToInt32(cookie.Value);
            }
           
            Hidistro.Core.HiCache.Remove("DataCache-CategoriesRange");//清除分类缓存
            //获取手机端所有商品的分类
            DataTable dt = CategoryBrowser.GetCategoriesByRange(2);
            this.rptCategories.DataSource = dt;
            this.rptCategories.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vQuickOrder.html";
            }
            base.OnInit(e);
        }

        ///// <summary>
        ///// 加载商品repeater时触发
        ///// </summary>
        //private void rptProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
        //    {
        //        //加载商品的轮播图repeater
        //        VshopTemplatedRepeater rptProductImages = (VshopTemplatedRepeater)e.Item.Controls[0].FindControl("rptProductImages");
        //        if(rptProductImages!= null)
        //        {
        //            HtmlInputHidden hidProductId = (HtmlInputHidden)e.Item.Controls[0].FindControl("hidProductId");
        //            ProductInfo product = ProductBrowser.GetProduct(MemberProcessor.GetCurrentMember(), Convert.ToInt32(hidProductId.Value));
        //            string locationUrl = "javascript:;";
        //            SlideImage[] imageArray = new SlideImage[] { new SlideImage(product.ImageUrl1, locationUrl), new SlideImage(product.ImageUrl2, locationUrl), new SlideImage(product.ImageUrl3, locationUrl), new SlideImage(product.ImageUrl4, locationUrl), new SlideImage(product.ImageUrl5, locationUrl) };
        //            rptProductImages.DataSource = from item in imageArray
        //                                               where !string.IsNullOrWhiteSpace(item.ImageUrl)
        //                                               select item;
        //            rptProductImages.DataBind();
        //        }
        //    }
        //}



    }
}

