using System.Linq.Expressions;
using Store.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Domain.Dtos.CompanyComments;

public record GetUserCompanyCommentRequestDto : IContentPolicyQueryDto<CompanyComment>
{
    [QueryFilter(MemberPath = "companyComment.UserId")]
    public int? UserId { get; init; }

    [QueryFilter(MemberPath = "companyComment.CompanyId")]
    public int? CompanyId { get; init; }

    [QueryFilter(MemberPath = "companyComment.StatusType")]
    public CommentStatusType? Status { get; set; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<CompanyComment, bool>>? ContentFilter { get; set; }
}
