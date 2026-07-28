using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Edition.Application.Features.ContentPolicies.Queries;

public record GetAllContentPolicyRequest : IRequest<OperationResult<PagedResult<GetAllContentPolicyResponse>>>
{
    public int? RoleId { get; init; }
    public int? UserId { get; init; }
    public string? EntityType { get; init; }
    public ContentPolicyQueryAction? QueryAction { get; init; }
    public bool? IsActive { get; init; }
    public string? Name { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
