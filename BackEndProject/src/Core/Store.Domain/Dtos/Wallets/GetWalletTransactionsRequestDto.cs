using System.Linq.Expressions;
using Edition.Domain.QueryFilters;
using Store.Domain.Enums;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.Wallets;

public record GetWalletTransactionsRequestDto : IContentPolicyQueryDto<WalletTransaction>
{
    [QueryFilter]
    public int? WalletId { get; init; }

    [QueryFilter]
    public int? UserId { get; init; }

    [QueryFilter]
    public WalletTransactionType? Type { get; init; }

    [QueryFilter]
    public WalletTransactionStatusType? Status { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<WalletTransaction, bool>>? ContentFilter { get; set; }
}