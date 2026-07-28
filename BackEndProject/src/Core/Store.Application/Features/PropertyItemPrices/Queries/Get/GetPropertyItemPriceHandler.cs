using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PropertyItemPrices.Queries;

public class GetPropertyItemPriceHandler 
    (IPropertyItemPriceRepository propertyItemPriceRepository, IPropertyItemPriceMapperService mapper)
    : IRequestHandler<GetPropertyItemPriceRequest, OperationResult<GetPropertyItemPriceResponse?>>
{
    public async Task<OperationResult<GetPropertyItemPriceResponse?>> Handle(GetPropertyItemPriceRequest request, CancellationToken cancellationToken)
    {
        var propertyItemPrice = await propertyItemPriceRepository.GetAsNoTrackingAsync(request.Id);
        if (propertyItemPrice is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(propertyItemPrice);
        return result;
    }
}