namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.HtmlControls;

    public class WgwWapArticle : VWeiXinOAuthTemplatedWebControl
    {
        private int categoryId;
        private string keyWord;
        private VshopTemplatedRepeater rptArticles;
        private HtmlInputHidden txtCategoryId;
        private HtmlInputHidden txtCategoryName;
        private HtmlInputHidden txtTotalPages;

        protected override void AttachChildControls()
        {
            int num;
            int num2;
            int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
            this.txtCategoryName = (HtmlInputHidden) this.FindControl("txtCategoryName");
            this.txtCategoryId = (HtmlInputHidden) this.FindControl("txtCategoryId");
            this.keyWord = this.Page.Request.QueryString["keyWord"];
            if (!string.IsNullOrWhiteSpace(this.keyWord))
            {
                this.keyWord = this.keyWord.Trim();
            }
            this.rptArticles = (VshopTemplatedRepeater)this.FindControl("rptArticles");
            this.txtTotalPages = (HtmlInputHidden) this.FindControl("txtTotal");
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 20;
            }
            ArticleQuery articleQuery = new ArticleQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CategoryId"]))
            {
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["CategoryId"], out result))
                {
                    ArticleCategoryInfo articleCategory = CommentBrowser.GetArticleCategory(result);
                    if (articleCategory != null)
                    {
                        PageTitle.AddSiteNameTitle(articleCategory.Name);
                        articleQuery.CategoryId = new int?(result);
                        this.txtCategoryId.Value = result.ToString();
                        this.txtCategoryName.Value = articleCategory.Name;
                    }
                    else
                    {
                        PageTitle.AddSiteNameTitle("文章分类搜索页");
                    }
                }
            }
            articleQuery.Keywords = this.keyWord;
            articleQuery.PageIndex = num;
            articleQuery.PageSize = num2;
            articleQuery.SortBy = "AddedDate";
            articleQuery.SortOrder = SortAction.Desc;
            DbQueryResult articleList = CommentBrowser.GetArticleList(articleQuery);
            this.rptArticles.DataSource = articleList.Data;
            this.rptArticles.DataBind();
            this.txtTotalPages.SetWhenIsNotNull(articleList.TotalRecords.ToString());
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-WgwArticles.html";
            }
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
    }
}

