using System.Threading.Tasks;
using Corporate.Chat.Domain.Model;

namespace Corporate.Chat.Domain.Interfaces.Repositories
{
    public interface IUserChatRepository : IRepository<UserChat>
    {
        Task<UserChat> GetByConnectionIdAsync(string connectionId);
    }
}