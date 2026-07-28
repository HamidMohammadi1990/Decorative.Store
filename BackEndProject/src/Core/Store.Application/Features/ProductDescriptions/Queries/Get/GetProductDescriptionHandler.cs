using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductDescriptions.Queries;

public class GetProductDescriptionHandler
    (IProductDescriptionRepository productDescriptionRepository, IProductDescriptionMapperService mapper)
    : IRequestHandler<GetProductDescriptionRequest, OperationResult<GetProductDescriptionResponse?>>
{
    public async Task<OperationResult<GetProductDescriptionResponse?>> Handle(GetProductDescriptionRequest request, CancellationToken cancellationToken)
    {
        var productDescription = await productDescriptionRepository.GetAsNoTrackingAsync(request.Id);
        if (productDescription is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(productDescription);
        return result;
    }
}