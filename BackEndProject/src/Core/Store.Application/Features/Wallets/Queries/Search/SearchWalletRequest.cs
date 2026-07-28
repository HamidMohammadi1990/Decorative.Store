using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.Wallets.Queries;

public record SearchWalletRequest : ContentPolicyRequest<Wallet>, IRequest<OperationResult<PagedResult<SearchWalletResponse>>>
{
    public string? Title { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
