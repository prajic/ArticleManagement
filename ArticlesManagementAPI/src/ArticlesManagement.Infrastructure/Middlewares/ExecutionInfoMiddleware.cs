using ArticlesManagement.Application.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Infrastructure.Middlewares
{
    public class ExecutionInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public ExecutionInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IExecutionInfo executionInfo)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                executionInfo.UserId = int.Parse(userIdClaim.Value);
            }

            await _next(context);
        }

    }
}
