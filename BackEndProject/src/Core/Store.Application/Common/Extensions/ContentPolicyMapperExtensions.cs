using System.Linq.Expressions;
using Edition.Application.Contracts.ContentPolicies;
using Store.Domain.Dtos.ContentPolicies;

namespace Edition.Application.Common.Extensions;

public static class ContentPolicyMapperExtensions
{
    public static TDto WithContentPolicy<TEntity, TDto>(
        this TDto dto,
        IContentPolicyFilteredRequest request)
        where TEntity : class
        where TDto : IContentPolicyQueryDto<TEntity>
    {
        dto.ContentFilter = request.GetContentPolicyFilter<TEntity>();
        return dto;
    }

    public static Expression<Func<TEntity, bool>>? GetContentPolicyFilter<TEntity>(
        this IContentPolicyFilteredRequest request)
        where TEntity : class
        => request.ContentPolicyDenyAll
            ? _ => false
            : request.ContentPolicyFilter as Expression<Func<TEntity, bool>>;
}
