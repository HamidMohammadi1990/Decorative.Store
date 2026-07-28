using System.Linq.Expressions;
using Store.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.CompanyStoryComments;

public record SearchCompanyStoryCommentRequestDto : IContentPolicyQueryDto<CompanyStoryComment>
{
    [QueryFilter(MemberPath = "companyStoryComment.CompanyStoryId")]
    public int CompanyStoryId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<CompanyStoryComment, bool>>? ContentFilter { get; set; }
}
