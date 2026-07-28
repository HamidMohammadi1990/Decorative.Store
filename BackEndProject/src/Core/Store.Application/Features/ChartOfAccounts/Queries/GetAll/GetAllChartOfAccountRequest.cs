using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Enums;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.ChartOfAccounts.Queries;

public record GetAllChartOfAccountRequest : ContentPolicyRequest<ChartOfAccount>, IRequest<OperationResult<PagedResult<GetAllChartOfAccountResponse>>>
{
    public int? Level { get; set; }
    public int? ParentId { get; init; }
    public string? AccountCode { get; init; }
    public string? AccountTitle { get; init; }
    public ChartOfAccountType? AccountType { get; init; }
    public ChartOfAccountDetailType? AccountDetailType { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}