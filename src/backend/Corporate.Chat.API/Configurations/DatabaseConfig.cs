using System;
using System.Linq;
using Corporate.Chat.Infra.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Corporate.Chat.API.Configurations
{
    public static class DatabaseConfig
    {
        public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<ChatContext>((serviceProvider, options) =>
                {
                    options.UseApplicationServiceProvider(serviceProvider);
                    options.UseInternalServiceProvider(serviceProvider);
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                });

        }

        public static void SeedDatabaseSetup(this IApplicationBuilder app)
        {
            // Ensure SQL Database Created
            using(var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using(var context = serviceScope.ServiceProvider.GetService<ChatContext>())
            {
                try
                {
                    context.Database.EnsureCreated();
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}