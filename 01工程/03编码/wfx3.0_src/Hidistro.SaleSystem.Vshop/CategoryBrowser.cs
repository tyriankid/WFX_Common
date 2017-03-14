namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.SqlDal.Commodities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Web.Caching;

    public static class CategoryBrowser
    {
        private const string MainCategoriesCachekey = "DataCache-Categories";

        public static DataTable GetAllCategories()
        {
            return new CategoryDao().GetCategories();
        }

        public static DataTable GetAllCategoriesRange(ProductInfo.ProductRanage productRange = ProductInfo.ProductRanage.NormalSelect)
        {
            return new CategoryDao().GetCategoriesRange(productRange);
        }

        public static DataTable GetBrandsShow(int brandId)
        {
            return new BrandCategoryDao().GetBrandsShow(brandId);
        }
        public static DataTable GetBrandsShow(string brandIds)
        {
            return new BrandCategoryDao().GetBrandsShow(brandIds);
        }
        public static DataTable GetBrandCategories()
        {
            return new BrandCategoryDao().GetBrandCategories();
        }

        public static BrandCategoryInfo GetBrandCategory(int brandId)
        {
            return new BrandCategoryDao().GetBrandCategory(brandId);
        }

        public static DataTable GetCategories()
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

        public static IList<CategoryInfo> GetMaxMainCategories(int maxNum = 0x3e8)
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            DataRow[] rowArray = GetCategories().Select("Depth = 1");
            for (int i = 0; (i < maxNum) && (i < rowArray.Length); i++)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(rowArray[i]));
            }
            return list;
        }

        public static IList<CategoryInfo> GetMaxSubCategories(int parentCategoryId, int maxNum = 0x3e8)
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            DataRow[] rowArray = GetCategories().Select("ParentCategoryId = " + parentCategoryId);
            for (int i = 0; (i < maxNum) && (i < rowArray.Length); i++)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(rowArray[i]));
            }
            return list;
        }

        /// <summary>
        /// 根据限定范围获取商品分类
        /// </summary>
        public static IList<CategoryInfo> GetMaxSubCategoriesRange(int parentCategoryId, int maxNum = 0x3e8,ProductInfo.ProductRanage productRange= ProductInfo.ProductRanage.NormalSelect)
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            DataRow[] rowArray = GetCategoriesRange(productRange).Select("ParentCategoryId = " + parentCategoryId);
            for (int i = 0; (i < maxNum) && (i < rowArray.Length); i++)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(rowArray[i]));
            }
            return list;
        }
        //卡拉萌购
        public static IList<BrandCategoryInfo> GetBrandCategory(int BrandId, int maxNum = 0x3e8, ProductInfo.ProductRanage productRange = ProductInfo.ProductRanage.NormalSelect)
        {
            IList<BrandCategoryInfo> list = new List<BrandCategoryInfo>();
            DataRow[] rowArray = GetBrandRange(productRange).Select("BrandId= " + BrandId);
            for (int i = 0; (i < maxNum) && (i < rowArray.Length); i++)
            {
                list.Add(DataMapper.ConvertDataRowToBrandID(rowArray[i]));
            }
            return list;
        }
        public static DataTable GetBrandRange(ProductInfo.ProductRanage productRange = ProductInfo.ProductRanage.NormalSelect)
        {
            DataTable Brand = HiCache.Get("DataCache-CategoriesRange") as DataTable;
            if (Brand == null)
            {
                Brand = new CategoryDao().GetBrandRange(productRange);
                HiCache.Insert("DataCache-CategoriesRange", Brand, 360, CacheItemPriority.Normal);
            }
            return Brand;
        }




        public static DataTable GetCategoriesRange(ProductInfo.ProductRanage productRange = ProductInfo.ProductRanage.NormalSelect)
        {
            DataTable categories = HiCache.Get("DataCache-CategoriesRange") as DataTable;
            if (categories == null)
            {
                categories = new CategoryDao().GetCategoriesRange(productRange);
                HiCache.Insert("DataCache-CategoriesRange", categories, 360, CacheItemPriority.Normal);
            }
            return categories;
        }

        /// <summary>
        /// 根据ProductType查询分类
        /// </summary>
        public static DataTable GetCategoriesByPruductType(int Top,int ProductTypeID)
        {
            return new CategoryDao().GetCategoriesByPruductType(Top,ProductTypeID);
        }

        /// <summary>
        /// (爽爽挝啡)根据端id查找所有的分类,pc端1,移动端2
        /// </summary>
        public static DataTable GetCategoriesByRange(int rangeId)
        {
            return new CategoryDao().GetCategoriesByRange(rangeId);
        }

        public static int GetCategoryIdBySkuId(string skuId)
        {
            return new CategoryDao().GetCategoryIdBySkuId(skuId);
        }
    }
}

