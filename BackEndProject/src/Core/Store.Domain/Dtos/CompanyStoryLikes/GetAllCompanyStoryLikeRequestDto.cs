using System.Linq.Expressions;
using Edition.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.CompanyStoryLikes;

public record GetAllCompanyStoryLikeRequestDto : IContentPolicyQueryDto<CompanyStoryLike>
{
    [QueryFilter(MemberPath = "companyStoryLike.CompanyStoryId")]
    public int? CompanyStoryId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<CompanyStoryLike, bool>>? ContentFilter { get; set; }
}
