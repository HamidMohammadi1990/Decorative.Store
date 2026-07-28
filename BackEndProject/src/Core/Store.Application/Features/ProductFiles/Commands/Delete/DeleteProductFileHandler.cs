using Edition.Application.Common.Directories;
using Edition.Application.Common.Utilities.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductFiles.Commands;

public class DeleteProductFileHandler
     (IUnitOfWork uow, IProductFileRepository productFileRepository, ILocalFileService localFileService)
    : IRequestHandler<DeleteProductFileRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteProductFileRequest request, CancellationToken cancellationToken)
    {
        var productFile = await productFileRepository.FindAsync(request.Id, cancellationToken);
        if (productFile is null)
            return ErrorModel.Create("InvalidId");
        
        productFileRepository.Remove(productFile);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        localFileService.DeleteFile(ProductDirectory.ProductImage, productFile.FileName);
        return OperationResult.Success();
    }
}