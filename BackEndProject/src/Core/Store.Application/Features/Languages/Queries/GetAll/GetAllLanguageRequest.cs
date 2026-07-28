using Store.Common.Models;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.Languages.Queries;

public record GetAllLanguageRequest : IRequest<OperationResult<PagedResult<GetAllLanguageResponse>>>
{
    public string? Code { get; init; }
    public string? Name { get; init; }
    public bool? IsActive { get; init; }
    public bool? IsDefault { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
