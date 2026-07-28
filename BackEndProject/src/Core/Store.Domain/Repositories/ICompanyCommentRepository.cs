using System.Linq.Expressions;
using Store.Domain.Dtos.CompanyComments;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface ICompanyCommentRepository
{
    void Add(CompanyComment companyComment);
    ValueTask<CompanyComment?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<CompanyComment?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<CompanyComment, bool>> expression, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllCompanyCommentResponseDto>> GetAllAsync(GetUserCompanyCommentRequestDto request);
    Task<PagedResult<SearchCompanyCommentResponseDto>> SearchAsync(SearchCompanyCommentRequestDto request);
}