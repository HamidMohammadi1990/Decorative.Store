using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IBankAccountRepository
{
    Task<BankAccount?> GetActiveByIdAsync(int bankAccountId, CancellationToken cancellationToken = default);
}
