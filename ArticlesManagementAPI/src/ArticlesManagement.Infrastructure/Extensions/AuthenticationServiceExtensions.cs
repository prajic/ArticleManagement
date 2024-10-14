using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ArticlesManagement.Infrastructure.Extensions
{
    public static class AuthenticationServiceExtensions
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Authentication:Jwt:Issuer"],
                ValidAudience = configuration["AuthenticationJwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authentication:Jwt:Key"]))
                };
            });


            //services.AddAuthentication()
            //.AddGoogle(options =>
            //{
            //    options.ClientId = configuration["Authentication:Google:ClientId"];
            //    options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
            //})
            //.AddApple(options =>
            //{
            //    options.ClientId = configuration["Authentication:Apple:ClientId"];
            //    options.KeyId = configuration["Authentication:Apple:KeyId"];
            //    options.TeamId = configuration["Authentication:Apple:TeamId"];
            //    //options.PrivateKey = configuration["Authentication:Apple:PrivateKey"];
            //});

            return services;
        }
    }
}
