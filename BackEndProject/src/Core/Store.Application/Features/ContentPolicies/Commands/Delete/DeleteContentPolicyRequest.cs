using Store.Common.Models;

namespace Edition.Application.Features.ContentPolicies.Commands;

public record DeleteContentPolicyRequest : IRequest<OperationResult>
{
    public int Id { get; init; }
}
