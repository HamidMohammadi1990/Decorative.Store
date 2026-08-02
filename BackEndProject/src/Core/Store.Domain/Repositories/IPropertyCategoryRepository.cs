using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.PropertyCategories;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IPropertyCategoryRepository
{
    void Add(PropertyCategory propertyCategory);
    ValueTask<PropertyCategory?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<PropertyCategory?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default);
    Task<PropertyCategory?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<PropertyCategory?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllPropertyCategoryDto>> GetAllAsync(GetAllPropertyCategoryRequestDto request, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<PropertyCategory, bool>> expression, CancellationToken cancellationToken = default);
    Task<bool> ExistsCodeAsync(string code, int? excludePropertyCategoryId = null, CancellationToken cancellationToken = default);
}
