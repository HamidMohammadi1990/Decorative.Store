using Store.Common.Models;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.Languages.Queries;

public record SearchLanguageRequest : IRequest<OperationResult<PagedResult<SearchLanguageResponse>>>
{
    public string? Code { get; init; }
    public string? Name { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
