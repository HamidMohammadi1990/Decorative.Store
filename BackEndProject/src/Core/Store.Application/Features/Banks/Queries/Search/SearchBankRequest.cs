using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.Banks.Queries;

public record SearchBankRequest : ContentPolicyRequest<Bank>, IRequest<OperationResult<PagedResult<SearchBankResponse>>>
{
    public string? Title { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
