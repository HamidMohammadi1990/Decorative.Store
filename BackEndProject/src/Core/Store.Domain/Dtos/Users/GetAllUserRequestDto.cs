using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.Users;

public record GetAllUserRequestDto : IContentPolicyQueryDto<User>
{
    [QueryFilter(MemberPath = "user.UserName", Operator = FilterOperator.Contains)]
    public string? UserName { get; init; }

    [QueryFilter(MemberPath = "user.FirstName", Operator = FilterOperator.Contains)]
    public string? FirstName { get; init; }

    [QueryFilter(MemberPath = "user.LastName", Operator = FilterOperator.Contains)]
    public string? LastName { get; init; }

    public string? Email { get; init; }

    [QueryFilter(MemberPath = "user.EconomicCode")]
    public string? EconomicCode { get; init; }

    [QueryFilter(MemberPath = "user.PhoneNumber")]
    public string? PhoneNumber { get; init; }

    [QueryFilter(MemberPath = "user.EmailConfirmed")]
    public bool? EmailConfirmed { get; init; }

    [QueryFilter(MemberPath = "user.PhoneNumberConfirmed")]
    public bool? PhoneNumberConfirmed { get; init; }

    [QueryFilter(MemberPath = "user.LoginPermission")]
    public bool? LoginPermission { get; init; }

    [QueryFilter(MemberPath = "user.Gender")]
    public GenderType? Gender { get; init; }

    [QueryFilter(MemberPath = "user.IsActive")]
    public bool? IsActive { get; init; }

    [QueryFilter(MemberPath = "user.RefundMethod")]
    public RefundMethodType? RefundMethod { get; init; }

    [QueryFilter(MemberPath = "user.CityId")]
    public int? CityId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<User, bool>>? ContentFilter { get; set; }
}
