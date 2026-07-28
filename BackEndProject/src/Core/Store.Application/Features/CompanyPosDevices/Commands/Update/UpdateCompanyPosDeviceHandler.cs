using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyPosDevices.Commands;

public class UpdateCompanyPosDeviceHandler
    (IUnitOfWork uow, ICompanyPosDeviceRepository companyPosDeviceRepository)
    : IRequestHandler<UpdateCompanyPosDeviceRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateCompanyPosDeviceRequest request, CancellationToken cancellationToken)
    {
        var companyPosDevice = await companyPosDeviceRepository.FindAsync(request.Id, cancellationToken);
        if (companyPosDevice is null)
            return ErrorModel.Create("InvalidId");

        companyPosDevice.Update(
            request.Name,
            request.Description,
            true,
            request.CompanyId,
            request.BankId,
            request.IP);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}