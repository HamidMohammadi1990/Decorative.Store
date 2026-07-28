using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.Tags.Queries;

public record GetAllTagRequest : ContentPolicyRequest<Tag>, IRequest<OperationResult<PagedResult<GetAllTagResponse>>>
{
    public string? Title { get; init; }
    public bool? IsActive { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}