using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPropertyPrices.Queries;

public class GetAllProductPropertyPriceHandler
    (IProductPropertyPriceRepository productPropertyPriceRepository, IProductPropertyPriceMapperService mapper)
    : IRequestHandler<GetAllProductPropertyPriceRequest, OperationResult<PagedResult<GetAllProductPropertyPriceResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllProductPropertyPriceResponse>>> Handle(GetAllProductPropertyPriceRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var productPropertyPrices = await productPropertyPriceRepository.GetAllAsync(requestModel);
        var result = mapper.Map(productPropertyPrices);
        return result;
    }
}