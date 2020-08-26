using System;
using Corporate.Chat.Application;
using Corporate.Chat.Application.Interfaces;
using Corporate.Chat.Domain.Interfaces.Repositories;
using Corporate.Chat.Infra.Data.Context;
using Corporate.Chat.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Corporate.Chat.Infra.CrossCutting
{
    public static class RegisterContainer
    {

        public static void RegisterDependecies(this IServiceCollection services)
        {
            services.AddScoped<ChatContext>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IUserChatRepository, UserChatRepository>();

            services.AddScoped(typeof(IAppService<>), typeof(AppService<>));
            services.AddScoped<IMessageAppService, MessageAppService>();
            services.AddScoped<IUserChatAppService, UserChatAppService>();
            
        }
    }
}