using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyPosDevices.Queries;

public class GetCompanyPosDeviceHandler
    (ICompanyPosDeviceRepository companyPosDeviceRepository, ICompanyPosDeviceMapperService mapper)
    : IRequestHandler<GetCompanyPosDeviceRequest, OperationResult<GetCompanyPosDeviceResponse?>>
{
    public async Task<OperationResult<GetCompanyPosDeviceResponse?>> Handle(GetCompanyPosDeviceRequest request, CancellationToken cancellationToken)
    {
        var companyPosDevice = await companyPosDeviceRepository.GetAsNoTrackingAsync(request.Id);
        if (companyPosDevice is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(companyPosDevice);
        return result;
    }
}