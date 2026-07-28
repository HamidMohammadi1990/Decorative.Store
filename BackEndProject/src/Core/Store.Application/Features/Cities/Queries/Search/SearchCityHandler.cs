using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Cities.Queries;

public class SearchCityHandler
    (ICityRepository cityRepository, ICityMapperService mapper)
    : IRequestHandler<SearchCityRequest, OperationResult<PagedResult<SearchCityResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchCityResponse>>> Handle(SearchCityRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var cities = await cityRepository.SearchAsync(requestModel);
        var result = mapper.Map(cities);
        return result;
    }
}