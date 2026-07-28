using System.Linq.Expressions;
using Store.Domain.Dtos.CompanyStoryLikes;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface ICompanyStoryLikeRepository
{
    void Add(CompanyStoryLike companyStoryLike);
    Task<CompanyStoryLike?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<CompanyStoryLikeDto>> GetAllAsync(GetAllCompanyStoryLikeRequestDto request);
    Task<bool> AnyAsync(Expression<Func<CompanyStoryLike, bool>> expression, CancellationToken cancellationToken = default);
}
