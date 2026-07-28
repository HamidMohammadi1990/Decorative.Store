using System.Linq.Expressions;
using Store.Domain.Dtos.SubCategories;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface ISubCategoryRepository
{
    void Add(SubCategory subCategory);
    ValueTask<SubCategory?> FindAsync(int id, CancellationToken cancellationToken = default);
    void Remove(SubCategory subCategory);
    Task<SubCategory?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<SubCategory, bool>> expression, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllSubCategoryResponseDto>> GetAllAsync(GetAllSubCategoryRequestDto request);
    Task<PagedResult<SearchSubCategoryResponseDto>> SearchAsync(SearchSubCategoryRequestDto request);
}