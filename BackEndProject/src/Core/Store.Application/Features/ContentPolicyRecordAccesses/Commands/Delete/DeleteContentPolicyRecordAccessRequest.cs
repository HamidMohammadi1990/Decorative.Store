using Store.Common.Models;

namespace Edition.Application.Features.ContentPolicyRecordAccesses.Commands;

public record DeleteContentPolicyRecordAccessRequest : IRequest<OperationResult>
{
    public int Id { get; init; }
}
