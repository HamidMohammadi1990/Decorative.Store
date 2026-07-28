using Edition.Domain.Enums;
using Store.Common.Models;

namespace Edition.Application.Features.ContentPolicyRecordAccesses.Commands;

public record CreateContentPolicyRecordAccessRequest : IRequest<OperationResult<CreateContentPolicyRecordAccessResponse>>
{
    public int PolicyId { get; init; }
    public int EntityId { get; init; }
}

public record CreateContentPolicyRecordAccessResponse
{
    public int Id { get; init; }
}
