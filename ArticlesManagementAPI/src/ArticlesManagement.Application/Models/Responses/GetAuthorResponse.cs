using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Application.Models.Responses
{
    public class GetAuthorResponse
    {
        public int UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Biography { get; set; }
        public string? SocialMediaLinks { get; set; }

        public string? ImageUrl { get; set; }
        public string? Website { get; set; }
        public int PublishedArticlesCount { get; set; }
    }
}
