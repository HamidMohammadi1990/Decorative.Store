using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductQuestions;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IProductQuestionRepository
{
    void Add(ProductQuestion productQuestion);
    ValueTask<ProductQuestion?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllProductQuestionResponseDto>> GetAllAsync(
        GetAllProductQuestionRequestDto request,
        CancellationToken cancellationToken = default);
    Task<PagedResult<SearchProductQuestionResponseDto>> SearchAsync(
        SearchProductQuestionRequestDto request,
        CancellationToken cancellationToken = default);
}
