using Store.Domain.Common;

namespace Store.Domain.Entities;

public class Bank : BaseEntity
{
    public string Title { get; set; } = default!;
    public string Icon { get; set; } = default!;
    public bool IsActive { get; set; } = default!;


    public ICollection<BankAccount> BankAccounts { get; set; } = default!;
    public ICollection<ChequeTransaction> ChequeTransactions { get; set; } = default!;
}