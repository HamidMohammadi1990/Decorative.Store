using Edition.Application.Features.Provinces.Queries;
using Store.Domain.Dtos.Provinces;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IProvinceMapperService : IMapper
{
    GetProvinceResponse Map(Province province);
    GetAllProvinceRequestDto Map(GetAllProvinceRequest model);
    SearchProvinceRequestDto Map(SearchProvinceRequest model);
    PagedResult<GetAllProvinceResponse> Map(PagedResult<GetAllProvinceResponseDto> model);
    PagedResult<SearchProvinceResponse> Map(PagedResult<SearchProvinceResponseDto> model);
}