using Store.Common.Models;
using Store.Domain.Entities;
using Store.Domain.Dtos.Pagination;
using Edition.Application.Contracts.ContentPolicies;

namespace Edition.Application.Features.PropertyCategories.Queries;

public record GetAllPropertyCategoryRequest : ContentPolicyRequest<PropertyCategory>, IRequest<OperationResult<PagedResult<GetAllPropertyCategoryResponse>>>
{
    public string? Title { get; init; }
    public bool? IsActive { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}