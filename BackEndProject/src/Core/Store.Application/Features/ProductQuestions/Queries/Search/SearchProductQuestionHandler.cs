using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductQuestions.Queries;

public class SearchProductQuestionHandler
    (IProductQuestionRepository productQuestionRepository, IProductQuestionMapperService mapper)
    : IRequestHandler<SearchProductQuestionRequest, OperationResult<PagedResult<SearchProductQuestionResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchProductQuestionResponse>>> Handle(
        SearchProductQuestionRequest request,
        CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var questions = await productQuestionRepository.SearchAsync(requestModel, cancellationToken);
        return mapper.Map(questions);
    }
}
