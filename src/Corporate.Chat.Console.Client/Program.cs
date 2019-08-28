using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corporate.Chat.Console.Client
{
    class Program
    {
        private static HubConnection _hubConnection;
        private static bool isConnected;
        private static string name;

        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        static async Task MainAsync()
        {

            System.Console.WriteLine("Type your name:");

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
                            System.Console.WriteLine("SendAsync to Hub");
                        }
                    }
                }
                catch (Exception ex)
                {
                    isConnected = false;
                    System.Console.WriteLine($"Error on SendAsync: {ex.Message}");
                }
            }
            while (System.Console.ReadKey(false).Key != ConsoleKey.Escape);

            await _hubConnection.DisposeAsync();
        }

        private static async Task SetupSignalRHubAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/chat")
                .ConfigureLogging(factory =>
                {
                    factory.AddConsole();
                    factory.AddFilter("Console", level => level >= LogLevel.Trace);
                }).Build();

            await _hubConnection.StartAsync();

            System.Console.WriteLine("Connected to Hub");
            System.Console.WriteLine("Press ESC to stop");
            System.Console.WriteLine("Type message:");

            isConnected = true;

            try
            {
                await _hubConnection.SendAsync("OnUserConnected", new Message { Name = name });
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error on OnUserConnected: {ex.Message}");
                isConnected = false;
            }

            _hubConnection.HandshakeTimeout = TimeSpan.FromSeconds(3);

            _hubConnection.Closed += (args) =>
            {
                isConnected = false;
                System.Console.WriteLine($"Connection close {args?.Message}");
                return Task.CompletedTask;
            };

            _hubConnection.On<Message>("ReceiveMessage", (message) =>
            {
                if (message != null)
                {
                    System.Console.WriteLine($"Received Message -> {message.Name} said: {message.Text}");
                }
            });

            _hubConnection.On<Message>("UserConnected", (message) =>
            {
                System.Console.WriteLine($"{message.Text}");
            });

            _hubConnection.On<Message>("UserDisconnected", (message) =>
            {
                System.Console.WriteLine($"{message.Text}");
            });
        }
    }
}
