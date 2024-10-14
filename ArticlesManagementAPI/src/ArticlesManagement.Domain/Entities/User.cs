using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Domain.Entities
{
    public class User : BaseEntity
    {
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public string? DisplayName { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsVerified { get; set; }

        public Author? Author { get; set; }
        public ICollection<ArticleOwner> ArticleOwners { get; set; } = new List<ArticleOwner>();  // Collection of ArticleOwners

    }
}
