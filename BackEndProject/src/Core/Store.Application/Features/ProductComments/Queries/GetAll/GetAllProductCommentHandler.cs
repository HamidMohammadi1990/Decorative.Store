using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductComments.Queries;

public class GetAllProductCommentHandler
    (IProductCommentRepository productCommentRepository, IProductCommentMapperService mapper)
    : IRequestHandler<GetAllProductCommentRequest, OperationResult<PagedResult<GetAllProductCommentResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllProductCommentResponse>>> Handle(GetAllProductCommentRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var productComment = await productCommentRepository.GetAllAsync(requestModel, cancellationToken);
        var result = mapper.Map(productComment);
        return result;
    }
}
