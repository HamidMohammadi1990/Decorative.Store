using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class FinancialDocumentDetail : BaseEntity
{
    public AccountPartyType PartyType { get; private set; }
    public int ChartOfAccountId { get; private set; } = default!;
    public int FinancialDocumentId { get; private set; } = default!;
    public decimal Debit { get; private set; } = default!;
    public decimal Credit { get; private set; } = default!;
    public string Description { get; private set; } = default!;


    public ChartOfAccount ChartOfAccount { get; set; } = default!;
    public FinancialDocument FinancialDocument { get; set; } = default!;


    public static FinancialDocumentDetail Create(AccountPartyType partyType, int chartOfAccountId, int financialDocumentId, decimal debit, decimal credit, string description)
        => new()
        {
            Debit = debit,
            Credit = credit,
            Description = description,
            PartyType = partyType,
            ChartOfAccountId = chartOfAccountId,
            FinancialDocumentId = financialDocumentId
        };
}