using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.PropertyItemPrices;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IPropertyItemPriceRepository
{
    void Add(PropertyItemPrice propertyItemPrice);
    ValueTask<PropertyItemPrice?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<PropertyItemPrice?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<PropertyItemPrice, bool>> expression, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllPropertyItemPriceDto>> GetAllAsync(GetAllPropertyItemPriceRequestDto request);
}