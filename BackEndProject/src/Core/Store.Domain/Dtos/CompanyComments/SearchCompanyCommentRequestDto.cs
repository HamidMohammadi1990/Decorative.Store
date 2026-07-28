using System.Linq.Expressions;
using Store.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.CompanyComments;

public record SearchCompanyCommentRequestDto : IContentPolicyQueryDto<CompanyComment>
{
    [QueryFilter(MemberPath = "companyComment.CompanyId")]
    public int? CompanyId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<CompanyComment, bool>>? ContentFilter { get; set; }
}
