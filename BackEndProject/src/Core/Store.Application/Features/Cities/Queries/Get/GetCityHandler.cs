using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Cities.Queries;

public class GetCityHandler
    (ICityRepository cityRepository, ICityMapperService mapper)
    : IRequestHandler<GetCityRequest, OperationResult<GetCityResponse>>
{
    public async Task<OperationResult<GetCityResponse>> Handle(GetCityRequest request, CancellationToken cancellationToken)
    {
        var city = await cityRepository.GetAsNoTrackingAsync(request.Id);
        if (city is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(city);
        return result;
    }
}