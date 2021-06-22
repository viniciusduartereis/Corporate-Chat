using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Corporate.Chat.Domain.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Corporate.Chat.Infra.Data.Helper
{
    public static class PagedHelper
    {
        public static async Task<PagedResult<T>> GetPagedAsync<T>(this IQueryable<T> query,
            int page, int pageSize, CancellationToken cancellationToken = default(CancellationToken))where T : class
        {
            var result = new PagedResult<T>
            {
            CurrentPage = page,
            PageSize = pageSize,
            RowCount = query.Count()
            };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = await query.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);

            return result;
        }

    }

}