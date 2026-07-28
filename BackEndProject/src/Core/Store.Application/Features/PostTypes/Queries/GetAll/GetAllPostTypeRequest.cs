using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.PostTypes.Queries;

public record GetAllPostTypeRequest : ContentPolicyRequest<PostType>, IRequest<OperationResult<PagedResult<GetAllPostTypeResponse>>>
{
    public string? Title { get; init; }
    public bool? IsActive { get; init; } = true;
    public PagedRequest Pagination { get; init; } = default!;
}