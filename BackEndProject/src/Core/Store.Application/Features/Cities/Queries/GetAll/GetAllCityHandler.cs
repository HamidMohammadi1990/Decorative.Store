using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Cities.Queries;

public class GetAllCityHandler
    (ICityRepository cityRepository, ICityMapperService mapper)
    : IRequestHandler<GetAllCityRequest, OperationResult<PagedResult<GetAllCityResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllCityResponse>>> Handle(GetAllCityRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var cities = await cityRepository.GetAllAsync(requestModel);
        var result = mapper.Map(cities);
        return result;
    }
}