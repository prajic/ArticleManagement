using ArticlesManagement.Domain.Entities;
using ArticlesManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Application.Models.Requests
{
    public class CreateArticleRequest
    {
        public required string Title { get; set; }

        public string? Subtitle { get; set; }

        public required string Content { get; set; }
        public DateTime PublishDate { get; set; }

        public int AuthorId { get; set; }

        public int ReadCount { get; set; }

        public ArticleType Type { get; set; }

        public List<int>? OwnerIds { get; set; }

    }
}
