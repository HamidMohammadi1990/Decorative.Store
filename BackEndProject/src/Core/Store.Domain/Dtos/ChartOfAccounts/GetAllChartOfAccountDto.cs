using Store.Domain.Enums;

namespace Store.Domain.Dtos.ChartOfAccounts;

public record GetAllChartOfAccountDto
{
    public int Id { get; init; } = default!;
    public int Level { get; set; }
    public int? ParentId { get; init; }
    public string AccountCode { get; init; } = default!;
    public string AccountTitle { get; init; } = default!;
    public ChartOfAccountType AccountType { get; init; } = default!;
    public ChartOfAccountDetailType AccountDetailType { get; init; } = default!;
}