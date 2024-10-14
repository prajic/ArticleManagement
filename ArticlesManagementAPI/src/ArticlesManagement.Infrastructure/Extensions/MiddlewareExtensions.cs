using ArticlesManagement.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Infrastructure.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenValidationMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<TokenValidationMiddleware>();
        }

        public static IApplicationBuilder UseExecutionInfoMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExecutionInfoMiddleware>();
        }
    }
}
