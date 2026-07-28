using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPropertyPrices.Queries;

public class GetProductPropertyPriceHandler
    (IProductPropertyPriceRepository productPropertyPriceRepository, IProductPropertyPriceMapperService mapper)
    : IRequestHandler<GetProductPropertyPriceRequest, OperationResult<GetProductPropertyPriceResponse?>>
{
    public async Task<OperationResult<GetProductPropertyPriceResponse?>> Handle(GetProductPropertyPriceRequest request, CancellationToken cancellationToken)
    {
        var productPropertyPrice = await productPropertyPriceRepository.GetAsNoTrackingAsync(request.Id);
        if (productPropertyPrice is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(productPropertyPrice);
        return result;
    }
}