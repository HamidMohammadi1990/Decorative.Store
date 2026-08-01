using System.Linq.Expressions;
using Store.Domain.Dtos.SubCategories;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface ISubCategoryRepository
{
    void Add(SubCategory subCategory);
    ValueTask<SubCategory?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<SubCategory?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default);
    Task<SubCategory?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    void Remove(SubCategory subCategory);
    Task<SubCategory?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<SubCategory, bool>> expression, CancellationToken cancellationToken = default);
    Task<bool> ExistsCodeAsync(string code, int? excludeSubCategoryId = null, CancellationToken cancellationToken = default);
    Task<bool> ExistsTranslationAsync(
        int languageId,
        string title,
        string slug,
        int? excludeSubCategoryId = null,
        CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllSubCategoryResponseDto>> GetAllAsync(GetAllSubCategoryRequestDto request, CancellationToken cancellationToken = default);
    Task<PagedResult<SearchSubCategoryResponseDto>> SearchAsync(SearchSubCategoryRequestDto request, CancellationToken cancellationToken = default);
}
