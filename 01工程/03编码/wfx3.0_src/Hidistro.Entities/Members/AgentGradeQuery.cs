namespace Hidistro.Entities.Members
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class AgentGradeQuery : Pagination
    {
        public string Description { get; set; }

        public int AgentGradeId { get; set; }

        public string AgentGradeName { get; set; }
    }
}

