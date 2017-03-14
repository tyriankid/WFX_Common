namespace Hidistro.ControlPanel.Comments
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.SqlDal.Comments;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;

    public static class ArticleHelper
    {
        public static bool AddReleatesProdcutByArticId(int articId, int productId)
        {
            return new ArticleDao().AddReleatesProdcutByArticId(articId, productId);
        }

        public static bool CreateArticle(ArticleInfo article)
        {
            if (null == article)
            {
                return false;
            }
            Globals.EntityCoding(article, true);
            return new ArticleDao().AddArticle(article);
        }

        public static bool CreateArticleCategory(ArticleCategoryInfo articleCategory)
        {
            if (null == articleCategory)
            {
                return false;
            }
            Globals.EntityCoding(articleCategory, true);
            return new ArticleCategoryDao().CreateUpdateDeleteArticleCategory(articleCategory, DataProviderAction.Create);
        }

       
        public static bool DeleteArticle(int articleId)
        {
            return new ArticleDao().DeleteArticle(articleId);
        }

        public static bool DeleteArticleCategory(int categoryId)
        {
            ArticleCategoryInfo articleCategory = new ArticleCategoryInfo {
                CategoryId = categoryId
            };
            return new ArticleCategoryDao().CreateUpdateDeleteArticleCategory(articleCategory, DataProviderAction.Delete);
        }

        public static int DeleteArticles(IList<int> articles)
        {
            if ((articles == null) || (articles.Count == 0))
            {
                return 0;
            }
            int num = 0;
            foreach (int num2 in articles)
            {
                new ArticleDao().DeleteArticle(num2);
                num++;
            }
            return num;
        }

       

        public static ArticleInfo GetArticle(int articleId)
        {
            return new ArticleDao().GetArticle(articleId);
        }

        public static ArticleCategoryInfo GetArticleCategory(int categoryId)
        {
            return new ArticleCategoryDao().GetArticleCategory(categoryId);
        }

        public static DbQueryResult GetArticleList(ArticleQuery articleQuery)
        {
            return new ArticleDao().GetArticleList(articleQuery);
        }

       
        public static IList<ArticleCategoryInfo> GetMainArticleCategories()
        {
            return new ArticleCategoryDao().GetMainArticleCategories();
        }

        public static DbQueryResult GetRelatedArticsProducts(Pagination page, int articId)
        {
            return new ArticleDao().GetRelatedArticsProducts(page, articId);
        }

        public static bool RemoveReleatesProductByArticId(int articId)
        {
            return new ArticleDao().RemoveReleatesProductByArticId(articId);
        }

        public static bool RemoveReleatesProductByArticId(int articId, int productId)
        {
            return new ArticleDao().RemoveReleatesProductByArticId(articId, productId);
        }

        public static void SwapArticleCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence)
        {
            new ArticleCategoryDao().SwapArticleCategorySequence(categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
        }

        public static bool UpdateArticle(ArticleInfo article)
        {
            if (null == article)
            {
                return false;
            }
            Globals.EntityCoding(article, true);
            return new ArticleDao().UpdateArticle(article);
        }

        public static bool UpdateArticleCategory(ArticleCategoryInfo articleCategory)
        {
            if (null == articleCategory)
            {
                return false;
            }
            Globals.EntityCoding(articleCategory, true);
            return new ArticleCategoryDao().CreateUpdateDeleteArticleCategory(articleCategory, DataProviderAction.Update);
        }

       

        public static bool UpdateRelease(int articId, bool release)
        {
            return new ArticleDao().UpdateRelease(articId, release);
        }

        public static string UploadArticleImage(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
            {
                return string.Empty;
            }
            string str = HiContext.Current.GetStoragePath() + "/article/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
            postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }

        public static string UploadHelpImage(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
            {
                return string.Empty;
            }
            string str = HiContext.Current.GetStoragePath() + "/help/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
            postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }
    }
}

