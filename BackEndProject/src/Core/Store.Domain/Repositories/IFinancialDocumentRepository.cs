using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IFinancialDocumentRepository
{
    void Add(FinancialDocument financialDocument);
}