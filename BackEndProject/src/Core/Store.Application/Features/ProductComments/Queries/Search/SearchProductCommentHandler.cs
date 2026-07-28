using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductComments.Queries;

public class SearchProductCommentHandler
    (IProductCommentRepository productCommentRepository, IProductCommentMapperService mapper)
    : IRequestHandler<SearchProductCommentRequest, OperationResult<PagedResult<SearchProductCommentResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchProductCommentResponse>>> Handle(SearchProductCommentRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var productComment = await productCommentRepository.SearchAsync(requestModel, cancellationToken);
        var result = mapper.Map(productComment);
        return result;
    }
}
