namespace Hidistro.Entities.Members
{
    using System;
    using System.Runtime.CompilerServices;

    public class AgentGradeInfo
    {
        public decimal CommissionsLimit { get; set; }

        public string Description { get; set; }

        public decimal FirstCommissionRise { get; set; }

        public int AgentGradeId { get; set; }

        public string Ico { get; set; }

        public bool IsDefault { get; set; }

        public string AgentGradeName { get; set; }

        public decimal SecondCommissionRise { get; set; }

        public decimal ThirdCommissionRise { get; set; }
    }
}

