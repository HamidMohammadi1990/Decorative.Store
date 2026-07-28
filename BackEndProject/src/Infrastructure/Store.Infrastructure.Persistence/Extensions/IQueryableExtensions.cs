using Microsoft.EntityFrameworkCore;
using Store.Domain.Dtos.Pagination;

namespace Store.Infrastructure.Persistence.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<T> Pagination<T>(this IQueryable<T> query, PagedRequest pagination)
    {
        return query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize);
    }

    public static async Task<PagedResult<T>> ToPagedAsync<T>(
        this IQueryable<T> query,
        PagedRequest pagedRequest,
        CancellationToken cancellationToken = default)
        where T : class
    {
        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Pagination(pagedRequest)            
            .ToListAsync(cancellationToken);

        return PagedResult<T>.Create(items, pagedRequest, totalCount);
    }
}