using System.Linq.Expressions;
using Store.Domain.Dtos.Categories;
using Store.Domain.Enums;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface ICategoryRepository
{
    void Add(Category category);
    void Remove(Category category);
    ValueTask<Category?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<Category?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<Category, bool>> expression, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllCategoryResponseDto>> GetAllAsync(GetAllCategoryRequestDto request);
    Task<PagedResult<SearchCategoryResponseDto>> SearchAsync(SearchCategoryRequestDto request);
    Task<List<CategoryWithSubCategoriesDto>> GetAllWithProductsAsync(ProductFeatureTypeCode featureType);
}