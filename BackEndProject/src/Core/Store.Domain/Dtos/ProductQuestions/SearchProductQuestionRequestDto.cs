using System.Linq.Expressions;
using Store.Domain.QueryFilters;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.ProductQuestions;

public record SearchProductQuestionRequestDto : IContentPolicyQueryDto<ProductQuestion>
{
    [QueryFilter(MemberPath = "question.ProductId")]
    public int? ProductId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<ProductQuestion, bool>>? ContentFilter { get; set; }
}
