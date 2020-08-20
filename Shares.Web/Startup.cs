using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shares.Core.Extensions;
using Shares.Web.Auth;
using Shares.Web.Services;

namespace Shares.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("Development", cors => cors
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:4200"));
            });
            services.AddSwaggerGen();

            services.ConfigureCache();
            services.AddSingleton<ITokenValidationService, TokenValidationService>();
            services.AddSingleton<AuthorizationFilter>();
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AuthorizedUser>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Shares API"); });

            app.UseRouting();
            app.UseCors("Development");
            app.UseEndpoints(routes => routes.MapControllers());
        }
    }
}
