using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.CompanyStories;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface ICompanyStoryRepository
{
    void Add(CompanyStory companyStory);
    ValueTask<CompanyStory?> FindAsync(int id, CancellationToken cancellationToken = default);
    ValueTask<CompanyStory?> FindWithItemsAsync(int id, CancellationToken cancellationToken = default);
    Task<CompanyStory?> GetWithItemsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<CompanyStory, bool>> expression, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllCompanyStoryDto>> GetAllAsync(GetAllCompanyStoryRequestDto request);
    Task<PagedResult<SearchCompanyStoryDto>> SearchAsync(SearchCompanyStoryRequestDto request);
}
