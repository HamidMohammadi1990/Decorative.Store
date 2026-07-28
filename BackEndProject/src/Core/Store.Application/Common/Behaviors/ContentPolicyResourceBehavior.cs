using Edition.Application.Contracts.ContentPolicies;

namespace Edition.Application.Common.Behaviors;

public sealed class ContentPolicyResourceBehavior<TRequest, TResponse>
    (IContentPolicyAccessChecker contentPolicyAccessChecker, IContentPolicyResourceRequestResolver resourceRequestResolver)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default)
    {
        if (resourceRequestResolver.TryResolve(request, out var entityType, out var resourceId))
            await contentPolicyAccessChecker.EnsureAccessibleAsync(entityType, resourceId, cancellationToken);

        return await next();
    }
}