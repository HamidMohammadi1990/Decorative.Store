using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.SectionTypes.Queries;

public record GetAllSectionTypeRequest : ContentPolicyRequest<SectionType>, IRequest<OperationResult<PagedResult<GetAllSectionTypeResponse>>>
{
    public string? Name { get; init; }
    public bool? IsActive { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
