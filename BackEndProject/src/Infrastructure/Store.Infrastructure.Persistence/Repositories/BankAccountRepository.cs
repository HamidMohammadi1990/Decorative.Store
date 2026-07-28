using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using Store.Domain.Repositories;
using Store.Infrastructure.Persistence;

namespace Store.Infrastructure.Persistence.Repositories;

public class BankAccountRepository
    (EditionDbContext context)
    : Repository<BankAccount>(context), IBankAccountRepository
{
    public Task<BankAccount?> GetActiveByIdAsync(int bankAccountId, CancellationToken cancellationToken = default)
    {
        return Context.BankAccount
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == bankAccountId && x.IsActive, cancellationToken);
    }
}
