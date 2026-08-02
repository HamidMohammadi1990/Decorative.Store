using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductQuestions;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IProductQuestionRepository
{
    void Add(ProductQuestion productQuestion);
    Task<PagedResult<SearchProductQuestionResponseDto>> SearchAsync(
        SearchProductQuestionRequestDto request,
        CancellationToken cancellationToken = default);
}
