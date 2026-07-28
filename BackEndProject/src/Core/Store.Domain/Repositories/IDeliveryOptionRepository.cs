using System.Linq.Expressions;
using Store.Domain.Dtos.DeliveryOptions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IDeliveryOptionRepository
{
    Task<DeliveryOption?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    ValueTask<DeliveryOption?> FindAsync(int id, CancellationToken cancellationToken = default);
    void Add(DeliveryOption deliveryOption);
    Task<bool> AnyAsync(Expression<Func<DeliveryOption, bool>> expression, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllDeliveryOptionResponseDto>> GetAllAsync(GetAllDeliveryOptionRequestDto request);
    Task<PagedResult<SearchDeliveryOptionResponseDto>> SearchAsync(SearchDeliveryOptionRequestDto request);    
}