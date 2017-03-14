namespace Hidistro.Entities.Members
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class Channel : Pagination
    {
        public Guid Id { get; set; }

        public string  Remark { get; set; }

        public string ChannelName { get; set; }
    }
}

