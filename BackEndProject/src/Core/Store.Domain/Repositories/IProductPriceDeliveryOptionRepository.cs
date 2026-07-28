using System.Linq.Expressions;
using Store.Domain.Dtos.ProductPriceDeliveryOptions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IProductPriceDeliveryOptionRepository
{
    Task<PagedResult<GetAllProductPriceDeliveryOptionResponseDto>> GetAllAsync(GetAllProductPriceDeliveryOptionRequestDto request);
    void Add(ProductPriceDeliveryOption productPriceDeliveryOption);
    ValueTask<ProductPriceDeliveryOption?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<ProductPriceDeliveryOption?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    void Remove(ProductPriceDeliveryOption productPriceDeliveryOption);
    Task<bool> AnyAsync(Expression<Func<ProductPriceDeliveryOption, bool>> expression, CancellationToken cancellationToken = default);
}