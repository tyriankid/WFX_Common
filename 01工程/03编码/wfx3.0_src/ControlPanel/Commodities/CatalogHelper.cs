namespace Hidistro.ControlPanel.Commodities
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.SqlDal.Commodities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Web.Caching;

    public sealed class CatalogHelper
    {
        private const string CategoriesCachekey = "DataCache-Categories";

        private CatalogHelper()
        {
        }

        public static bool AddBrandCategory(BrandCategoryInfo brandCategory)
        {
            int brandId = new BrandCategoryDao().AddBrandCategory(brandCategory);
            if (brandId <= 0)
            {
                return false;
            }
            if (brandCategory.ProductTypes.Count > 0)
            {
                new BrandCategoryDao().AddBrandProductTypes(brandId, brandCategory.ProductTypes);
            }
            return true;
        }

        public static CategoryActionStatus AddCategory(CategoryInfo category)
        {
            if (category == null)
            {
                return CategoryActionStatus.UnknowError;
            }
            Globals.EntityCoding(category, true);
            if (new CategoryDao().CreateCategory(category) > 0)
            {
                EventLogs.WriteOperationLog(Privilege.AddProductCategory, string.Format(CultureInfo.InvariantCulture, "创建了一个新的店铺分类:”{0}”", new object[] { category.Name }));
                HiCache.Remove("DataCache-Categories");
            }
            return CategoryActionStatus.Success;
        }

        public static bool AddProductTags(int productId, IList<int> tagsId, DbTransaction dbtran)
        {
            return new TagDao().AddProductTags(productId, tagsId, dbtran);
        }

        public static int AddTags(string tagName)
        {
            int num = 0;
            if (new TagDao().GetTags(tagName) <= 0)
            {
                num = new TagDao().AddTags(tagName);
            }
            return num;
        }

        public static bool BrandHvaeProducts(int brandId)
        {
            return new BrandCategoryDao().BrandHvaeProducts(brandId);
        }

        public static bool DeleteBrandCategory(int brandId)
        {
            return new BrandCategoryDao().DeleteBrandCategory(brandId);
        }

        public static bool DeleteCategory(int categoryId)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteProductCategory);
            bool flag = new CategoryDao().DeleteCategory(categoryId);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteProductCategory, string.Format(CultureInfo.InvariantCulture, "删除了编号为 “{0}” 的店铺分类", new object[] { categoryId }));
                HiCache.Remove("DataCache-Categories");
            }
            return flag;
        }

        public static bool DeleteProductTags(int productId, DbTransaction tran)
        {
            return new TagDao().DeleteProductTags(productId, tran);
        }

        public static bool DeleteTags(int tagId)
        {
            return new TagDao().DeleteTags(tagId);
        }

        public static int DisplaceCategory(int oldCategoryId, int newCategory)
        {
            return new CategoryDao().DisplaceCategory(oldCategoryId, newCategory);
        }

        public static DataTable GetBrandCategories()
        {
            return new BrandCategoryDao().GetBrandCategories();
        }

        public static DataTable GetBrandCategories(string brandName)
        {
            return new BrandCategoryDao().GetBrandCategories(brandName);
        }

        public static BrandCategoryInfo GetBrandCategory(int brandId)
        {
            return new BrandCategoryDao().GetBrandCategory(brandId);
        }

        private static DataTable GetCategories()
        {
            DataTable categories = HiCache.Get("DataCache-Categories") as DataTable;
            if (categories == null)
            {
                categories = new CategoryDao().GetCategories();
                HiCache.Insert("DataCache-Categories", categories, 360, CacheItemPriority.Normal);
            }
            return categories;
        }

        public static CategoryInfo GetCategory(int categoryId)
        {
            return new CategoryDao().GetCategory(categoryId);
        }

        public static string GetFullCategory(int categoryId)
        {
            CategoryInfo category = GetCategory(categoryId);
            if (category == null)
            {
                return null;
            }
            string name = category.Name;
            while ((category != null) && category.ParentCategoryId.HasValue)
            {
                category = GetCategory(category.ParentCategoryId.Value);
                if (category != null)
                {
                    name = category.Name + " &raquo; " + name;
                }
            }
            return name;
        }

        public static IList<CategoryInfo> GetMainCategories()
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            DataRow[] rowArray = GetCategories().Select("Depth = 1");
            for (int i = 0; i < rowArray.Length; i++)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(rowArray[i]));
            }
            return list;
        }

        public static IList<CategoryInfo> GetSequenceCategories()
        {
            IList<CategoryInfo> categories = new List<CategoryInfo>();
            foreach (CategoryInfo info in GetMainCategories())
            {
                categories.Add(info);
                LoadSubCategorys(info.CategoryId, categories);
            }
            return categories;
        }

        public static IList<CategoryInfo> GetSubCategories(int parentCategoryId)
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            string filterExpression = "ParentCategoryId = " + parentCategoryId.ToString(CultureInfo.InvariantCulture);
            DataRow[] rowArray = GetCategories().Select(filterExpression);
            for (int i = 0; i < rowArray.Length; i++)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(rowArray[i]));
            }
            return list;
        }

        public static string GetTagName(int tagId)
        {
            return new TagDao().GetTagName(tagId);
        }

        public static DataTable GetTags()
        {
            return new TagDao().GetTags();
        }

        public static bool IsExitProduct(string CategoryId)
        {
            return new CategoryDao().IsExitProduct(CategoryId);
        }

        private static void LoadSubCategorys(int parentCategoryId, IList<CategoryInfo> categories)
        {
            IList<CategoryInfo> subCategories = GetSubCategories(parentCategoryId);
            if ((subCategories != null) && (subCategories.Count > 0))
            {
                foreach (CategoryInfo info in subCategories)
                {
                    categories.Add(info);
                    LoadSubCategorys(info.CategoryId, categories);
                }
            }
        }

        public static bool SetBrandCategoryThemes(int brandid, string themeName)
        {
            bool flag = new BrandCategoryDao().SetBrandCategoryThemes(brandid, themeName);
            if (flag)
            {
                HiCache.Remove("DataCache-Categories");
            }
            return flag;
        }

        public static bool SetCategoryThemes(int categoryId, string themeName)
        {
            if (new CategoryDao().SetCategoryThemes(categoryId, themeName))
            {
                HiCache.Remove("DataCache-Categories");
            }
            return false;
        }

        public static bool SetProductExtendCategory(int productId, string extendCategoryPath)
        {
            return new CategoryDao().SetProductExtendCategory(productId, extendCategoryPath);
        }

        public static bool SwapCategorySequence(int categoryId, int displaysequence)
        {
            return new CategoryDao().SwapCategorySequence(categoryId, displaysequence);
        }

        public static void UpdateBrandCategorieDisplaySequence(int brandId, SortAction action)
        {
            new BrandCategoryDao().UpdateBrandCategoryDisplaySequence(brandId, action);
        }

        public static bool UpdateBrandCategory(BrandCategoryInfo brandCategory)
        {
            bool flag = new BrandCategoryDao().UpdateBrandCategory(brandCategory);
            if (flag && new BrandCategoryDao().DeleteBrandProductTypes(brandCategory.BrandId))
            {
                new BrandCategoryDao().AddBrandProductTypes(brandCategory.BrandId, brandCategory.ProductTypes);
            }
            return flag;
        }

        public static bool UpdateBrandCategoryDisplaySequence(int barndId, int displaysequence)
        {
            return new BrandCategoryDao().UpdateBrandCategoryDisplaySequence(barndId, displaysequence);
        }

        public static CategoryActionStatus UpdateCategory(CategoryInfo category)
        {
            if (category == null)
            {
                return CategoryActionStatus.UnknowError;
            }
            Globals.EntityCoding(category, true);
            CategoryActionStatus status = new CategoryDao().UpdateCategory(category);
            if (status == CategoryActionStatus.Success)
            {
                EventLogs.WriteOperationLog(Privilege.EditProductCategory, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的店铺分类", new object[] { category.CategoryId }));
                HiCache.Remove("DataCache-Categories");
            }
            return status;
        }

        public static bool UpdateTags(int tagId, string tagName)
        {
            bool flag = false;
            int tags = new TagDao().GetTags(tagName);
            if ((tags != tagId) && (tags > 0))
            {
                return flag;
            }
            return new TagDao().UpdateTags(tagId, tagName);
        }

        public static string UploadBrandCategorieImage(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
            {
                return string.Empty;
            }
            string str = Globals.GetStoragePath() + "/brand/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
            postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }

        /// <summary>
        /// 根据分类id获取特殊的5项返佣比例
        /// </summary>
        /// <param name="categoryId">分类id</param>
        /// <param name="specialName">特殊的代理商名字</param>
        /// <returns></returns>
        public static string GetSpecialCategoryRent(int categoryId, string specialName = "")
        {
            DataTable dtRents = new CategoryDao().GetSpecialCategoryRent(categoryId);//获取6个比例,顺序写死,一一对应代理商名字
            if (dtRents.Rows.Count > 0 && string.IsNullOrEmpty(dtRents.Rows[0]["Theme"].ToString()))
            {
                return "100";
            }
            //直接传回字符串
            if (specialName == "")
            {
                return dtRents.Rows[0]["Theme"].ToString();
            }
            else
            {
                string[] arryRents = dtRents.Rows[0]["Theme"].ToString().Split(',');
                
                decimal rate=100;
                switch (specialName)
                {
                    case "熳洁儿小区代"://第一个比例
                        rate = Convert.ToDecimal(arryRents[0]) / 100;//获取折扣比例
                        break;
                    case "熳洁儿二级客户/电商部门":
                        rate = Convert.ToDecimal(arryRents[1]) / 100;
                        break;
                    case "熳洁儿三级客户":
                        rate = Convert.ToDecimal(arryRents[2]) / 100;
                        break;
                    case "芬奈/U美单品牌客户":
                        rate = Convert.ToDecimal(arryRents[3]) / 100;
                        break;
                    case "非在册客户/公司内部员工":
                        rate = Convert.ToDecimal(arryRents[4]) / 100;
                        break;
                    case "PC端客户":
                        rate = Convert.ToDecimal(arryRents[5]) / 100;
                        break;
                }
                return rate.ToString();
            }
            
        }
        /// <summary>
        /// 更新某分类的特殊6项返佣比例
        /// </summary>
        /// <param name="categoryId">分类id</param>
        /// <param name="rents">六种分类,依序从1到6,写死</param>
        /// <returns></returns>
        public static bool UpdateSpecialCategoryRent(int categoryId, string rents)
        {
            return new CategoryDao().UpdateSpecialCategoryRent(categoryId, rents);
        }

        /// <summary>
        /// 判断当前分类是否定义了自定义的出货折扣
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static bool isSpecialRateExist(int categoryId)
        {
            DataTable dt = new CategoryDao().GetSpecialCategoryRent(categoryId);
            string strTheme=string.IsNullOrEmpty(dt.Rows[0]["Theme"].ToString()) ?"":dt.Rows[0]["Theme"].ToString();
            string[] arrayTheme = strTheme.Split(',');
            if (string.IsNullOrEmpty(strTheme) || int.Parse(arrayTheme[0]) == 0)//如果取不到特殊的折扣
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

