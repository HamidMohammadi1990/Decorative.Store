using Store.Common.Models;

namespace Edition.Application.Features.ContentPolicyRules.Commands;

public record DeleteContentPolicyRuleRequest : IRequest<OperationResult>
{
    public int Id { get; init; }
}
