using Corporate.Chat.Domain.Interfaces.Repositories;
using Corporate.Chat.Domain.Model;
using Corporate.Chat.Infra.Data.Context;

namespace Corporate.Chat.Infra.Data.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(ChatContext context) : base(context)
        { }
    }
}