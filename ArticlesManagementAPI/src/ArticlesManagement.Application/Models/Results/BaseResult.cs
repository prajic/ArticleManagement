using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Application.Models.Results
{
    public class BaseResult<TEntity>
    {
        public IEnumerable<string>? Errors { get; set; }
        
        public TEntity? Result { get; set; }
    }
}
