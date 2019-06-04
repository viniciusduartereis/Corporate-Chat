using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Corporate.Chat.API.Context;
using Corporate.Chat.API.Model;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corporate.Chat.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> logger;
        private readonly ChatContext chatContext;
        public ChatHub(ChatContext chatContext, ILogger<ChatHub> logger)
        {
            this.chatContext = chatContext;
            this.logger = logger;
        }

        public Task Send(Message message)
        {
            message.EventId = Event.Message.GetHashCode();
            message.CreatedDate = DateTime.Now;

            var userChat = chatContext.UsersChat.AsNoTracking().FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (userChat != null)
            {
                message.UserChatId = userChat.UserChatId;
                chatContext.Messages.Add(message);
                chatContext.SaveChangesAsync();
            }
            return Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task OnUserConnected(Message message)
        {
            try
            {
                message.EventId = Event.Connect.GetHashCode();
                message.CreatedDate = DateTime.Now;

                var userChat = chatContext.UsersChat.AsNoTracking().FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

                if (userChat != null)
                {
                    userChat.Name = message.Name;
                    userChat.UpdatedDate = DateTime.Now;
                    chatContext.Update(userChat);
                    await chatContext.SaveChangesAsync();
                }
                else
                {
                    userChat = new UserChat();
                    userChat.ConnectionId = Context.ConnectionId;
                    userChat.Name = message.Name;
                    userChat.CreatedDate = DateTime.Now;
                    await chatContext.AddAsync(userChat);
                    await chatContext.SaveChangesAsync();
                }

                message.UserChatId = userChat.UserChatId;
                message.Text = $"{message.Name} connected.";

                chatContext.Messages.Add(message);
                await chatContext.SaveChangesAsync();

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

            var userChat = chatContext.UsersChat.AsNoTracking().FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
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

                chatContext.Messages.Add(message);
                await chatContext.SaveChangesAsync();

                await Clients.All.SendAsync("UserDisconnected", message);
            }
        }

    }
}