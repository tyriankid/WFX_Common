using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hidistro.SqlDal.Comments
{
    public class StoreMessageDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddStoreMessage(StoreMessage msg)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Vshop_StoreMessage VALUES (@DisUserID, @MsgUserID, @MessaegeCon,  @MsgTime, @Value1, @Value2, @Value3)");
            this.database.AddInParameter(sqlStringCommand, "DisUserID", DbType.Int32, msg.DisUserID);
            this.database.AddInParameter(sqlStringCommand, "MsgUserID", DbType.Int32, msg.MsgUserID);
            this.database.AddInParameter(sqlStringCommand, "MessaegeCon", DbType.String, msg.MessaegeCon);
            this.database.AddInParameter(sqlStringCommand, "MsgTime", DbType.DateTime, msg.MsgTime);
            this.database.AddInParameter(sqlStringCommand, "Value1", DbType.String, msg.Value1);
            this.database.AddInParameter(sqlStringCommand, "Value2", DbType.String, msg.Value2);
            this.database.AddInParameter(sqlStringCommand, "Value3", DbType.String, msg.Value3);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public bool DeleteStoreMessage(int ID)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Vshop_StoreMessage WHERE ID = @ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, ID);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public StoreMessage GetModel(int ID)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_Hishop_StoreMessage WHERE ID = @ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, ID);
            StoreMessage info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = ReaderConvert.ReaderToModel<StoreMessage>(reader);
                }
            }
            return info;
        }

        public DbQueryResult GetMsgList(StoreMessageQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
            if (query.MsgUserID > 0)
            {
                builder.AppendFormat(" and MsgUserID={0}",query.MsgUserID);
            }
            if (query.DisUserID > 0)
            {
                builder.AppendFormat(" and DisUserID={0}", query.DisUserID);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_StoreMessage", "ID", builder.ToString(), "*");
        }

        public DataTable GetMyMsg(int UserID)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from vw_Hishop_StoreMessage where MsgUserID=@UserID");
            this.database.AddInParameter(sqlStringCommand, "UserID", DbType.Int32,UserID);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }
    }
}
