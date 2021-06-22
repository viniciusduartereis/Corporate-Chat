using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Corporate.Chat.Console.Client
{
    public class Worker : IHostedService
    {
        private HubConnection _hubConnection;
        private bool isConnected;
        private string name;
        private readonly ILogger<Worker> logger;
        private IConfiguration configuration;

        public Worker(IConfiguration configuration, ILogger<Worker> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("---Chat Client ---");

            logger.LogInformation("Type your name:");

            name = System.Console.ReadLine();

            await SetupSignalRHubAsync();

            do
            {
                try
                {
                    if (!isConnected)
                    {
                        await SetupSignalRHubAsync();
                    }
                    else
                    {
                        var message = System.Console.ReadLine();

                        if (!string.IsNullOrEmpty(message))
                        {
                            await _hubConnection.SendAsync("Send", new Message { Name = name, Text = message });
                            logger.LogInformation("SendAsync to Hub");
                        }
                    }
                }
                catch (Exception ex)
                {
                    isConnected = false;
                    logger.LogInformation($"Error on SendAsync: {ex.Message}");
                }
            }
            while (System.Console.ReadKey(false).Key != ConsoleKey.Escape);

            await _hubConnection.DisposeAsync();
        }

        private async Task SetupSignalRHubAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(configuration.GetValue<string>("Endpoints:ChatAPI"))
                .WithAutomaticReconnect()
                .ConfigureLogging(factory =>
                {
                    factory.AddConsole();
                    factory.AddFilter("Console", level => level >= LogLevel.Trace);
                }).Build();

            await _hubConnection.StartAsync();


            logger.LogInformation("Connected to Hub");
            logger.LogInformation("Press ESC to stop");
            logger.LogInformation("Type message:");

            isConnected = true;

            try
            {
                await _hubConnection.SendAsync("OnUserConnected", new Message { Name = name });
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Error on OnUserConnected: {ex.Message}");
                isConnected = false;
            }

            _hubConnection.HandshakeTimeout = TimeSpan.FromSeconds(3);

            _hubConnection.Closed += (args) =>
            {
                isConnected = false;
                logger.LogInformation($"Connection close {args?.Message}");
                return Task.CompletedTask;
            };

            _hubConnection.On<Message>("ReceiveMessage", (message) =>
            {
                if (message != null)
                {
                    logger.LogInformation($"Received Message -> {message.Name} said: {message.Text}");
                }
            });

            _hubConnection.On<Message>("UserConnected", (message) =>
            {
                logger.LogInformation($"{message.Text}");
            });

            _hubConnection.On<Message>("UserDisconnected", (message) =>
            {
                logger.LogInformation($"{message.Text}");
            });
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"Stop Service..");
            await _hubConnection?.DisposeAsync();
        }
    }
}