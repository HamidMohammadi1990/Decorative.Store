using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.PropertyItems;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IPropertyItemRepository
{
    void Add(PropertyItem propertyItem);
    ValueTask<PropertyItem?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<PropertyItem?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default);
    Task<PropertyItem?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<PropertyItem?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<PropertyItem, bool>> expression, CancellationToken cancellationToken = default);
    Task<bool> ExistsCodeAsync(string code, int propertyId, int? excludePropertyItemId = null, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllPropertyItemDto>> GetAllAsync(GetAllPropertyItemRequestDto request, CancellationToken cancellationToken = default);
}
