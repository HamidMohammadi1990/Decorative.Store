using Store.Domain.Dtos.Pagination;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.Languages;

public record SearchLanguageRequestDto
{
    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Code { get; init; }

    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Name { get; init; }

    public PagedRequest Pagination { get; init; } = default!;
}
