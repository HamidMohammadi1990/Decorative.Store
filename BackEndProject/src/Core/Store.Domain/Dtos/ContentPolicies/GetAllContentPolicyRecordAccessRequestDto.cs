using Store.Domain.Enums;
using Store.Domain.QueryFilters;
using Store.Domain.Dtos.Pagination;

namespace Store.Domain.Dtos.ContentPolicies;

public record GetAllContentPolicyRecordAccessRequestDto
{
    [QueryFilter]
    public int? PolicyId { get; init; }

    [QueryFilter(MemberPath = "Policy.EntityType")]
    public string? EntityType { get; init; }

    [QueryFilter]
    public int? EntityId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;
}
