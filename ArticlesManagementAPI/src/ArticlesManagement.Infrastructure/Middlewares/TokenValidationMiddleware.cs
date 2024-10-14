using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Infrastructure.Middlewares
{
    public class TokenValidationMiddleware
    {

        private readonly RequestDelegate _next;

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // Here, you can extract claims or perform token validation manually
                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;

                if (!string.IsNullOrEmpty(userIdClaim))
                {
                    
                    context.Items["UserId"] = userIdClaim;
                }
            }

            await _next(context);

        }
    }}
