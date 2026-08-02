using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.Wallets;

public record GetAllWalletRequestDto : IContentPolicyQueryDto<Wallet>
{
    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Title { get; init; }

    [QueryFilter]
    public int? UserId { get; init; }

    [QueryFilter]
    public WalletStatusType? Status { get; init; }

    [QueryFilter]
    public bool? IsDefault { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<Wallet, bool>>? ContentFilter { get; set; }
}
