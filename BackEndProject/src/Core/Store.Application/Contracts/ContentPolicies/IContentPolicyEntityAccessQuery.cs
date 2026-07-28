using System.Linq.Expressions;
using Edition.Domain.Enums;

namespace Edition.Application.Contracts.ContentPolicies;

public interface IContentPolicyEntityAccessQuery
{
    Task<bool> ExistsAsync(
        string entityType,
        int resourceId,
        LambdaExpression? filter,
        CancellationToken cancellationToken = default);
}
