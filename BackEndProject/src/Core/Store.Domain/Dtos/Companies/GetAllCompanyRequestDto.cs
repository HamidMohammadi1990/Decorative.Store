using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.Companies;

public record GetAllCompanyRequestDto : IContentPolicyQueryDto<Company>
{
    [QueryFilter(MemberPath = "companyProduct.ProductId")]
    public int? ProductId { get; init; }

    [QueryFilter(MemberPath = "province.Id")]
    public int? ProvinceId { get; set; }

    [QueryFilter(MemberPath = "company.CityId")]
    public int? CityId { get; init; }

    [QueryFilter(MemberPath = "company.UserId")]
    public int? UserId { get; set; }

    [QueryFilter(MemberPath = "company.Name", Operator = FilterOperator.Contains)]
    public string? Name { get; init; }

    [QueryFilter(MemberPath = "company.Code")]
    public string? Code { get; init; }

    [QueryFilter(MemberPath = "company.PostalCode")]
    public string? PostalCode { get; init; }

    [QueryFilter(MemberPath = "company.IsActive")]
    public bool? IsActive { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<Company, bool>>? ContentFilter { get; set; }
}
