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
    Task<PagedResult<GetAllPropertyDto>> GetAllAsync(GetAllPropertyRequestDto request);
    Task<bool> AnyAsync(Expression<Func<Property, bool>> expression, CancellationToken cancellationToken = default);
    ValueTask<Property?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<List<ProductPropertyDto>> GetByProductIdAsync(int productId, int companyId);
}