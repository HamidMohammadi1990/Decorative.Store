using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.DeliveryTypes;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IDeliveryTypeRepository
{
    void Add(DeliveryType deliveryType);
    void Remove(DeliveryType deliveryType);
    ValueTask<DeliveryType?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<DeliveryType?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<List<DeliveryType>> GetAllAsync();
    Task<bool> AnyAsync(Expression<Func<DeliveryType, bool>> expression, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllDeliveryTypeResponseDto>> GetAllAsync(GetAllDeliveryTypeRequestDto request);
    Task<PagedResult<SearchDeliveryTypeResponseDto>> SearchAsync(SearchDeliveryTypeRequestDto request);
}