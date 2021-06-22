using System.Threading.Tasks;
using Corporate.Chat.Domain.Interfaces.Repositories;
using Corporate.Chat.Domain.Model;
using Corporate.Chat.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Corporate.Chat.Infra.Data.Repositories
{
    public class UserChatRepository : Repository<UserChat>, IUserChatRepository
    {
        public UserChatRepository(ChatContext context) : base(context) { }

        public async Task<UserChat> GetByConnectionIdAsync(string connectionId)
        {
            return await context.UsersChat.AsNoTracking().FirstOrDefaultAsync(x => x.ConnectionId == connectionId);
        }
    }
}