using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPriceDeliveryOptions.Queries;

public class GetProductPriceDeliveryOptionHandler 
    (IProductPriceDeliveryOptionRepository productPriceDeliveryOptionRepository, IProductPriceDeliveryOptionMapperService mapper)
    : IRequestHandler<GetProductPriceDeliveryOptionRequest, OperationResult<GetProductPriceDeliveryOptionResponse>>
{
    public async Task<OperationResult<GetProductPriceDeliveryOptionResponse>> Handle(GetProductPriceDeliveryOptionRequest request, CancellationToken cancellationToken)
    {
        var productPriceDeliveryOption = await productPriceDeliveryOptionRepository.FindAsync(request.Id);
        if (productPriceDeliveryOption is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(productPriceDeliveryOption);
        return result;
    }
}