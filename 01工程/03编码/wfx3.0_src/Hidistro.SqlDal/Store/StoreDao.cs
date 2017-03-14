namespace Hidistro.SqlDal.Store
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;
    using System.Text;

    public class StoreDao
    {
        private Database database = DatabaseFactory.CreateDatabase();


        public DeliveryMemberInfo GetDeliveryMemberInfo(int userid)
        {
            string selectSql = string.Format(@"Select * From store_deliveryMember Where DeliveryUserId={0}", userid);
            DbCommand command = this.database.GetSqlStringCommand(selectSql);
            using (IDataReader reader = this.database.ExecuteReader(command))
            {
                return ReaderConvert.ReaderToModel<DeliveryMemberInfo>(reader);
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public bool CreateDeliveryMember(DeliveryMemberInfo entity, bool isAdd)
        {
            try
            {
                string execSql = (isAdd) ?
                            "insert into store_deliveryMember (Addtime,UserName,Phone,DeliveryState,[State],StoreId,Sex) values (@Addtime,@UserName,@Phone,@DeliveryState,@State,@StoreId,@Sex)" :
                            "update store_deliveryMember set AddTime = @Addtime,UserName = @UserName,Phone=@Phone,DeliveryState=@DeliveryState,[State]=@State,StoreId=@StoreId,Sex=@Sex";
                SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@Addtime",entity.AddTime),
					new SqlParameter("@UserName",entity.UserName),
					new SqlParameter("@Phone",entity.Phone),
					new SqlParameter("@DeliveryState",entity.DeliveryState),
					new SqlParameter("@State",entity.State),
					new SqlParameter("@StoreId",entity.StoreId),
					new SqlParameter("@Sex",entity.Sex),
				};
                DbCommand sqlCommand = this.database.GetSqlStringCommand(execSql);
                this.database.AddInParameter(sqlCommand, "Addtime", DbType.DateTime, entity.AddTime);
                this.database.AddInParameter(sqlCommand, "UserName", DbType.String, entity.UserName);
                this.database.AddInParameter(sqlCommand, "Phone", DbType.String, entity.Phone);
                this.database.AddInParameter(sqlCommand, "DeliveryState", DbType.Int32, entity.DeliveryState);
                this.database.AddInParameter(sqlCommand, "State", DbType.Int32, entity.State);
                this.database.AddInParameter(sqlCommand, "StoreId", DbType.Int32, entity.StoreId);
                this.database.AddInParameter(sqlCommand, "Sex", DbType.Int32, entity.Sex);


                return this.database.ExecuteNonQuery(sqlCommand) > 0;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 门店商品列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetDeliveryMemberList(DeliveryMemberQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1 ");
            
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND AddTime >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND AddTime <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                builder.AppendFormat(" AND  UserName = '{0}' ", query.UserName);
            }
            if (query.StoreId > 0)
            {
                builder.AppendFormat(" AND  StoreId = '{0}' ", query.StoreId);
            }
            string selectFields = "*";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Store_DeliveryMember", "DeliveryUserId", builder.ToString(), selectFields);
        }

    }
}

