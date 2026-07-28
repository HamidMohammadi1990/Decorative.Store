using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Products.Queries;

public class GetProductHandler
     (IProductRepository productRepository, IProductMapperService mapper)
     : IRequestHandler<GetProductRequest, OperationResult<GetProductResponse?>>
{
    public async Task<OperationResult<GetProductResponse?>> Handle(GetProductRequest request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetAsNoTrackingAsync(request.Id);
        if (product is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(product);
        return result;
    }
}