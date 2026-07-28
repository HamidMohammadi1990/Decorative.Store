using System.Linq.Expressions;
using Edition.Application.Services.ContentPolicies;
using Edition.Application.Contracts.ContentPolicies;

namespace Edition.Application.Common.Behaviors;

public sealed class ContentPolicyQueryBehavior<TRequest, TResponse>
    (IContentPolicyFilter contentPolicyFilter)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default)
    {
        if (request is IContentPolicyFilteredRequest filteredRequest
            && !filteredRequest.IsContentPolicyResolved)
        {
            var entityType = filteredRequest.EntityClrType.Name;
            var queryAction = filteredRequest.ContentPolicyQueryAction
                ?? ContentPolicyQueryActionResolver.Resolve(request);

            var filter = await contentPolicyFilter.BuildFilterAsync(
                entityType,
                queryAction,
                cancellationToken);

            filteredRequest.ContentPolicyFilter = filter;
            filteredRequest.ContentPolicyDenyAll = filter is not null && IsDenyAll(filter);
            filteredRequest.IsContentPolicyResolved = true;
        }

        return await next();
    }

    private static bool IsDenyAll(LambdaExpression filter)
        => filter.Body is ConstantExpression { Value: false };
}
