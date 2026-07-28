using Store.Common.Models;

namespace Edition.Application.Features.ContentPolicies.Queries;

public record GetContentPolicyRequest : IRequest<OperationResult<GetContentPolicyResponse?>>
{
    public int Id { get; init; }
}
