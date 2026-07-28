using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.Categories.Queries;

public record SearchCategoryRequest : ContentPolicyRequest<Category>, IRequest<OperationResult<PagedResult<SearchCategoryResponse>>>
{
    public string? Title { get; init; }
    public string? Slug { get; init; }
    public string? Code { get; init; }
    public bool? IsActive { get; init; } = true;
    public PagedRequest Pagination { get; init; } = default!;
}