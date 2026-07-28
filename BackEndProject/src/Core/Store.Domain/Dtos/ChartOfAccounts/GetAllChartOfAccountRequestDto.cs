using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Domain.Dtos.ChartOfAccounts;

public record GetAllChartOfAccountRequestDto : IContentPolicyQueryDto<ChartOfAccount>
{
    public int? Level { get; set; }
    public int? ParentId { get; init; }
    public string? AccountCode { get; init; }
    public string? AccountTitle { get; init; }
    public ChartOfAccountType? AccountType { get; init; }
    public ChartOfAccountDetailType? AccountDetailType { get; init; }
    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<ChartOfAccount, bool>>? ContentFilter { get; set; }
}