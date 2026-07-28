using System.Linq.Expressions;
using Edition.Common.Exceptions;
using Edition.Application.Contracts.ContentPolicies;
using Store.Domain.Enums;

namespace Edition.Application.Services.ContentPolicies;

public sealed class ContentPolicyAccessChecker
    (IContentPolicyFilter contentPolicyFilter, IContentPolicyEntityAccessQuery entityAccessQuery)
    : IContentPolicyAccessChecker
{
    public async Task EnsureAccessibleAsync(
        string entityType,
        int resourceId,
        CancellationToken cancellationToken = default)
    {
        if (resourceId <= 0)
            throw new AppContentPolicyAccessDeniedException();

        var filter = await contentPolicyFilter.BuildFilterAsync(
            entityType,
            ContentPolicyQueryAction.Get,
            cancellationToken);
        if (filter is null)
            return;

        if (IsDenyAll(filter))
            throw new AppContentPolicyAccessDeniedException();

        var exists = await entityAccessQuery.ExistsAsync(entityType, resourceId, filter, cancellationToken);
        if (!exists)
            throw new AppContentPolicyAccessDeniedException();
    }

    private static bool IsDenyAll(LambdaExpression filter)
        => ContentPolicyFilter.IsDenyAll(filter);
}
