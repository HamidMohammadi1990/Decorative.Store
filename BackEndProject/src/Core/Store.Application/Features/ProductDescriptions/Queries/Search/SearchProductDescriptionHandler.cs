using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.ProductDescriptions.Queries;

public class SearchProductDescriptionHandler
    (IProductDescriptionRepository productDescriptionRepository, IProductDescriptionMapperService mapper)
    : IRequestHandler<SearchProductDescriptionRequest, OperationResult<PagedResult<SearchProductDescriptionResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchProductDescriptionResponse>>> Handle(SearchProductDescriptionRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var descriptions = await productDescriptionRepository.SearchAsync(requestModel);
        var result = mapper.Map(descriptions);
        return result;
    }
}