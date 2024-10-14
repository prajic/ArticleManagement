using ArticlesManagement.Application.Abstractions;
using ArticlesManagement.Application.Helpers;
using ArticlesManagement.Application.Interfaces;
using ArticlesManagement.Application.Mappings;
using ArticlesManagement.Application.Services;
using ArticlesManagement.Domain.Abstractions;
using ArticlesManagement.Infrastructure.Extensions;
using ArticlesManagement.Infrastructure.Persistence;
using ArticlesManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace ArticlesManagement.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method is used to configure services
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAuthenticationServices(_configuration);
            services.AddAuthorization();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Articles Management API", Version = "v1" });

                // Configure JWT Bearer authentication
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token as follows: Bearer {token}",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });


            // Add other services as needed

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IExecutionInfo, ExecutionInfo>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();


            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IArticleService, ArticleService>();

            services.AddAutoMapper(typeof(MappingProfile)); // Reference to your mapping profile

        }

        // This method is used to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Use authentication and authorization middlewares
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseTokenValidationMiddleware();
            app.UseExecutionInfoMiddleware();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

}
