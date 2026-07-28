using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPriceDeliveryOptions.Queries;

public class GetAllProductPriceDeliveryOptionHandler
    (IProductPriceDeliveryOptionRepository productPriceDeliveryOptionRepository, IProductPriceDeliveryOptionMapperService mapper)
    : IRequestHandler<GetAllProductPriceDeliveryOptionRequest, OperationResult<PagedResult<GetAllProductPriceDeliveryOptionResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllProductPriceDeliveryOptionResponse>>> Handle(GetAllProductPriceDeliveryOptionRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var deliveryOptions = await productPriceDeliveryOptionRepository.GetAllAsync(requestModel);
        var result = mapper.Map(deliveryOptions);
        return result;
    }
}