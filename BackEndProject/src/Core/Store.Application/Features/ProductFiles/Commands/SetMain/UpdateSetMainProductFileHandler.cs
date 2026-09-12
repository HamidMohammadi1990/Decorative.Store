using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Enums;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductFiles.Commands;

public class UpdateSetMainProductFileHandler
    (IUnitOfWork uow, IProductFileRepository productFileRepository)
    : IRequestHandler<UpdateSetMainProductFileRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateSetMainProductFileRequest request, CancellationToken cancellationToken)
    {
        var productFile = await productFileRepository.FindAsync(request.Id, cancellationToken);
        if (productFile is null)
            return ErrorModel.Create("InvalidId");

        if (productFile.FileTypeId != ProductFileKind.Gallery)
            return ErrorModel.Create("InvalidProductFileKind");

        await productFileRepository.ClearMainFlagsAsync(productFile.ProductId, cancellationToken);
        productFile.SetMain(true);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
