using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductFeatureTypes.Queries;

public class GetProductFeatureTypeHandler
    (IProductFeatureTypeRepository productFeatureTypeRepository, IProductFeatureTypeMapperService mapper)
    : IRequestHandler<GetProductFeatureTypeRequest, OperationResult<GetProductFeatureTypeResponse?>>
{
    public async Task<OperationResult<GetProductFeatureTypeResponse?>> Handle(GetProductFeatureTypeRequest request, CancellationToken cancellationToken)
    {
        var productFeatureType = await productFeatureTypeRepository.GetAsNoTrackingAsync(request.Id);
        if (productFeatureType is null)
            return ErrorModel.Create("InvalidId");

        return mapper.Map(productFeatureType);
    }
}
