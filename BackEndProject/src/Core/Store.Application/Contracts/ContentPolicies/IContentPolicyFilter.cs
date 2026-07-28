using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Enums;

namespace Edition.Application.Contracts.ContentPolicies;

public interface IContentPolicyFilter
{
    Task<LambdaExpression?> BuildFilterAsync(
        string entityType,
        ContentPolicyQueryAction queryAction,
        CancellationToken cancellationToken = default);

    Task<LambdaExpression?> BuildFilterForUserAsync(
        int userId,
        string entityType,
        ContentPolicyQueryAction queryAction,
        CancellationToken cancellationToken = default);
}

public interface IContentPolicyEntityPreviewQuery
{
    Task<ContentPolicyEntityPreviewData> PreviewAsync(
        string entityType,
        LambdaExpression? filter,
        int sampleSize,
        CancellationToken cancellationToken = default);
}
