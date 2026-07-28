using System.Linq.Expressions;
using Edition.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.CompanyStoryComments;

public record GetAllCompanyStoryCommentRequestDto : IContentPolicyQueryDto<CompanyStoryComment>
{
    [QueryFilter(MemberPath = "companyStory.Id")]
    public int? CompanyStoryId { get; init; }

    [QueryFilter(MemberPath = "companyStoryComment.CreatedByUserId")]
    public int? CreatedByUserId { get; init; }

    [QueryFilter(MemberPath = "companyStoryComment.ApprovedByUserId")]
    public int? ApprovedByUserId { get; init; }

    [QueryFilter(MemberPath = "companyStoryComment.IsApproved")]
    public bool? IsApproved { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<CompanyStoryComment, bool>>? ContentFilter { get; set; }
}
