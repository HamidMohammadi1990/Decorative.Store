using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductPropertyPrices;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IProductPropertyPriceRepository
{
    ValueTask<ProductPropertyPrice?> FindAsync(int id, CancellationToken cancellationToken = default);
    void Add(ProductPropertyPrice productPropertyPrice);
    Task<ProductPropertyPrice?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<ProductPropertyPrice, bool>> expression, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllProductPropertyPriceDto>> GetAllAsync(GetAllProductPropertyPriceRequestDto request);
}