using System.Linq;
using System.Threading.Tasks;
using Corporate.Chat.API.Configurations;
using Corporate.Chat.API.Hubs;
using Corporate.Chat.Infra.CrossCutting;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Corporate.Chat.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;

        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddControllers();

            services.AddMvc(config =>
                {
                    config.Filters.Add(new ProducesAttribute("application/json"));
                })
                .AddFluentValidation()
                .AddJsonOptions(x =>
                {
                    x.JsonSerializerOptions.WriteIndented = false;
                    x.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
                    x.JsonSerializerOptions.IgnoreNullValues = false;
                });

            services.AddResponseCompressionConfiguration();

            services.SignalRConfiguration(Configuration);

            services.AddCors();

            services.AddDatabaseConfiguration(Configuration);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.RegisterDependecies();

            services.AddHealthCheckConfiguration(Configuration);

            services.AddSwaggerConfiguration();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(policy =>
            {
                var origins = Configuration.GetSection("Cors:Host")
                    .AsEnumerable()
                    .Where(x => !string.IsNullOrEmpty(x.Value))
                    .Select(x => x.Value).ToArray();

                policy.WithOrigins(origins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseResponseCompression();

            app.UseRouting();

            app.UseSwaggerSetup();

            app.UseHttpsRedirection();

            app.UseHealthChecksSetup();

            app.SeedDatabaseSetup();

            app.UseEndpoints((endpoints) =>
            {
                endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapControllers();

                endpoints.MapHealthChecks("healthz", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecksUI();
                endpoints.MapDefaultControllerRoute();;

            });

            app.Run(context =>
            {
                context.Response.Redirect("/swagger");
                return Task.CompletedTask;
            });

        }
    }
}