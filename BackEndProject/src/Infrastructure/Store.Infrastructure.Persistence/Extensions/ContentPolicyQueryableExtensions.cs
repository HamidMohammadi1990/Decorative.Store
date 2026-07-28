using System.Linq.Expressions;

namespace Store.Infrastructure.Persistence.Extensions;

public static class ContentPolicyQueryableExtensions
{
    public static IQueryable<TEntity> ApplyContentPolicyFilter<TEntity>(
        this IQueryable<TEntity> query,
        Expression<Func<TEntity, bool>>? contentFilter)
        where TEntity : class
        => contentFilter is null ? query : query.Where(contentFilter);
}