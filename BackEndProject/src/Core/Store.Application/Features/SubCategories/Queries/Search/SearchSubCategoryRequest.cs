using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.SubCategories.Queries;

public record SearchSubCategoryRequest : ContentPolicyRequest<SubCategory>, IRequest<OperationResult<PagedResult<SearchSubCategoryResponse>>>
{
    public string? Title { get; init; }
    public string? Slug { get; init; }
    public string? Code { get; init; }
    public int? CategoryId { get; init; }
    public string? CategoryTitle { get; init; }
    public string? CategoryCode { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}