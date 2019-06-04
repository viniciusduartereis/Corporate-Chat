using System;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Corporate.Chat.API.Configurations;
using Corporate.Chat.API.Context;
using Corporate.Chat.API.Hubs;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;

namespace Corporate.Chat.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            var serializeSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DefaultValueHandling = DefaultValueHandling.Include,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Include,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            services.AddMvc(config =>
                {
                    config.UseCentralRoutePrefix(new RouteAttribute($"api/{SwaggerConfig.GetVersion()}"));

                    config.Filters.Add(new ProducesAttribute("application/json"));

                })
                .AddFluentValidation()
                .AddJsonOptions(x =>
                {
                    x.SerializerSettings.Formatting = Formatting.Indented;
                    x.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    x.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    x.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                    x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    x.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                    x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);

            services.AddResponseCompression(options => options.Providers.Add<GzipCompressionProvider>());

            services.AddSignalR()
                .AddJsonProtocol(x =>
                {
                    x.PayloadSerializerSettings = serializeSettings;
                })
                .AddMessagePackProtocol()
                .AddStackExchangeRedis(o =>
                {
                    o.ConnectionFactory = async writer =>
                    {
                        var config = new StackExchange.Redis.ConfigurationOptions
                        {
                            AbortOnConnectFail = false,
                            ResolveDns = true
                        };

                        config.EndPoints.Add(Configuration.GetConnectionString("Redis"), 6379);
                        config.SetDefaultPorts();
                        var connection = await ConnectionMultiplexer.ConnectAsync(config, writer);
                        connection.ConnectionFailed += (_, e) =>
                        {
                            Console.WriteLine("Connection to Redis failed.");
                        };

                        if (!connection.IsConnected)
                        {
                            Console.WriteLine("Did not connect to Redis.");
                        }

                        return connection;
                    };
                });

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc(SwaggerConfig.GetVersion(), SwaggerConfig.GetSwashbuckleApiInfo());
                config.DescribeAllEnumsAsStrings();
                config.DescribeStringEnumsInCamelCase();
                config.UseReferencedDefinitionsForEnums();
                config.IncludeXmlComments(SwaggerConfig.GetApiXmlCommentsPath());
                config.EnableAnnotations();
            });

            services.AddCors();

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<ChatContext>((serviceProvider, options) =>
                {
                    options.UseApplicationServiceProvider(serviceProvider);
                    options.UseInternalServiceProvider(serviceProvider);
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

                });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ChatContext>();

            // Health Checks
            services.AddHealthChecks()
                .AddSqlServer(Configuration.GetConnectionString("DefaultConnection"), name: "MSSQL")
                .AddRedis(Configuration.GetConnectionString("Redis"), name: "REDIS");

            services.AddHealthChecksUI();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
                policy.WithOrigins(Configuration.GetSection("Cors:Host").Value)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseResponseCompression();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chat");
            });

            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.Host = httpReq.Host.Value);
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{SwaggerConfig.GetVersion()}/swagger.json", SwaggerConfig.GetApplicationName());
            });

            app.UseHttpsRedirection();
            app.UseMvc();

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

            // Dashboard health check UI
            app.UseHealthChecksUI();

            // Ensure SQL Database Created
            using(var context = app.ApplicationServices.GetService<ChatContext>())
            {
                try
                {
                    context.Database.EnsureCreated();
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            app.Run(context =>
            {
                context.Response.Redirect("/swagger");
                return Task.CompletedTask;
            });

        }
    }
}