namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Hidistro.Entities.Commodities;
    using System.Data;
using Hidistro.ControlPanel.Config;

    [ParseChildren(true)]
    public class VProductList : VshopTemplatedWebControl
    {
        private int categoryId;
        private int BrandId;
        private HiImage imgUrl;
        private string keyWord;
        private Literal litContent;
        private VshopTemplatedRepeater rptCategories;
        private VshopTemplatedRepeater rptProducts;
        private HtmlInputHidden txtTotalPages;
        private VshopTemplatedRepeater rptBrandShow;
        protected override void AttachChildControls()
        {
            if (CustomConfigHelper.Instance.BrandShow)
            {
                #region   卡拉萌购特殊需求
                int num;
                int num2;
                int num3;
                int.TryParse(this.Page.Request.QueryString["BrandId"], out this.BrandId);
                this.keyWord = this.Page.Request.QueryString["keyWord"];
                if (!string.IsNullOrWhiteSpace(this.keyWord))
                {
                    this.keyWord = this.keyWord.Trim();
                }
                this.imgUrl = (HiImage)this.FindControl("imgUrl");
                this.litContent = (Literal)this.FindControl("litContent");
                this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
                this.rptBrandShow = (VshopTemplatedRepeater)this.FindControl("rptBrandShow");
                this.txtTotalPages = (HtmlInputHidden)this.FindControl("txtTotal");
                string str = this.Page.Request.QueryString["sort"];
                if (string.IsNullOrWhiteSpace(str))
                {
                    str = "DisplaySequence";
                }
                string str2 = this.Page.Request.QueryString["order"];
                if (string.IsNullOrWhiteSpace(str2))
                {
                    str2 = "desc";
                }
                if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
                {
                    num = 1;
                }
                if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
                {
                    num2 = 20;
                }
                IList<BrandCategoryInfo> maxSubCategories = CategoryBrowser.GetBrandCategory(this.BrandId, 0x3e8);
                this.rptBrandShow.DataSource = maxSubCategories;
                this.rptBrandShow.DataBind();
                string swr = "";
                DataTable dt = null;
                if (!string.IsNullOrWhiteSpace(this.Page.Request.QueryString["TypeId"]))
                {
                    int typeID = Convert.ToInt32(this.Page.Request.QueryString["TypeId"]);
                    if (typeID == 1)
                    {
                        swr = " CategoryId in (1,2,3,4)";
                        dt = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null,0,null, this.keyWord, num, num2, out num3, str, str2, swr);
                    }
                    else
                    {
                        swr = " CategoryId in (select CategoryId from Hishop_Categories where AssociatedProductType=" + typeID + ")";
                        dt = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null,0, null, this.keyWord, num, num2, out num3, str, str2, swr);
                    }
                }
                else
                {
                    dt = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null, this.BrandId,null, this.keyWord, num, num2, out num3, str, str2);
                }
                this.rptProducts.DataSource = dt;
                this.rptProducts.DataBind();
                this.txtTotalPages.SetWhenIsNotNull(num3.ToString());
                PageTitle.AddSiteNameTitle("分类搜索页");
                #endregion         
            }
            else 
            {
               #region 
                int num;
                int num2;
                int num3;
                int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
                this.keyWord = this.Page.Request.QueryString["keyWord"];
                if (!string.IsNullOrWhiteSpace(this.keyWord))
                {
                    this.keyWord = this.keyWord.Trim();
                }
                this.imgUrl = (HiImage)this.FindControl("imgUrl");
                this.litContent = (Literal)this.FindControl("litContent");
                this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
                this.rptCategories = (VshopTemplatedRepeater)this.FindControl("rptCategories");
                this.txtTotalPages = (HtmlInputHidden)this.FindControl("txtTotal");
                string str = this.Page.Request.QueryString["sort"];
                if (string.IsNullOrWhiteSpace(str))
                {
                    str = "DisplaySequence";
                }
                string str2 = this.Page.Request.QueryString["order"];
                if (string.IsNullOrWhiteSpace(str2))
                {
                    str2 = "desc";
                }
                if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
                {
                    num = 1;
                }
                if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
                {
                    num2 = 20;
                }
                IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategoriesRange(this.categoryId, 0x3e8);
                this.rptCategories.DataSource = maxSubCategories;
                this.rptCategories.DataBind();
                string swr = "";
                DataTable dt = null;
                if (!string.IsNullOrWhiteSpace(this.Page.Request.QueryString["TypeId"]))
                {
                    int typeID = Convert.ToInt32(this.Page.Request.QueryString["TypeId"]);
                    if (typeID == 1)
                    {
                        swr = " CategoryId in (1,2,3,4)";
                        dt = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null,0, null, this.keyWord, num, num2, out num3, str, str2, swr);
                    }
                    else
                    {
                        swr = " CategoryId in (select CategoryId from Hishop_Categories where AssociatedProductType=" + typeID + ")";
                        dt = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null,0, null, this.keyWord, num, num2, out num3, str, str2, swr);
                    }
                }
                else
                {
                    dt = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null,0, new int?(this.categoryId), this.keyWord, num, num2, out num3, str, str2);
                }
                this.rptProducts.DataSource = dt;
                this.rptProducts.DataBind();
                this.txtTotalPages.SetWhenIsNotNull(num3.ToString());
                PageTitle.AddSiteNameTitle("分类搜索页");
                #endregion         
            }
        
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VProductList.html";
            }
            base.OnInit(e);
        }
    }
}

