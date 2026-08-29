using System.Linq.Expressions;
using Store.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.ProductQuestions;

public record GetAllProductQuestionRequestDto : IContentPolicyQueryDto<ProductQuestion>
{
    [QueryFilter(MemberPath = "productQuestion.ProductId")]
    public int? ProductId { get; init; }

    [QueryFilter(MemberPath = "productQuestion.UserId")]
    public int? UserId { get; init; }

    [QueryFilter(MemberPath = "productQuestion.IsActive")]
    public bool? IsActive { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<ProductQuestion, bool>>? ContentFilter { get; set; }
}
