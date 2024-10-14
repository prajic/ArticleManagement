using ArticlesManagement.Application.Interfaces;
using ArticlesManagement.Application.Services;
using ArticlesManagement.Domain.Abstractions;
using ArticlesManagement.Infrastructure.Extensions;
using ArticlesManagement.Infrastructure.Persistence;
using ArticlesManagement.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace ArticlesManagement.Infrastructure.ConfigureExtensions
{
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthenticationServices(configuration); 
            services.AddAuthorizationServices();

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IAuthService, AuthService>();

        }


        public static void ConfigureMiddleware(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseTokenValidationMiddleware(); 
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
