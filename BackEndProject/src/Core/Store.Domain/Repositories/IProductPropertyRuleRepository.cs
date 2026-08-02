using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductPropertyRules;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IProductPropertyRuleRepository
{
    void Add(ProductPropertyRule productPropertyRule);
    void Remove(ProductPropertyRule productPropertyRule);
    ValueTask<ProductPropertyRule?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<ProductPropertyRule?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default);
    Task<ProductPropertyRule?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<ProductPropertyRule, bool>> expression, CancellationToken cancellationToken = default);
    Task<PagedResult<ProductPropertyRule>> GetAllAsync(GetAllProductPropertyRuleRequestDto request);
    Task<PagedResult<ProductPropertyRule>> SearchAsync(SearchProductPropertyRuleRequestDto request);
}
