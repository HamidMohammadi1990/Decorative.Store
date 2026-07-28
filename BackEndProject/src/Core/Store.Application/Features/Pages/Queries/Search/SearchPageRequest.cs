using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;
using Store.Domain.Entities;

namespace Edition.Application.Features.Pages.Queries;

public record SearchPageRequest : ContentPolicyRequest<Page>, IRequest<OperationResult<PagedResult<SearchPageResponse>>>
{
    public string? Title { get; init; }
    public string? Slug { get; init; }
    public PageType? Type { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
