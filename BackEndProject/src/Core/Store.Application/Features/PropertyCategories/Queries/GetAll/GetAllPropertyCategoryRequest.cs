using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.PropertyCategories.Queries;

public record GetAllPropertyCategoryRequest : ContentPolicyRequest<PropertyCategory>, IRequest<OperationResult<PagedResult<GetAllPropertyCategoryResponse>>>
{
    public string? Title { get; init; }
    public bool? IsActive { get; init; }
}