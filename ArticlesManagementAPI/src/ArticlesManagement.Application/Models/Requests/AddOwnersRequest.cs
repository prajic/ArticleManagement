using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Application.Models.Requests
{
    public class AddOwnersRequest
    {
        public int ArticleId { get; set; }
        public List<int> NewOwnerUserIds { get; set; }
    }
}
