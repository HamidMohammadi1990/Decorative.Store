using Store.Domain.Enums;
using Store.Domain.Entities;

namespace Edition.Application.Features.Wallets.Common;

public static class WalletFinancialDocumentBuilder
{
    public static FinancialDocument BuildCredit(
        Wallet wallet,
        decimal amount,
        int financialYearId,
        string description)
    {
        var financialDocument = FinancialDocument.CreateForWallet(
            FinancialDocumentType.WalletTransaction,
            description,
            string.Empty,
            financialYearId);

        financialDocument.AddDetail(FinancialDocumentDetail.Create(
            AccountPartyType.Intermediary,
            WalletConstants.IntermediaryChartOfAccountId,
            financialDocument.Id,
            amount,
            0m,
            description));

        financialDocument.AddDetail(FinancialDocumentDetail.Create(
            AccountPartyType.Customer,
            WalletConstants.CustomerChartOfAccountId,
            financialDocument.Id,
            0m,
            amount,
            description));

        return financialDocument;
    }
}
