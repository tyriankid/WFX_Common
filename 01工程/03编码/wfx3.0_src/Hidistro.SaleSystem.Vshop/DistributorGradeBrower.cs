namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.SqlDal.Members;
    using System;
    using System.Data;

    public class DistributorGradeBrower
    {
        public static bool CreateDistributorGrade(DistributorGradeInfo distributorgrade)
        {
            return new DistributorGradeDao().CreateDistributorGrade(distributorgrade);
        }
        public static bool CreateAgentGrade(AgentGradeInfo agentgrade)
        {
            return new AgentGradeDao().CreateAgentGrade(agentgrade);
        }

        public static string DelOneGrade(int gradeid)
        {
            return new DistributorGradeDao().DelOneGrade(gradeid);
        }
        public static string DelOneAgentGrade(int gradeid)
        {
            return new AgentGradeDao().DelOneGrade(gradeid);
        }

        public static DataTable GetAllDistributorGrade()
        {
            return new DistributorGradeDao().GetAllDistributorGrade();
        }
        public static DataTable GetAllAgentGrade()
        {
            return new AgentGradeDao().GetAllAgentGrade();
        }

        public static DistributorGradeInfo GetDistributorGradeInfo(int gradeid)
        {
            return new DistributorGradeDao().GetDistributorGradeInfo(gradeid);
        }


        public static Channel GetChannelListGrade(Guid ChannelId)
        {
            return new DistributorGradeDao().GetChannelListGrade(ChannelId);
        }

        public static bool InsertChannelList(Channel query)
        {
            return new DistributorGradeDao().InsertChannelList(query);
        }

        public static bool UpdateChannelList(Channel Channel)
        {
            return new DistributorGradeDao().UpdateChannelList(Channel);
        }

        public static AgentGradeInfo GetAgentGradeInfo(int gradeid)
        {
            return new AgentGradeDao().GetAgentGradeInfo(gradeid);
        }

        public static DataTable GetChannelList()
        {
            return new DistributorGradeDao().GetChannelList();
        }

        public static DbQueryResult GetDistributorGradeRequest(DistributorGradeQuery query)
        {
            return new DistributorGradeDao().GetDistributorGradeRequest(query);
        }


        public static DbQueryResult GetChannelListGradeRequest(Channel query)
        {
            return new DistributorGradeDao().GetChannelListGradeRequest(query);
        }


        public static DbQueryResult GetAgentGradeRequest(AgentGradeQuery query)
        {
            return new AgentGradeDao().GetAgentGradeRequest(query);
        }

        public static DistributorGradeInfo GetIsDefaultDistributorGradeInfo()
        {
            return new DistributorGradeDao().GetIsDefaultDistributorGradeInfo();
        }

        public static bool IsExistsMinAmount(int gradeid, decimal minorderamount)
        {
            return new DistributorGradeDao().IsExistsMinAmount(gradeid, minorderamount);
        }

        public static bool SetGradeDefault(int gradeid)
        {
            return new DistributorGradeDao().SetGradeDefault(gradeid);
        }
        public static bool SetAgentGradeDefault(int gradeid)
        {
            return new AgentGradeDao().SetGradeDefault(gradeid);
        }

        public static bool UpdateDistributor(DistributorGradeInfo distributorgrade)
        {
            return new DistributorGradeDao().UpdateDistributor(distributorgrade);
        }
        public static bool UpdateAgent(AgentGradeInfo agnetgrade)
        {
            return new AgentGradeDao().UpdateAgent(agnetgrade);
        }
    }
}

