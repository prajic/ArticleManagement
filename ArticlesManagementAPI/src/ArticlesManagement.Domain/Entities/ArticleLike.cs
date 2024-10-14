using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Domain.Entities
{
    public class ArticleLike
    {
        public int ArticleId { get; set; }

        public int UserId { get; set; }
    }
}
