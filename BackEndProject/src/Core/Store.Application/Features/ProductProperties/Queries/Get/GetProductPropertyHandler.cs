using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductProperties.Queries;

public class GetProductPropertyHandler
    (IProductPropertyRepository productPropertyRepository, IProductPropertyMapperService mapper)
    : IRequestHandler<GetProductPropertyRequest, OperationResult<GetProductPropertyResponse?>>
{
    public async Task<OperationResult<GetProductPropertyResponse?>> Handle(GetProductPropertyRequest request, CancellationToken cancellationToken)
    {
        var productProperty = await productPropertyRepository.GetAsNoTrackingAsync(request.Id);
        if (productProperty is null)
            return ErrorModel.Create("InvalidId");

        return mapper.Map(productProperty);
    }
}
