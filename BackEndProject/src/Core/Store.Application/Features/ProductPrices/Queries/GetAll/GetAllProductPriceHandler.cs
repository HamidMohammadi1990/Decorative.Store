using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPrices.Queries;

public class GetAllProductPriceHandler
    (IProductPriceRepository productPriceRepository, IProductPriceMapperService mapper)
    : IRequestHandler<GetAllProductPriceRequest, OperationResult<PagedResult<GetAllProductPriceResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllProductPriceResponse>>> Handle(GetAllProductPriceRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var productPrices = await productPriceRepository.GetAllAsync(requestModel);
        var result = mapper.Map(productPrices);
        return result;
    }
}
