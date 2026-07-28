using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.CompanyStories;

public record SearchCompanyStoryRequestDto : IContentPolicyQueryDto<CompanyStory>
{
    [QueryFilter(MemberPath = "companyStory.CompanyId")]
    public int? CompanyId { get; init; }

    [QueryFilter(MemberPath = "companyStory.Caption", Operator = FilterOperator.Contains)]
    public string? Caption { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<CompanyStory, bool>>? ContentFilter { get; set; }
}
