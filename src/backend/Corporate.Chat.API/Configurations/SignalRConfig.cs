using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Corporate.Chat.API.Configurations
{
    public static class SignalRConfig
    {
        public static void SignalRConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSignalR()
                .AddJsonProtocol(x =>
                {
                    x.PayloadSerializerOptions.IgnoreNullValues = false;
                    x.PayloadSerializerOptions.PropertyNameCaseInsensitive = false;
                    x.PayloadSerializerOptions.WriteIndented = false;
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

                        config.EndPoints.Add(configuration.GetConnectionString("Redis"), 6379);
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
        }
    }
}