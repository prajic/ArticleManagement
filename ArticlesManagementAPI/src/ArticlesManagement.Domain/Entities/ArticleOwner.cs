using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Domain.Entities
{
    public class ArticleOwner
    {
        public int ArticleId { get; set; }
        public Article Article { get; set; }  

        public int UserId { get; set; }
        public User User { get; set; }  
    }
}
