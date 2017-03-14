namespace Hidistro.SqlDal.Commodities
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.SqlDal.Members;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class RegionDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        /// <summary>
        /// 根据条件得到当前区域表信息
        /// </summary>
        /// <param name="where">条件，为空则查询所有信息</param>
        /// <returns></returns>
        public DataTable GetRegion(string where)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.IsNullOrEmpty(where) ? "SELECT * FROM Erp_Regions " : "SELECT * FROM Erp_Regions Where " + where);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 根据条件得到当前商品区域关系信息
        /// </summary>
        /// <param name="where">条件，为空则查询所有信息</param>
        /// <returns></returns>
        public DataTable GetProductRegion(string where)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.IsNullOrEmpty(where) ? "SELECT * FROM Erp_ProductRegion " : "SELECT * FROM Erp_ProductRegion Where " + where);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 根据条件得到下单列表
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public DataTable GetAgentProduct(string where)
        {
            DataTable table = new DataTable();
            string strSql = "SELECT * From vw_Hishop_BrowseProductAgentList " + (string.IsNullOrEmpty(where) ? "" : ("where " + where)) + " order by Date desc";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 根据SkuId集合删除订货列表值
        /// </summary>
        /// <param name="skuids">SkuId值集合</param>
        /// <returns></returns>
        public bool DeleteAgentProduct(string skuids, int userid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format(@"DELETE FROM Erp_AgentProduct WHERE SkuId in ({0}) and UserId = '{1}' ", skuids, userid));
            //this.database.AddInParameter(sqlStringCommand, "@SkuIds", DbType.String, skuids);
            try
            {
                this.database.ExecuteNonQuery(sqlStringCommand);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /*
        public DataTable GetRegion(string where)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.IsNullOrEmpty(where) ? "SELECT * FROM Erp_Regions " : "SELECT * FROM Erp_Regions Where" + where);
            //this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        } */
    }
}

