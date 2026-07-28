using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;
using Store.Domain.Entities;

namespace Edition.Application.Features.Pages.Queries;

public record GetAllPageRequest : ContentPolicyRequest<Page>, IRequest<OperationResult<PagedResult<GetAllPageResponse>>>
{
    public string? Title { get; init; }
    public string? Slug { get; init; }
    public PageType? Type { get; init; }
    public bool? IsActive { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
