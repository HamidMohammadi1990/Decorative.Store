using System.Linq.Expressions;
using Store.Domain.Enums;
using Edition.Application.Contracts.ContentPolicies;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.ContentPolicies;

namespace Store.Infrastructure.Persistence.ContentPolicies;

public sealed class ContentPolicyEntityPreviewQuery
    (EditionDbContext context, IContentEntityTypeRegistry entityTypeRegistry)
    : IContentPolicyEntityPreviewQuery
{
    public async Task<ContentPolicyEntityPreviewData> PreviewAsync(
        string entityType,
        LambdaExpression? filter,
        int sampleSize,
        CancellationToken cancellationToken = default)
    {
        var clrType = entityTypeRegistry.GetClrType(entityType);
        var query = ContentPolicyQueryableReflection.GetQueryable(context, clrType);
        var totalCount = await ContentPolicyQueryableReflection.ExecuteCountAsync(query, clrType, cancellationToken);

        if (filter is null)
        {
            var sampleIds = await ContentPolicyQueryableReflection.ExecuteTakeIdsAsync(
                query,
                clrType,
                sampleSize,
                cancellationToken);
            return new ContentPolicyEntityPreviewData(totalCount, totalCount, sampleIds);
        }

        if (ContentPolicyFilterExpressions.IsDenyAll(filter))
            return new ContentPolicyEntityPreviewData(totalCount, 0, []);

        var parameter = Expression.Parameter(clrType, "entity");
        var filterBody = new ExpressionParameterReplacer(filter.Parameters[0], parameter).Visit(filter.Body)
            ?? throw new InvalidOperationException("Unable to apply content policy filter to preview query.");
        var predicate = Expression.Lambda(filterBody, parameter);
        var filteredQuery = ContentPolicyQueryableReflection.ApplyWhere(query, clrType, predicate);
        var accessibleCount = await ContentPolicyQueryableReflection.ExecuteCountAsync(filteredQuery, clrType, cancellationToken);
        var accessibleIds = accessibleCount == 0
            ? []
            : await ContentPolicyQueryableReflection.ExecuteTakeIdsAsync(filteredQuery, clrType, sampleSize, cancellationToken);

        return new ContentPolicyEntityPreviewData(totalCount, accessibleCount, accessibleIds);
    }
}
