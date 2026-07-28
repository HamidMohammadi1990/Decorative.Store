using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.CompanyStoryComments;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface ICompanyStoryCommentRepository
{
    Task<PagedResult<GetAllCompanyStoryCommentResponseDto>> GetAllAsync(GetAllCompanyStoryCommentRequestDto request);
    Task<PagedResult<SearchCompanyStoryCommentResponseDto>> SearchAsync(SearchCompanyStoryCommentRequestDto request);
    ValueTask<CompanyStoryComment?> FindAsync(int id, CancellationToken cancellationToken = default);
    void Add(CompanyStoryComment companyStoryComment);
    void Remove(CompanyStoryComment companyStoryComment);
    Task<CompanyStoryComment?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<CompanyStoryComment, bool>> expression, CancellationToken cancellationToken = default);
}
