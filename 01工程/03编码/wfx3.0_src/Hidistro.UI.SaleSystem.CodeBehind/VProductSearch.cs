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
    [ParseChildren(true)]
    public class VProductSearch : VshopTemplatedWebControl
    {
        private int categoryId;
        private HiImage imgUrl;
        private string keyWord;
        private Literal litContent;
        private VshopTemplatedRepeater rptCategories;
        private VshopTemplatedRepeater rptProducts;


        protected override void AttachChildControls()
        {
            int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
            this.keyWord = this.Page.Request.QueryString["keyWord"];
            this.imgUrl = (HiImage)this.FindControl("imgUrl");
            this.litContent = (Literal)this.FindControl("litContent");
            this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
            this.rptCategories = (VshopTemplatedRepeater)this.FindControl("rptCategories");
            this.Page.Session["stylestatus"] = "4";
            switch (Hidistro.Core.SettingsManager.GetMasterSettings(true).VTheme.ToLower())
            {
                case "common":
                    DataTable dt = CategoryBrowser.GetCategoriesRange(ProductInfo.ProductRanage.NormalSelect);
                    this.rptCategories.DataSource = dt;
                    this.rptCategories.DataBind();
                    int total = 0;
                    if (categoryId == 0)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            int FirstCategoryID = Convert.ToInt32(dt.Rows[0]["categoryId"]);
                            this.rptProducts.DataSource = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null, 0,FirstCategoryID, this.keyWord, 1, 200, out total, "ShowSaleCounts", "desc");
                            this.rptProducts.DataBind();

                        }
                    }
                    else
                    {
                        this.rptProducts.DataSource = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null, 0,categoryId, this.keyWord, 1, 200, out total, "ShowSaleCounts", "desc");
                        this.rptProducts.DataBind();
                    }
                    break;
                default:
                    this.rptCategories.ItemDataBound += new RepeaterItemEventHandler(this.rptCategories_ItemDataBound);
                    if (this.Page.Request.QueryString["TypeId"] != null)
                    {
                        this.rptCategories.DataSource = CategoryBrowser.GetCategoriesByPruductType(100, Convert.ToInt32(this.Page.Request.QueryString["TypeId"]));
                        this.rptCategories.DataBind();
                    }
                    else
                    {
                        IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategoriesRange(this.categoryId, 0x3e8, DistributorsBrower.GetCurrStoreProductRange());
                        this.rptCategories.DataSource = maxSubCategories;
                        this.rptCategories.DataBind();
                    }

                    PageTitle.AddSiteNameTitle("分类搜索页");
                    break;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vProductSearch.html";
            }
            base.OnInit(e);
        }

        private void rptCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Literal literal = (Literal)e.Item.Controls[0].FindControl("litpromotion");
                if (!string.IsNullOrEmpty(literal.Text))
                {
                    literal.Text = "<img src='" + literal.Text + "'></img>";
                }
                else
                {
                    literal.Text = "<img src='/Storage/master/default.png'></img>";
                }
            }
        }
        //private void rptCategories_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    if (e.CommandName == "fl")
        //    {
        //        int CategoryId = Convert.ToInt32(e.CommandArgument);
        //        this.Page.Response.Redirect("/Vshop/ProductSearch.aspx?categoryId=" + CategoryId);
        //    }
        //}



    }
}

