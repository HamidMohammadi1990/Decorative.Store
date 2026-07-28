using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.ContentPolicyRecordAccesses.Queries;

public record GetContentPolicyRecordAccessRequest : IRequest<OperationResult<GetContentPolicyRecordAccessResponse?>>
{
    public int Id { get; init; }
}

public record GetContentPolicyRecordAccessResponse
{
    public int Id { get; init; }
    public int PolicyId { get; init; }
    public int EntityId { get; init; }
    public string EntityType { get; init; }
    public string PolicyName { get; init; } = default!;
    public ContentPolicyEffect PolicyEffect { get; init; }
}
