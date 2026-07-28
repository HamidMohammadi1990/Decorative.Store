using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.Banks.Queries;

public record GetAllBankRequest : ContentPolicyRequest<Bank>, IRequest<OperationResult<PagedResult<GetAllBankResponse>>>
{
    public string? Title { get; init; }
    public bool? IsActive { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
