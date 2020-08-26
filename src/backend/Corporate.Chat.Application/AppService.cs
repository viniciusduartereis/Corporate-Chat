using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Corporate.Chat.Application.Interfaces;
using Corporate.Chat.Domain.Interfaces.Model;
using Corporate.Chat.Domain.Interfaces.Repositories;
using Corporate.Chat.Domain.Pagination;

namespace Corporate.Chat.Application
{
    public class AppService<T> : IAppService<T> where T : class, IEntity
    {
        private readonly IRepository<T> repository;
        public AppService(IRepository<T> repository)
        {
            this.repository = repository;
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await repository.AddAsync(entity, cancellationToken);
        }

        public bool Delete(T entity)
        {
            return repository.Delete(entity);
        }

        public bool Delete(int id)
        {
            return repository.Delete(id);
        }

        public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return repository.GetAllAsync(cancellationToken);
        }

        public Task<T> GetByIdAsync(int id)
        {
            return repository.GetByIdAsync(id);
        }

        public Task<PagedResult<T>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            return repository.GetPagedAsync(page, pageSize, cancellationToken);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return repository.SaveChangesAsync(cancellationToken);
        }

        public T Update(T entity)
        {
            return repository.Update(entity);
        }
    }
}