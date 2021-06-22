using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Corporate.Chat.Domain.Interfaces.Model;
using Corporate.Chat.Domain.Interfaces.Repositories;
using Corporate.Chat.Domain.Pagination;
using Corporate.Chat.Infra.Data.Context;
using Corporate.Chat.Infra.Data.Helper;
using Microsoft.EntityFrameworkCore;

namespace Corporate.Chat.Infra.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly ChatContext context;
        private readonly DbSet<T> dbSet;

        public Repository(ChatContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await dbSet.AddAsync(entity, cancellationToken);
            return result.Entity;
        }
        public bool Delete(T entity)
        {
            var result = this.dbSet.Remove(entity);

            return result.State == EntityState.Deleted;
        }
        public bool Delete(int id)
        {
            var entity = this.dbSet.Find(id);

            if (entity != null)
            {
                return this.Delete(entity);
            }

            return false;
        }
        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.dbSet.AsNoTracking().ToListAsync(cancellationToken);
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await this.dbSet.FindAsync(id);
        }

        public async Task<PagedResult<T>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.dbSet.AsNoTracking().GetPagedAsync(page, pageSize, cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.context.SaveChangesAsync();
        }
        public T Update(T entity)
        {
            var result = this.dbSet.Update(entity);

            return result.Entity;
        }
    }
}