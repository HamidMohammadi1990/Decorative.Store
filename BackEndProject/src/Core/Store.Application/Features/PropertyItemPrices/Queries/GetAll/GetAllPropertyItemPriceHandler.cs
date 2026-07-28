using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.PropertyItemPrices.Queries;

public class GetAllPropertyItemPriceHandler
    (IPropertyItemPriceRepository propertyItemPriceRepository, IPropertyItemPriceMapperService mapper)
    : IRequestHandler<GetAllPropertyItemPriceRequest, OperationResult<PagedResult<GetAllPropertyItemPriceResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllPropertyItemPriceResponse>>> Handle(GetAllPropertyItemPriceRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var itemPrices = await propertyItemPriceRepository.GetAllAsync(requestModel);
        var result = mapper.Map(itemPrices);
        return result;
    }
}