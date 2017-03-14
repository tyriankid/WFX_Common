namespace Hidistro.Entities.Comments
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class CategoryQuery 
    {
        public int CategoryId { get; set; }

        public int ClientUserId { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public string AgentName { get; set;}
    }
}

