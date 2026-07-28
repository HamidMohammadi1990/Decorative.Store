using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductFiles.Commands;

public class UpdateStatusProductFileHandler
     (IUnitOfWork uow, IProductFileRepository productFileRepository)
     : IRequestHandler<UpdateStatusProductFileRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateStatusProductFileRequest request, CancellationToken cancellationToken)
    {
        var productFile = await productFileRepository.FindAsync(request.Id, cancellationToken);
        if (productFile is null)
            return ErrorModel.Create("InvalidId");

        productFile.UpdateStatus(request.Status);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}