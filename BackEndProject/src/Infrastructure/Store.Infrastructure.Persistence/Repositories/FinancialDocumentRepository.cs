using Store.Domain.Entities;
using Store.Domain.Repositories;
using Store.Infrastructure.Persistence;

namespace Store.Infrastructure.Persistence.Repositories;

public class FinancialDocumentRepository
    (EditionDbContext context)
    : Repository<FinancialDocument>(context), IFinancialDocumentRepository
{

}