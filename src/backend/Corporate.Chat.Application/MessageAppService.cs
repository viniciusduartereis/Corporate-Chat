using Corporate.Chat.Application.Interfaces;
using Corporate.Chat.Domain.Interfaces.Repositories;
using Corporate.Chat.Domain.Model;

namespace Corporate.Chat.Application
{
    public class MessageAppService : AppService<Message>, IMessageAppService
    {
        public MessageAppService(IMessageRepository repository) : base(repository) { }
    }
}