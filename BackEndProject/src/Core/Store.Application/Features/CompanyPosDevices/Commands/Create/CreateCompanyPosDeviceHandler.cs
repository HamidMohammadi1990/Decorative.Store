using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.CompanyPosDevices.Commands;

public class CreateCompanyPosDeviceHandler
    (IUnitOfWork uow, ICompanyPosDeviceRepository companyPosDeviceRepository)
    : IRequestHandler<CreateCompanyPosDeviceRequest, OperationResult<CreateCompanyPosDeviceResponse>>
{
    async Task<OperationResult<CreateCompanyPosDeviceResponse>> IRequestHandler<CreateCompanyPosDeviceRequest, OperationResult<CreateCompanyPosDeviceResponse>>.Handle(CreateCompanyPosDeviceRequest request, CancellationToken cancellationToken)
    {
        var companyPosDevice = CompanyPosDevice.Create(
            request.Name,
            request.Description,
            request.CompanyId,
            request.BankId,
            request.IP);

        companyPosDeviceRepository.Add(companyPosDevice);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateCompanyPosDeviceResponse>();

        return new CreateCompanyPosDeviceResponse { Id = companyPosDevice.Id };
    }
}