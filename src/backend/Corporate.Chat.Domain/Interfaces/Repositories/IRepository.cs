using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Corporate.Chat.Domain.Interfaces.Model;
using Corporate.Chat.Domain.Pagination;

namespace Corporate.Chat.Domain.Interfaces.Repositories
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));

        bool Delete(T entity);

        bool Delete(int id);

        Task<T> GetByIdAsync(int id);

        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<PagedResult<T>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default(CancellationToken));

        T Update(T entity);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}