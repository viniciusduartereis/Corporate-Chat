using System.Threading.Tasks;
using Corporate.Chat.Domain.Model;

namespace Corporate.Chat.Application.Interfaces
{
    public interface IUserChatAppService : IAppService<UserChat>
    {
        Task<UserChat> GetByConnectionIdAsync(string connectionId);
    }
}