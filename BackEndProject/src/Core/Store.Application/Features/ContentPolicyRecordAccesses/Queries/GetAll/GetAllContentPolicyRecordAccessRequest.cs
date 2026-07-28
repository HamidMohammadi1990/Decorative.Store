using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Edition.Application.Features.ContentPolicyRecordAccesses.Queries;

public record GetAllContentPolicyRecordAccessRequest : IRequest<OperationResult<PagedResult<GetAllContentPolicyRecordAccessResponse>>>
{
    public int? PolicyId { get; init; }
    public string? EntityType { get; init; }
    public int? EntityId { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}

public record GetAllContentPolicyRecordAccessResponse
{
    public int Id { get; init; }
    public int PolicyId { get; init; }
    public int EntityId { get; init; }
    public string EntityType { get; init; }
    public string PolicyName { get; init; } = default!;
    public ContentPolicyEffect PolicyEffect { get; init; }
}
