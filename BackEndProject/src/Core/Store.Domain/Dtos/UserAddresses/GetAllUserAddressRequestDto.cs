using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.UserAddresses;

public record GetAllUserAddressRequestDto : IContentPolicyQueryDto<UserAddress>
{
    [QueryFilter(MemberPath = "address.IsActive")]
    public bool? IsActive { get; init; }

    [QueryFilter(MemberPath = "address.Title", Operator = FilterOperator.Contains)]
    public string? Title { get; init; }

    [QueryFilter(MemberPath = "address.CityId")]
    public int? CityId { get; init; }

    [QueryFilter(MemberPath = "address.UserId")]
    public int? UserId { get; init; }

    [QueryFilter(MemberPath = "address.PostalCode", Operator = FilterOperator.Contains)]
    public string? PostalCode { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<UserAddress, bool>>? ContentFilter { get; set; }
}
