using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.SubCategories;

public record SearchSubCategoryRequestDto : IContentPolicyQueryDto<SubCategory>
{
    public string? Title { get; init; }
    public string? Slug { get; init; }
    public string? Code { get; init; }
    public int? CategoryId { get; init; }
    public string? CategoryTitle { get; init; }
    public string? CategoryCode { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<SubCategory, bool>>? ContentFilter { get; set; }
}
