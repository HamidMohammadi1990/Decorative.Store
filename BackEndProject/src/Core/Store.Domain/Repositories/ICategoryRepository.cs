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
    Task<Category?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default);
    Task<Category?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<Category?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<Category, bool>> expression, CancellationToken cancellationToken = default);
    Task<bool> ExistsCodeAsync(string code, int? excludeCategoryId = null, CancellationToken cancellationToken = default);
    Task<bool> ExistsTranslationAsync(
        int languageId,
        string title,
        string slug,
        int? excludeCategoryId = null,
        CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllCategoryResponseDto>> GetAllAsync(GetAllCategoryRequestDto request, CancellationToken cancellationToken = default);
    Task<PagedResult<SearchCategoryResponseDto>> SearchAsync(SearchCategoryRequestDto request, CancellationToken cancellationToken = default);
    Task<List<CategoryWithSubCategoriesDto>> GetAllWithProductsAsync(
        ProductFeatureTypeCode featureType,
        CancellationToken cancellationToken = default);
}
