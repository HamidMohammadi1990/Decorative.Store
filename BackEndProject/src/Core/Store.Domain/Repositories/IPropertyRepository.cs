using System.Linq.Expressions;
using Store.Domain.Dtos.Products;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Properties;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IPropertyRepository
{
    void Add(Property property);
    Task<Property?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<Property?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<Property?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllPropertyDto>> GetAllAsync(GetAllPropertyRequestDto request, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<Property, bool>> expression, CancellationToken cancellationToken = default);
    Task<bool> ExistsCodeAsync(string code, int propertyCategoryId, int? excludePropertyId = null, CancellationToken cancellationToken = default);
    ValueTask<Property?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<List<ProductPropertyDto>> GetByProductIdAsync(int productId, CancellationToken cancellationToken = default);
}
