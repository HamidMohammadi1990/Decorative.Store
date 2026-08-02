using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;
using Store.Domain.Entities;

namespace Edition.Application.Features.Wallets.Queries;

public record GetAllWalletRequest : ContentPolicyRequest<Wallet>, IRequest<OperationResult<PagedResult<GetAllWalletResponse>>>
{
    public string? Title { get; init; }
    public int? UserId { get; init; }
    public WalletStatusType? Status { get; init; }
    public bool? IsDefault { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
