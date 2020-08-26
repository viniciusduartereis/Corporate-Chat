using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Corporate.Chat.API.Configurations
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                        Title = "Chat API",
                        Description = "Chat API SignalR Example",
                        Contact = new OpenApiContact { Name = "Vinícius Reis", Email = "viniciusduartereis@icloud.com" },
                        License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://github.com/viniciusduartereis/Corporate-Chat/blob/master/LICENSE") }
                });
                c.EnableAnnotations();
            });
        }

        public static void UseSwaggerSetup(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Chat API V1");
            });
        }
    }
}