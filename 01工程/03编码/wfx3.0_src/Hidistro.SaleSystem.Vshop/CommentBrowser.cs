namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Store;
    using Hidistro.SqlDal.Comments;
    using Hidistro.SqlDal.Promotions;
    using Hidistro.SqlDal.Store;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Web.Caching;
    using System.Xml;

    public static class CommentBrowser
    {
       
       
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

        public static IList<ArticleInfo> GetArticleList(int categoryId, int maxNum)
        {
            return new ArticleDao().GetArticleList(categoryId, maxNum);
        }

        public static IList<ArticleCategoryInfo> GetArticleMainCategories()
        {
            return new ArticleCategoryDao().GetMainArticleCategories();
        }

      

        public static DataTable GetArticlProductList(int arctid)
        {
            Pagination page = new Pagination {
                PageIndex = 1,
                PageSize = 20
            };
            return (new ArticleDao().GetRelatedArticsProducts(page, arctid).Data as DataTable);
        }

       
        public static ArticleInfo GetFrontOrNextArticle(int articleId, string type, int categoryId)
        {
            return new ArticleDao().GetFrontOrNextArticle(articleId, type, categoryId);
        }

        public static VoteInfo GetVoteById(long voteId)
        {
            return new VoteDao().GetVoteById(voteId);
        }
       
        public static IList<VoteItemInfo> GetVoteItems(long voteId)
        {
            return new VoteDao().GetVoteItems(voteId);
        }

      
    }
}

