namespace Hidistro.ControlPanel.Function
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

    public sealed class DataBaseHelper
    {
        private const string ConfigHelperCachekey = "DataCache-ConfigHelper";

        private DataBaseHelper()
        {
        }

        /// <summary>
        /// 批量更新表数据
        /// </summary>
        public static int CommitDataTable(DataTable dtData, string selectSql)
        {
            return new CommonDao().CommitDataTable(dtData, selectSql);
        }

        /// <summary>
        /// 批量更新数据集(带事务)
        /// </summary>
        public static int CommitDataSet(DataSet dsData, string[] selectSqls)
        {
            return new CommonDao().CommitDataSet(dsData, selectSqls);
        }

        /// <summary>
        /// 自定义条件查询表
        /// </summary>
        public static DataTable GetDataTable(string tableName, string where="", string order="")
        {
            return new CommonDao().GetDataTable(tableName, where, order);
        }

        /// <summary>
        /// 自定义条件查询数据集
        /// </summary>
        public static DataSet GetDataSet(string selectSql)
        {
            return new CommonDao().GetDataSet(selectSql);
        }

        /// <summary>
        /// 获取一个table的差异,用于批量更新表数据
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="includeDelete">是否包含删除操作</param>
        /// <returns></returns>
        public static DataTable GetDtDifferent(DataTable dt1, DataTable dt2, bool includeDelete = true)
        {
            int count = dt1.Rows.Count;//DB
            object[] key = new object[dt1.PrimaryKey.Length];
            object[] key2 = new object[dt2.PrimaryKey.Length];
            for (int i = 0; i < count; i++)
            {
                
                for (int j = 0; j < dt1.PrimaryKey.Length; j++)
                {
                    key[j] = dt1.Rows[i][dt1.PrimaryKey[j]];
                }

                DataRow drChange = dt2.Rows.Find(key);//User
                if (drChange == null)
                {
                    if (includeDelete)
                    {
                        //Delete:数据库的记录在用户输入中不存在
                        dt1.Rows[i].Delete();
                    }
                }
                else
                {
                    //update:数据库的记录在用户输入中存在
                    dt1.Rows[i].ItemArray = drChange.ItemArray;
                }
            }

            foreach (DataRow dr in dt2.Rows)
            {
                for (int j = 0; j < dt2.PrimaryKey.Length; j++)
                {
                    key2[j] = dr[dt2.PrimaryKey[j]];
                }
                DataRow drDbChange = dt1.Rows.Find(key2);
                if (drDbChange == null)
                {
                    //add:用户输入中在数据库的记录不存在
                    dt1.Rows.Add(dr.ItemArray);
                }
            }

            return dt1;
        }


        /// <summary>
        /// 得到ds的差异
        /// </summary>
        public static DataSet GetDsDifferent(DataSet dsSubmmit, DataSet dsDiff, bool includeDelete = true)
        {
            for (int i = 0; i < dsSubmmit.Tables.Count; i++)
			{
                GetDtDifferent(dsSubmmit.Tables[i], dsDiff.Tables[i], includeDelete);
			}

            return dsSubmmit;
        }
      
    }
}

