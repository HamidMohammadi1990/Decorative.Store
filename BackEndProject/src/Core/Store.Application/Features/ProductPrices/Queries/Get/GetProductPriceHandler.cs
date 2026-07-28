using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPrices.Queries;

public class GetProductPriceHandler 
    (IProductPriceRepository productPriceRepository, IProductPriceMapperService mapper)
    : IRequestHandler<GetProductPriceRequest, OperationResult<GetProductPriceResponse?>>
{
    public async Task<OperationResult<GetProductPriceResponse?>> Handle(GetProductPriceRequest request, CancellationToken cancellationToken)
    {
        var productPrice = await productPriceRepository.GetAsNoTrackingAsync(request.Id);
        if (productPrice is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(productPrice);
        return result;
    }
}