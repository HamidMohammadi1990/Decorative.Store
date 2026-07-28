using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.SectionTypes.Queries;

public record SearchSectionTypeRequest : ContentPolicyRequest<SectionType>, IRequest<OperationResult<PagedResult<SearchSectionTypeResponse>>>
{
    public string? Name { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
