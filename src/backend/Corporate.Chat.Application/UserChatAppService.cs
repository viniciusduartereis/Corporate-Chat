using System.Threading.Tasks;
using Corporate.Chat.Application.Interfaces;
using Corporate.Chat.Domain.Interfaces.Repositories;
using Corporate.Chat.Domain.Model;

namespace Corporate.Chat.Application
{
    public class UserChatAppService : AppService<UserChat>, IUserChatAppService
    {
        private readonly IUserChatRepository repository;
        public UserChatAppService(IUserChatRepository repository) : base(repository)
        {
            this.repository = repository;
        }
        public async Task<UserChat> GetByConnectionIdAsync(string connectionId)
        {
            return await this.repository.GetByConnectionIdAsync(connectionId);
        }
    }
}