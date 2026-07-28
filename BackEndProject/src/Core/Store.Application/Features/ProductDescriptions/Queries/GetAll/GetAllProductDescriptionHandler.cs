using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.ProductDescriptions.Queries;

public class GetAllProductDescriptionHandler
    (IProductDescriptionRepository productDescriptionRepository, IProductDescriptionMapperService mapper)
    : IRequestHandler<GetAllProductDescriptionRequest, OperationResult<PagedResult<GetAllProductDescriptionResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllProductDescriptionResponse>>> Handle(GetAllProductDescriptionRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var descriptions = await productDescriptionRepository.GetAllAsync(requestModel);
        var result = mapper.Map(descriptions);
        return result;
    }
}