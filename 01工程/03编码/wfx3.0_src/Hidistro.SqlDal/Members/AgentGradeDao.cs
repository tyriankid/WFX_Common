namespace Hidistro.SqlDal.Members
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class AgentGradeDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool CreateAgentGrade(AgentGradeInfo agentrgrade)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_AgentGrade(AgentGradeName,Description,FirstCommissionRise,Ico) VALUES(@AgentGradeName,@Description,@FirstCommissionRise,@Ico);select @@identity");
            this.database.AddInParameter(sqlStringCommand, "AgentGradeName", DbType.String, agentrgrade.AgentGradeName);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, agentrgrade.Description);
            //this.database.AddInParameter(sqlStringCommand, "CommissionsLimit", DbType.Decimal, agentrgrade.CommissionsLimit);
            this.database.AddInParameter(sqlStringCommand, "FirstCommissionRise", DbType.Decimal, agentrgrade.FirstCommissionRise);
            //this.database.AddInParameter(sqlStringCommand, "SecondCommissionRise", DbType.Decimal, agentrgrade.SecondCommissionRise);
            //this.database.AddInParameter(sqlStringCommand, "ThirdCommissionRise", DbType.Decimal, agentrgrade.ThirdCommissionRise);
            //this.database.AddInParameter(sqlStringCommand, "IsDefault", DbType.Boolean, agentrgrade.IsDefault);
            this.database.AddInParameter(sqlStringCommand, "Ico", DbType.String, agentrgrade.Ico);
            int gradeid = int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
            bool flag = gradeid > 0;
            //if (flag && agentrgrade.IsDefault)
            //{
            //    this.SetGradeDefault(gradeid);
            //}
            return flag;
        }

        public string DelOneGrade(int gradeid)
        {
            if (this.HasAgent(gradeid))
            {
                return "-1";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete  aspnet_AgentGrade  where AgentGradeId=@AgentGradeId");
            this.database.AddInParameter(sqlStringCommand, "AgentGradeId", DbType.Int32, gradeid);
            return ((this.database.ExecuteNonQuery(sqlStringCommand) > 0) ? "1" : "0");
        }

        public DataTable GetAllAgentGrade()
        {
            string query = "select * from aspnet_AgentGrade ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DbQueryResult GetAgentGrade(AgentGradeQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.AgentGradeId > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" AgentGradeId = {0}", query.AgentGradeId);
            }
            if (!string.IsNullOrEmpty(query.AgentGradeName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" AgentGradeName LIKE '%{0}%'", DataHelper.CleanSearchString(query.AgentGradeName));
            }
            if (!string.IsNullOrEmpty(query.Description))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" Description LIKE '%{0}%'", DataHelper.CleanSearchString(query.Description));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "aspnet_AgentGrade", "AgentGradeId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public AgentGradeInfo GetAgentGradeInfo(int agentGradeid)
        {
            if (agentGradeid <= 0)
            {
                return null;
            }
            AgentGradeInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM aspnet_AgentGrade where AgentGradeId={0}", agentGradeid));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateAgentGradeInfo(reader);
                }
            }
            return info;
        }

        public IList<AgentGradeInfo> GetAgentGradeInfos()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_AgentGrade");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<AgentGradeInfo>(reader);
            }
        }

        public DbQueryResult GetAgentGradeRequest(AgentGradeQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(query.AgentGradeName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" AgentGradeName LIKE '%{0}%'", DataHelper.CleanSearchString(query.AgentGradeName));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "aspnet_AgentGrade ", "AgentGradeId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        //public AgentGradeInfo GetIsDefaultAgentGradeInfo()
        //{
        //    AgentGradeInfo info = null;
        //    DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM aspnet_AgentGrade where IsDefault=1 order by CommissionsLimit asc", new object[0]));
        //    using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
        //    {
        //        if (reader.Read())
        //        {
        //            info = DataMapper.PopulateAgentGradeInfo(reader);
        //        }
        //    }
        //    return info;
        //}

        public bool HasAgent(int greadeid)
        {
            string query = "select * from aspnet_Distributors where AgentGradeId=@AgentGradeId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "AgentGradeId", DbType.Int32, greadeid);
            return (this.database.ExecuteDataSet(sqlStringCommand).Tables[0].Rows.Count > 0);
        }

        //public bool IsExistsMinAmount(int gradeid, decimal minorderamount)
        //{
        //    bool flag = false;
        //    string query = "select top 1 AgentGradeId from aspnet_AgentGrade where CommissionsLimit=" + minorderamount;
        //    if (gradeid > 0)
        //    {
        //        query = query + " and GradeId<>" + gradeid;
        //    }
        //    DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
        //    IDataReader reader = this.database.ExecuteReader(sqlStringCommand);
        //    if (reader.Read())
        //    {
        //        flag = true;
        //    }
        //    reader.Close();
        //    return flag;
        //}

        public bool SetGradeDefault(int agentGradeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_AgentGrade set IsDefault=0 where AgentGradeId<>@AgentGradeId;UPDATE aspnet_AgentGrade set IsDefault=1 where AgentGradeId=@AgentGradeId");
            this.database.AddInParameter(sqlStringCommand, "AgentGradeId", DbType.Int32, agentGradeId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateAgent(AgentGradeInfo agentgrade)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_AgentGrade SET AgentGradeName=@AgentGradeName,Description=@Description,FirstCommissionRise=@FirstCommissionRise,Ico=@Ico WHERE AgentGradeId=@AgentGradeId");
            this.database.AddInParameter(sqlStringCommand, "AgentGradeId", DbType.Int32, agentgrade.AgentGradeId);
            this.database.AddInParameter(sqlStringCommand, "AgentGradeName", DbType.String, agentgrade.AgentGradeName);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, agentgrade.Description);
            this.database.AddInParameter(sqlStringCommand, "FirstCommissionRise", DbType.Decimal, agentgrade.FirstCommissionRise);
            this.database.AddInParameter(sqlStringCommand, "Ico", DbType.String, agentgrade.Ico);
            bool flag = this.database.ExecuteNonQuery(sqlStringCommand) > 0;
            return flag;
        }
    }
}

