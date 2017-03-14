namespace Hidistro.SqlDal.Comments
{
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Comments;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class ArticleCategoryDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool CreateUpdateDeleteArticleCategory(ArticleCategoryInfo articleCategory, DataProviderAction action)
        {
            if (null == articleCategory)
            {
                return false;
            }
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ArticleCategory_CreateUpdateDelete");
            this.database.AddInParameter(storedProcCommand, "Action", DbType.Int32, (int) action);
            this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            if (action != DataProviderAction.Create)
            {
                this.database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, articleCategory.CategoryId);
            }
            if (action != DataProviderAction.Delete)
            {
                this.database.AddInParameter(storedProcCommand, "Name", DbType.String, articleCategory.Name);
                this.database.AddInParameter(storedProcCommand, "IconUrl", DbType.String, articleCategory.IconUrl);
                this.database.AddInParameter(storedProcCommand, "Description", DbType.String, articleCategory.Description);
            }
            this.database.ExecuteNonQuery(storedProcCommand);
            return (((int) this.database.GetParameterValue(storedProcCommand, "Status")) == 0);
        }

        public ArticleCategoryInfo GetArticleCategory(int categoryId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ArticleCategories WHERE CategoryId = @CategoryId ORDER BY [DisplaySequence]");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    return DataMapper.PopulateArticleCategory(reader);
                }
                return null;
            }
        }

        public IList<ArticleCategoryInfo> GetMainArticleCategories()
        {
            IList<ArticleCategoryInfo> list = new List<ArticleCategoryInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * From Hishop_ArticleCategories ORDER BY [DisplaySequence]");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    ArticleCategoryInfo item = DataMapper.PopulateArticleCategory(reader);
                    list.Add(item);
                }
            }
            return list;
        }

        public void SwapArticleCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Hishop_ArticleCategories", "CategoryId", "DisplaySequence", categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
        }
    }
}

