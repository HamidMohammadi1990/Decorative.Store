using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductComments.Queries;

public class GetProductCommentHandler
    (IProductCommentRepository productCommentRepository, IProductCommentMapperService mapper)
    : IRequestHandler<GetProductCommentRequest, OperationResult<GetProductCommentResponse?>>
{
    public async Task<OperationResult<GetProductCommentResponse?>> Handle(GetProductCommentRequest request, CancellationToken cancellationToken)
    {
        var productComment = await productCommentRepository.GetAsNoTrackingAsync(request.Id);
        if (productComment is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(productComment);
        return result;
    }
}