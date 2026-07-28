using Edition.Application.Features.Companies.Queries;
using Store.Domain.Dtos.Companies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface ICompanyMapperService : IMapper
{
    GetCompanyResponse Map(Company model);
    GetAllCompanyRequestDto Map(GetAllCompanyRequest model);
    SearchCompanyRequestDto Map(SearchCompanyRequest model);
    PagedResult<GetAllCompanyResponse> Map(PagedResult<GetAllCompanyResponseDto> model);
    PagedResult<SearchCompanyResponse> Map(PagedResult<SearchCompanyResponseDto> model);
}