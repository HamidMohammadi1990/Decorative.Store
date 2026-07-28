using Store.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace Store.Domain.Enums;

public enum FinancialDocumentType
{
    [Display(Name = "FinancialDocumentType_BankTransaction", ResourceType = typeof(EnumResources))]
    BankTransaction = 1,

    [Display(Name = "FinancialDocumentType_Vat", ResourceType = typeof(EnumResources))]
    Vat = 2,

    [Display(Name = "FinancialDocumentType_WalletTransaction", ResourceType = typeof(EnumResources))]
    WalletTransaction = 3
    // مثلا بانک - کیف پول - بانک و کیف پول - مالیات - عودت مالیات - کمیسیون - عودت کمیسیون - چک و ...
}