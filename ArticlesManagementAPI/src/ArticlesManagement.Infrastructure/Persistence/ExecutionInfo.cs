using ArticlesManagement.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Infrastructure.Persistence
{
    public class ExecutionInfo : IExecutionInfo
    {
        public int? UserId { get; set; }
    }
}
