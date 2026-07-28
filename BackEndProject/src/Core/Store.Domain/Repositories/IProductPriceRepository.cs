using System.Linq.Expressions;
using Store.Domain.Dtos.ProductPrices;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IProductPriceRepository
{
    void Add(ProductPrice property);
    ValueTask<ProductPrice?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<ProductPrice?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<ProductPrice, bool>> expression, CancellationToken cancellationToken = default);
    Task<PurchaseProductPriceDto?> GetPriceByProductId(int productId, int companyId);
    Task<PagedResult<GetAllProductPriceDto>> GetAllAsync(GetAllProductPriceRequestDto request);
}