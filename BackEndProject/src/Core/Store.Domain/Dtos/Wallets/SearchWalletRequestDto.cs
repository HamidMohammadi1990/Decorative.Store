using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.Wallets;

public record SearchWalletRequestDto : IContentPolicyQueryDto<Wallet>
{
    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Title { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<Wallet, bool>>? ContentFilter { get; set; }
}