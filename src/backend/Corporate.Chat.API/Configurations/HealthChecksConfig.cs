using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Corporate.Chat.API.Configurations
{
    public static class HealthChecksConfig
    {
        public static void AddHealthCheckConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Health Checks
            services.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("DefaultConnection"), name: "MSSQL")
                .AddRedis(configuration.GetConnectionString("Redis"), name: "REDIS");

            services
                .AddHealthChecksUI();

        }

        public static void UseHealthChecksSetup(this IApplicationBuilder app)
        {
            // Dashboard health check UI
            app.UseHealthChecksUI();

            /*
            //Middlweare Health Check
            app.UseHealthChecks("/status",
                new HealthCheckOptions()
                {
                    ResponseWriter = async(context, report) =>
                    {
                        var result = JsonConvert.SerializeObject(
                            new
                            {
                                statusApplication = report.Status.ToString(),
                                    healthChecks = report.Entries.Select(e => new
                                    {
                                        check = e.Key,
                                            ErrorMessage = e.Value.Exception?.Message,
                                            status = Enum.GetName(typeof(HealthStatus), e.Value.Status)
                                    })
                            });
                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        await context.Response.WriteAsync(result);
                    }
                });

            // Endpoint for dashboard
            app.UseHealthChecks("/healthchecks-data-ui", new HealthCheckOptions()
            {
                Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            */
        }
    }
}