using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.AssistantFaqs;

public record GetAllAssistantFaqRequestDto : IContentPolicyQueryDto<AssistantFaq>
{
    [QueryFilter]
    public int? LanguageId { get; init; }

    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Question { get; init; }

    [QueryFilter]
    public bool? IsActive { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<AssistantFaq, bool>>? ContentFilter { get; set; }
}

public record GetAllAssistantFaqResponseDto
{
    public int Id { get; init; }
    public int LanguageId { get; init; }
    public string Question { get; init; } = default!;
    public string Answer { get; init; } = default!;
    public int Priority { get; init; }
    public bool IsActive { get; init; }
}

public record SearchAssistantFaqRequestDto
{
    public int LanguageId { get; init; }
    public bool? IsActive { get; init; } = true;
    public PagedRequest Pagination { get; init; } = default!;
}

public record SearchAssistantFaqResponseDto
{
    public int Id { get; init; }
    public string Question { get; init; } = default!;
    public string Answer { get; init; } = default!;
    public int Priority { get; init; }
    public bool IsActive { get; init; }
}
