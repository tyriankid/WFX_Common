namespace Hidistro.SqlDal.Commodities
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    public class CommonDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public int CommitDataTable(DataTable dtData, string selectSql)
        {
            using (SqlConnection connection = new SqlConnection(this.database.ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(selectSql, connection))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        int returnValue = 0;
                        try
                        {
                            SqlCommandBuilder builder = new SqlCommandBuilder(da);
                            returnValue = da.Update(dtData);
                        }
                        catch { returnValue = -1; }
                        return returnValue;
                    }
                }
            }
        }

        public DataTable GetDataTable(string tableName,string where,string order)
        {
            string selectSql = "Select * From " + tableName;
            if (!string.IsNullOrEmpty(where))
            {
                selectSql += " Where " + where;
            }
            if (!string.IsNullOrEmpty(order))
            {
                selectSql += " Order By " + order;
            }
            using (SqlConnection connection = new SqlConnection(this.database.ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(selectSql, connection))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dtData = new DataTable();
                        da.Fill(dtData);
                        return dtData;
                    }
                }
            }
        }


        public DataSet GetDataSet(string selectSql)
        {
            using (SqlConnection connection = new SqlConnection(this.database.ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(selectSql, connection))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataSet dsData = new DataSet();
                        da.Fill(dsData);
                        return dsData;
                    }
                }
            }
        }

        public int CommitDataSet(DataSet dsData, string[] arraySelectSql)
        {
            int execCount = arraySelectSql.Length;
            using (SqlConnection connection = new SqlConnection(this.database.ConnectionString))
            {
                SqlTransaction tran = null;
                try
                {
                    connection.Open();
                    tran = connection.BeginTransaction();
                    for (int i = 0; i < dsData.Tables.Count; i++)
                    {
                        SqlCommand cmd = new SqlCommand(arraySelectSql[i], connection, tran);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        SqlCommandBuilder builder = new SqlCommandBuilder(da);
                        da.Update(dsData.Tables[i]);
                        execCount--;
                    }
                    if (execCount == 0)
                        tran.Commit();
                }
                catch
                {
                    if (tran != null)
                    {
                        tran.Rollback();
                    }
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            return (execCount == 0) ? 1 : 0;
        }


       
    }
}

