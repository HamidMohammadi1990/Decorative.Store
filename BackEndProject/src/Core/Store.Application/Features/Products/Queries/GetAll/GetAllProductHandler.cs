using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Products.Queries;

public class GetAllProductHandler
    (IProductRepository productRepository, IProductMapperService mapper)
    : IRequestHandler<GetAllProductRequest, OperationResult<PagedResult<GetAllProductResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllProductResponse>>> Handle(GetAllProductRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var products = await productRepository.GetAllAsync(requestModel, cancellationToken);
        var result = mapper.Map(products);
        return result;
    }
}
