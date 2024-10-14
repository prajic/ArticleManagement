using ArticlesManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Application.Models.Responses
{
    public class GetUserResponse
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public string DisplayName { get; set; }
        public string ImageUrl { get; set; }
        public bool IsVerified { get; set; }
        public GetAuthorResponse? Author { get; set; }

    }
}
