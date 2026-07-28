using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductFeatureTypes;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IProductFeatureTypeRepository
{
    void Add(ProductFeatureType productFeatureType);
    void Remove(ProductFeatureType productFeatureType);
    ValueTask<ProductFeatureType?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<ProductFeatureType?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<ProductFeatureType, bool>> expression, CancellationToken cancellationToken = default);
    Task<PagedResult<ProductFeatureType>> GetAllAsync(GetAllProductFeatureTypeRequestDto request);
    Task<PagedResult<ProductFeatureType>> SearchAsync(SearchProductFeatureTypeRequestDto request);
}
