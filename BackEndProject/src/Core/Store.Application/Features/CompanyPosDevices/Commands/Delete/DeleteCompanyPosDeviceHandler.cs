using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyPosDevices.Commands;

public class DeleteCompanyPosDeviceHandler
    (IUnitOfWork uow, ICompanyPosDeviceRepository companyPosDeviceRepository)
    : IRequestHandler<DeleteCompanyPosDeviceRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteCompanyPosDeviceRequest request, CancellationToken cancellationToken)
    {
        var companyPosDevice = await companyPosDeviceRepository.FindAsync(request.Id, cancellationToken);
        if (companyPosDevice is null)
            return ErrorModel.Create("InvalidId");

        companyPosDevice.DeActive();

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}