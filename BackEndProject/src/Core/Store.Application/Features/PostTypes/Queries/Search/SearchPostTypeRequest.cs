using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.PostTypes.Queries;

public record SearchPostTypeRequest : ContentPolicyRequest<PostType>, IRequest<OperationResult<PagedResult<SearchPostTypeResponse>>>
{
    public string? Title { get; init; }    
    public PagedRequest Pagination { get; init; } = default!;
}