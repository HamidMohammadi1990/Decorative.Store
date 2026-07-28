using Edition.Domain.Entities;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.ProductProperties.Queries;

public class SearchProductPropertyHandler
    (IProductPropertyRepository productPropertyRepository, IProductPropertyMapperService mapper)
    : IRequestHandler<SearchProductPropertyRequest, OperationResult<PagedResult<SearchProductPropertyResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchProductPropertyResponse>>> Handle(SearchProductPropertyRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var items = await productPropertyRepository.SearchAsync(requestModel);
        return mapper.MapToSearch(items);
    }
}
