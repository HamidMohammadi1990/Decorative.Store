using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductQuestions.Queries;

public class GetAllProductQuestionHandler
    (IProductQuestionRepository productQuestionRepository, IProductQuestionMapperService mapper)
    : IRequestHandler<GetAllProductQuestionRequest, OperationResult<PagedResult<GetAllProductQuestionResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllProductQuestionResponse>>> Handle(
        GetAllProductQuestionRequest request,
        CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var productQuestions = await productQuestionRepository.GetAllAsync(requestModel, cancellationToken);
        return mapper.Map(productQuestions);
    }
}
