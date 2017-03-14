namespace Hidistro.SqlDal.Commodities
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;

    public class CategoryDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public int CreateCategory(CategoryInfo category)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Category_Create");
            this.database.AddOutParameter(storedProcCommand, "CategoryId", DbType.Int32, 4);
            this.database.AddInParameter(storedProcCommand, "Name", DbType.String, category.Name);
            this.database.AddInParameter(storedProcCommand, "SKUPrefix", DbType.String, category.SKUPrefix);
            this.database.AddInParameter(storedProcCommand, "DisplaySequence", DbType.Int32, category.DisplaySequence);
            if (!string.IsNullOrEmpty(category.IconUrl))
            {
                this.database.AddInParameter(storedProcCommand, "IconUrl", DbType.String, category.IconUrl);
            }
            if (!string.IsNullOrEmpty(category.MetaTitle))
            {
                this.database.AddInParameter(storedProcCommand, "Meta_Title", DbType.String, category.MetaTitle);
            }
            if (!string.IsNullOrEmpty(category.MetaDescription))
            {
                this.database.AddInParameter(storedProcCommand, "Meta_Description", DbType.String, category.MetaDescription);
            }
            if (!string.IsNullOrEmpty(category.MetaKeywords))
            {
                this.database.AddInParameter(storedProcCommand, "Meta_Keywords", DbType.String, category.MetaKeywords);
            }
            if (!string.IsNullOrEmpty(category.Notes1))
            {
                this.database.AddInParameter(storedProcCommand, "Notes1", DbType.String, category.Notes1);
            }
            if (!string.IsNullOrEmpty(category.Notes2))
            {
                this.database.AddInParameter(storedProcCommand, "Notes2", DbType.String, category.Notes2);
            }
            if (!string.IsNullOrEmpty(category.Notes3))
            {
                this.database.AddInParameter(storedProcCommand, "Notes3", DbType.String, category.Notes3);
            }
            if (!string.IsNullOrEmpty(category.Notes4))
            {
                this.database.AddInParameter(storedProcCommand, "Notes4", DbType.String, category.Notes4);
            }
            if (!string.IsNullOrEmpty(category.Notes5))
            {
                this.database.AddInParameter(storedProcCommand, "Notes5", DbType.String, category.Notes5);
            }
            if (category.ParentCategoryId.HasValue)
            {
                this.database.AddInParameter(storedProcCommand, "ParentCategoryId", DbType.Int32, category.ParentCategoryId.Value);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "ParentCategoryId", DbType.Int32, 0);
            }
            if (category.AssociatedProductType.HasValue)
            {
                this.database.AddInParameter(storedProcCommand, "AssociatedProductType", DbType.Int32, category.AssociatedProductType.Value);
            }
            if (!string.IsNullOrEmpty(category.RewriteName))
            {
                this.database.AddInParameter(storedProcCommand, "RewriteName", DbType.String, category.RewriteName);
            }
            this.database.AddInParameter(storedProcCommand, "FirstCommission", DbType.String, category.FirstCommission);
            this.database.AddInParameter(storedProcCommand, "SecondCommission", DbType.String, category.SecondCommission);
            this.database.AddInParameter(storedProcCommand, "ThirdCommission", DbType.String, category.ThirdCommission);
            this.database.ExecuteNonQuery(storedProcCommand);
            return (int) this.database.GetParameterValue(storedProcCommand, "CategoryId");
        }

        public bool DeleteCategory(int categoryId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Category_Delete");
            this.database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, categoryId);
            return (this.database.ExecuteNonQuery(storedProcCommand) > 0);
        }

        public int DisplaceCategory(int oldCategoryId, int newCategory)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Products SET CategoryId=@newCategory, MainCategoryPath=(SELECT Path FROM Hishop_Categories WHERE CategoryId=@newCategory)+'|' WHERE CategoryId=@oldCategoryId");
            this.database.AddInParameter(sqlStringCommand, "oldCategoryId", DbType.Int32, oldCategoryId);
            this.database.AddInParameter(sqlStringCommand, "newCategory", DbType.Int32, newCategory);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public DataTable GetCategories()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CategoryId,Name,IconUrl,DisplaySequence,ParentCategoryId,Depth,[Path],RewriteName,HasChildren,FirstCommission,SecondCommission,ThirdCommission,AssociatedProductType FROM Hishop_Categories ORDER BY DisplaySequence");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 根据限定范围获取当前店铺的商品分类
        /// </summary>
        public DataTable GetCategoriesRange(ProductInfo.ProductRanage productRanage)
        {   
            string selectSql = "SELECT CategoryId,Name,IconUrl,DisplaySequence,ParentCategoryId,Depth,[Path],RewriteName,HasChildren,FirstCommission,SecondCommission,ThirdCommission FROM Hishop_Categories";
            selectSql += " Where 1=1 ";
            switch (productRanage)
            { 
                case ProductInfo.ProductRanage.RangeSelect:
                    //代理取系统设置的上架范围，分销商取所属代理商的上架范围
                    int ofAgentID = Globals.GetCurrentDistributorId();
                    DataTable dtDistributors = this.database.ExecuteDataSet(CommandType.Text,
                        string.Format("Select UserId,IsAgent,AgentPath From aspnet_Distributors Where UserId={0}", ofAgentID)).Tables[0];
                    if (dtDistributors.Rows[0]["IsAgent"].ToString() != "1" && dtDistributors.Rows[0]["AgentPath"] != DBNull.Value)
                        ofAgentID = int.Parse(dtDistributors.Rows[0]["AgentPath"].ToString().Split('|')[dtDistributors.Rows[0]["AgentPath"].ToString().Split('|').Length - 1]);
                    DataTable dtProductRange = this.database.ExecuteDataSet(CommandType.Text,
                        string.Format("Select * From Hishop_DistributorProductRange Where UserId={0}", ofAgentID)).Tables[0];
                    if (dtProductRange.Rows.Count == 0)
                    {
                        selectSql+=" And 1=2 ";//未设置上架范围
                    }
                    else
                    {
                        DataRow drProductRange = dtProductRange.Rows[0];
                        if (drProductRange["ProductRange1"] != DBNull.Value)
                            selectSql += string.Format(" And CategoryId in({0})", drProductRange["ProductRange1"].ToString());
                        else
                            selectSql += " And CategoryId in(null)";
                    }
                    break;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(selectSql + " ORDER BY DisplaySequence");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable GetBrandRange(ProductInfo.ProductRanage productRanage)
        {
            string selectSql = "SELECT BrandId,BrandName,Logo,CompanyUrl,RewriteName,MetaKeywords,MetaDescription,Description,DisplaySequence,Theme FROM Hishop_BrandCategories";
            selectSql += " Where 1=1 ";
            switch (productRanage)
            {
                case ProductInfo.ProductRanage.RangeSelect:
                    //代理取系统设置的上架范围，分销商取所属代理商的上架范围
                    int ofAgentID = Globals.GetCurrentDistributorId();
                    DataTable dtDistributors = this.database.ExecuteDataSet(CommandType.Text,
                        string.Format("Select UserId,IsAgent,AgentPath From aspnet_Distributors Where UserId={0}", ofAgentID)).Tables[0];
                    if (dtDistributors.Rows[0]["IsAgent"].ToString() != "1" && dtDistributors.Rows[0]["AgentPath"] != DBNull.Value)
                        ofAgentID = int.Parse(dtDistributors.Rows[0]["AgentPath"].ToString().Split('|')[dtDistributors.Rows[0]["AgentPath"].ToString().Split('|').Length - 1]);
                    DataTable dtProductRange = this.database.ExecuteDataSet(CommandType.Text,
                        string.Format("Select * From Hishop_DistributorProductRange Where UserId={0}", ofAgentID)).Tables[0];
                    if (dtProductRange.Rows.Count == 0)
                    {
                        selectSql += " And 1=2 ";//未设置上架范围
                    }
                    else
                    {
                        DataRow drProductRange = dtProductRange.Rows[0];
                        if (drProductRange["ProductRange1"] != DBNull.Value)
                            selectSql += string.Format(" And CategoryId in({0})", drProductRange["ProductRange1"].ToString());
                        else
                            selectSql += " And CategoryId in(null)";
                    }
                    break;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(selectSql + " ORDER BY DisplaySequence");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }





        public CategoryInfo GetCategory(int categoryId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Categories WHERE CategoryId =@CategoryId");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<CategoryInfo>(reader);
            }
        }

        public bool IsExitProduct(string CategoryId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Count(ProductId) FROM Hishop_Products WHERE CategoryId = @CategoryId");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.String, CategoryId);
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public bool SetCategoryThemes(int categoryId, string themeName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Categories SET Theme = @Theme WHERE CategoryId = @CategoryId");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            this.database.AddInParameter(sqlStringCommand, "Theme", DbType.String, themeName);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public bool SetProductExtendCategory(int productId, string extendCategoryPath)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Products SET ExtendCategoryPath = @ExtendCategoryPath WHERE ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "ExtendCategoryPath", DbType.String, extendCategoryPath);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public bool SwapCategorySequence(int categoryId, int displaysequence)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_Categories  set DisplaySequence=@DisplaySequence where CategoryId=@CategoryId");
            this.database.AddInParameter(sqlStringCommand, "@DisplaySequence", DbType.Int32, displaysequence);
            this.database.AddInParameter(sqlStringCommand, "@CategoryId", DbType.Int32, categoryId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public CategoryActionStatus UpdateCategory(CategoryInfo category)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Categories SET [Name] = @Name, SKUPrefix = @SKUPrefix,AssociatedProductType = @AssociatedProductType, Meta_Title=@Meta_Title,Meta_Description = @Meta_Description, IconUrl = @IconUrl,Meta_Keywords = @Meta_Keywords, Notes1 = @Notes1, Notes2 = @Notes2, Notes3 = @Notes3,  Notes4 = @Notes4, Notes5 = @Notes5, RewriteName = @RewriteName,FirstCommission=@FirstCommission,SecondCommission=@SecondCommission,ThirdCommission=@ThirdCommission WHERE CategoryId = @CategoryId;UPDATE Hishop_Categories SET RewriteName = @RewriteName WHERE ParentCategoryId = @CategoryId");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, category.CategoryId);
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, category.Name);
            this.database.AddInParameter(sqlStringCommand, "SKUPrefix", DbType.String, category.SKUPrefix);
            this.database.AddInParameter(sqlStringCommand, "AssociatedProductType", DbType.Int32, category.AssociatedProductType);
            this.database.AddInParameter(sqlStringCommand, "Meta_Title", DbType.String, category.MetaTitle);
            this.database.AddInParameter(sqlStringCommand, "Meta_Description", DbType.String, category.MetaDescription);
            this.database.AddInParameter(sqlStringCommand, "IconUrl", DbType.String, category.IconUrl);
            this.database.AddInParameter(sqlStringCommand, "Meta_Keywords", DbType.String, category.MetaKeywords);
            this.database.AddInParameter(sqlStringCommand, "Notes1", DbType.String, category.Notes1);
            this.database.AddInParameter(sqlStringCommand, "Notes2", DbType.String, category.Notes2);
            this.database.AddInParameter(sqlStringCommand, "Notes3", DbType.String, category.Notes3);
            this.database.AddInParameter(sqlStringCommand, "Notes4", DbType.String, category.Notes4);
            this.database.AddInParameter(sqlStringCommand, "Notes5", DbType.String, category.Notes5);
            this.database.AddInParameter(sqlStringCommand, "RewriteName", DbType.String, category.RewriteName);
            this.database.AddInParameter(sqlStringCommand, "FirstCommission", DbType.String, category.FirstCommission);
            this.database.AddInParameter(sqlStringCommand, "SecondCommission", DbType.String, category.SecondCommission);
            this.database.AddInParameter(sqlStringCommand, "ThirdCommission", DbType.String, category.ThirdCommission);
            return ((this.database.ExecuteNonQuery(sqlStringCommand) >= 1) ? CategoryActionStatus.Success : CategoryActionStatus.UnknowError);
        }

        public DataTable GetCategoriesByPruductType(int Top,int ProductTypeID)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select top "+Top+" * from Hishop_Categories where AssociatedProductType=@AssociatedProductType order by DisplaySequence asc");
            this.database.AddInParameter(sqlStringCommand, "AssociatedProductType",DbType.Int32, ProductTypeID);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 根据分类id获取特殊的5项返佣比例,取出的是Hishop_Categories未使用的字段theme
        /// </summary>
        /// <param name="categoryId">商品类别id</param>
        /// <returns></returns>
        public DataTable GetSpecialCategoryRent(int categoryId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select Theme from Hishop_Categories where CategoryId = "+categoryId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }
        /// <summary>
        /// 更新某分类的特殊5项返佣比例,使用的是Hishop_Categories未使用的字段theme
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="rents"></param>
        /// <returns></returns>
        public bool UpdateSpecialCategoryRent(int categoryId, string rents)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("update Hishop_Categories set Theme= '{0}' where CategoryId = {1}",rents,categoryId));
            return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
        }
        /// <summary>
        /// 通过显示端id获取第一个分类id
        /// </summary>
        public int GetFirstCategoryId(int rangeType)
        {
            DataTable dt=new DataTable ();
            DataTable dtAllCategories = GetCategories();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select HC.CategoryId from Hishop_Categories HC left join Hishop_Products HP on hc.CategoryId=hp.CategoryId where hp.[range]={0}", rangeType));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                dt = DataHelper.ConverDataReaderToDataTable(reader);
                if (dt.Rows.Count <= 0)
                {
                    return Convert.ToInt32(dtAllCategories.Rows[0]["CategoryId"]);
                }
                return Convert.ToInt32(dt.Rows[0]["CategoryId"]);
            }
        }
        /// <summary>
        /// (爽爽挝啡)根据端id查找所有的分类,pc端1,移动端2
        /// </summary>
        public DataTable GetCategoriesByRange(int rangeId)
        {
            DbCommand sqlStringCommand ;
            if(rangeId == 98 || rangeId==99)//机场店专用
                sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select * from Hishop_Categories where CategoryId in (select distinct hc.CategoryId from Hishop_Products HP right join Hishop_Categories HC on hp.CategoryId=hc.CategoryId where hp.[Range] = {0} and hp.SaleStatus=1)  order by DisplaySequence", rangeId));
            else
                sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select * from Hishop_Categories where CategoryId in (select distinct hc.CategoryId from Hishop_Products HP right join Hishop_Categories HC on hp.CategoryId=hc.CategoryId where hp.[Range] in ({0},0) and hp.SaleStatus=1)  order by DisplaySequence", rangeId));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }
        /// <summary>
        /// 通过skuid获取该商品分类id
        /// </summary>
        public int GetCategoryIdBySkuId(string skuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select hp.CategoryId from Hishop_SKUs HS left join Hishop_Products HP on hs.ProductId=hp.ProductId where hs.SkuId = '{0}'", skuId));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return Convert.ToInt32(((DataTable)DataHelper.ConverDataReaderToDataTable(reader)).Rows[0]["CategoryId"]);
            }
        }

    }
}

