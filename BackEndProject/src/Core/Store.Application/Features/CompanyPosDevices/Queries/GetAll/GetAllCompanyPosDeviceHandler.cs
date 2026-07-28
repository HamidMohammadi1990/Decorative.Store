using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyPosDevices.Queries;

public class GetAllCompanyPosDeviceHandler 
    (ICompanyPosDeviceRepository companyPosDeviceRepository, ICompanyPosDeviceMapperService mapper)
    : IRequestHandler<GetAllCompanyPosDeviceRequest, OperationResult<PagedResult<GetAllCompanyPosDeviceResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllCompanyPosDeviceResponse>>> Handle(GetAllCompanyPosDeviceRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var devices = await companyPosDeviceRepository.GetAllAsync(requestModel);
        var result = mapper.Map(devices);
        return result;
    }
}