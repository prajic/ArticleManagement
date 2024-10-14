using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Domain.Entities
{
    public class Comment: BaseEntity
    {
        public int ArticleId { get; set; }

        public int UserId { get; set; }

        public required string Content { get; set; }

        public int? ParentCommentId { get; set; }

    }
}
