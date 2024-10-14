using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;


namespace ArticlesManagement.Infrastructure.Extensions
{
    public static class AuthorizationServiceExtensions
    {
        public static IServiceCollection AddAuthorizationServices(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            return services;
        }
    }
}
