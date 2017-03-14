namespace Hidistro.Entities.Comments
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ArticleQuery : Pagination
    {
        public int? CategoryId { get; set; }

        public DateTime? EndArticleTime { get; set; }

        public string Keywords { get; set; }

        public DateTime? StartArticleTime { get; set; }
    }
}

