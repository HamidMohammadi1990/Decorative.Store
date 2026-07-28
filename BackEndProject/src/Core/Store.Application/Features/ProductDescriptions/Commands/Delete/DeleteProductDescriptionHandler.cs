using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductDescriptions.Commands;

public class DeleteProductDescriptionHandler
    (IUnitOfWork uow, IProductDescriptionRepository productDescriptionRepository)
    : IRequestHandler<DeleteProductDescriptionRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteProductDescriptionRequest request, CancellationToken cancellationToken)
    {
        var productDescription = await productDescriptionRepository.FindAsync(request.Id);
        if (productDescription is null)
            return ErrorModel.Create("InvalidId");

        productDescriptionRepository.Remove(productDescription);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}