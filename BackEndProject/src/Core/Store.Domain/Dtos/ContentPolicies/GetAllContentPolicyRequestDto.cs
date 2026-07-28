using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.ContentPolicies;

public record GetAllContentPolicyRequestDto
{
    [QueryFilter]
    public int? RoleId { get; init; }

    [QueryFilter]
    public int? UserId { get; init; }

    [QueryFilter]
    public string? EntityType { get; init; }

    [QueryFilter]
    public ContentPolicyQueryAction? QueryAction { get; init; }

    [QueryFilter]
    public bool? IsActive { get; init; }

    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Name { get; init; }

    public PagedRequest Pagination { get; init; } = default!;
}
