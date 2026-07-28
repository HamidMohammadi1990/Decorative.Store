using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.SubCategories;

public record GetAllSubCategoryRequestDto : IContentPolicyQueryDto<SubCategory>
{
    [QueryFilter(MemberPath = "subCategory.Title", Operator = FilterOperator.Contains)]
    public string? Title { get; init; }

    [QueryFilter(MemberPath = "subCategory.Slug")]
    public string? Slug { get; init; }

    [QueryFilter(MemberPath = "subCategory.Code")]
    public string? Code { get; init; }

    [QueryFilter(MemberPath = "subCategory.CategoryId")]
    public int? CategoryId { get; init; }

    [QueryFilter(MemberPath = "category.Title", Operator = FilterOperator.Contains)]
    public string? CategoryTitle { get; init; }

    [QueryFilter(MemberPath = "category.Code", Operator = FilterOperator.Contains)]
    public string? CategoryCode { get; init; }

    [QueryFilter(MemberPath = "subCategory.IsActive")]
    public bool? IsActive { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<SubCategory, bool>>? ContentFilter { get; set; }
}
