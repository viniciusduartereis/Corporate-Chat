using System;
using System.Threading.Tasks;
using Corporate.Chat.Application.Interfaces;
using Corporate.Chat.Domain.Model;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Corporate.Chat.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> logger;
        private readonly IUserChatAppService userChatAppService;
        private readonly IMessageAppService messageAppService;
        public ChatHub(IUserChatAppService userChatAppService, IMessageAppService messageAppService, ILogger<ChatHub> logger)
        {
            this.messageAppService = messageAppService;
            this.userChatAppService = userChatAppService;
            this.logger = logger;
        }

        public async Task Send(Message message)
        {
            try
            {

                message.EventId = Event.Message.GetHashCode();
                message.CreatedDate = DateTime.Now;

                var userChat = await userChatAppService.GetByConnectionIdAsync(Context.ConnectionId);
                if (userChat != null)
                {
                    message.UserChatId = userChat.UserChatId;
                    await messageAppService.AddAsync(message);
                    await messageAppService.SaveChangesAsync();
                }

                await Clients.All.SendAsync("ReceiveMessage", message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }

        public async Task OnUserConnected(Message message)
        {
            try
            {
                message.EventId = Event.Connect.GetHashCode();
                message.CreatedDate = DateTime.Now;

                var userChat = await userChatAppService.GetByConnectionIdAsync(Context.ConnectionId);

                if (userChat != null)
                {
                    userChat.Name = message.Name;
                    userChat.UpdatedDate = DateTime.Now;
                    userChatAppService.Update(userChat);
                    await userChatAppService.SaveChangesAsync();
                }
                else
                {
                    userChat = new UserChat();
                    userChat.ConnectionId = Context.ConnectionId;
                    userChat.Name = message.Name;
                    userChat.CreatedDate = DateTime.Now;
                    await userChatAppService.AddAsync(userChat);
                    await userChatAppService.SaveChangesAsync();
                }

                message.UserChatId = userChat.UserChatId;
                message.Text = $"{message.Name} connected.";

                await messageAppService.AddAsync(message);
                await messageAppService.SaveChangesAsync();

                logger.LogWarning($@"User ConnectionId: {Context.ConnectionId}  Name: {Context.User?.Identity?.Name} Identifier: {Context.UserIdentifier} connected.");

                await Clients.All.SendAsync("UserConnected", message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }

        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            logger.LogWarning($@"User ConnectionId: {Context.ConnectionId}  Name: {Context.User?.Identity?.Name} Identifier: {Context.UserIdentifier} disconnected.");

            var userChat = await userChatAppService.GetByConnectionIdAsync(Context.ConnectionId);
            if (userChat != null)
            {
                var message = new Message()
                {
                    Text = $"{userChat.Name} left.",
                    Name = userChat.Name,
                    CreatedDate = DateTime.Now,
                    UserChatId = userChat.UserChatId,
                    EventId = Event.Disconnect.GetHashCode()
                };

                await messageAppService.AddAsync(message);
                await messageAppService.SaveChangesAsync();

                await Clients.All.SendAsync("UserDisconnected", message);
            }
        }

    }
}