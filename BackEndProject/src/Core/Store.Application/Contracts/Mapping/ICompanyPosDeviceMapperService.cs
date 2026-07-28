using Edition.Application.Features.CompanyPosDevices.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.CompanyPosDevices;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface ICompanyPosDeviceMapperService : IMapper
{
    GetCompanyPosDeviceResponse Map(CompanyPosDevice model);
    GetAllCompanyPosDeviceRequestDto Map(GetAllCompanyPosDeviceRequest model);
    PagedResult<GetAllCompanyPosDeviceResponse> Map(PagedResult<GetAllCompanyPosDeviceDto> model);
}