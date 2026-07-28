using Edition.Application.Features.Cities.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Cities;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface ICityMapperService : IMapper
{
    GetCityResponse Map(City model);
    GetAllCityRequestDto Map(GetAllCityRequest model);
    SearchCityRequestDto Map(SearchCityRequest model);
    PagedResult<GetAllCityResponse> Map(PagedResult<GetAllCityResponseDto> model);
    PagedResult<SearchCityResponse> Map(PagedResult<SearchCityResponseDto> model);
}