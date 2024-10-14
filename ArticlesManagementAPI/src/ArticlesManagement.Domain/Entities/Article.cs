using ArticlesManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Domain.Entities
{
    public class Article: BaseEntity
    {
        public required string Title { get; set; }

        public string? Subtitle { get; set; }

        public required string Content { get; set; }
        public DateTime PublishDate { get; set; }

        public int AuthorId { get; set; }

        public int ReadCount { get; set; }

        public ArticleType Type { get; set; }

        public ICollection<ArticleOwner> ArticleOwners { get; set; } = new List<ArticleOwner>();

        public ICollection<ArticleLike>? ArticleLikes { get; set; }

    }
}
