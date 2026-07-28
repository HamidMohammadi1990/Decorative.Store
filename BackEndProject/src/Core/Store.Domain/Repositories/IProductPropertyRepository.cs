using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductProperties;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IProductPropertyRepository
{
    void Add(ProductProperty productProperty);
    void Remove(ProductProperty productProperty);
    ValueTask<ProductProperty?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<ProductProperty?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<ProductProperty, bool>> expression, CancellationToken cancellationToken = default);
    Task<PagedResult<ProductProperty>> GetAllAsync(GetAllProductPropertyRequestDto request);
    Task<PagedResult<ProductProperty>> SearchAsync(SearchProductPropertyRequestDto request);
}
