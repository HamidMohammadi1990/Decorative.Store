using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductDescriptions.Commands;

public class UpdateProductDescriptionHandler
    (IUnitOfWork uow, IProductDescriptionRepository productDescriptionRepository)
    : IRequestHandler<UpdateProductDescriptionRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateProductDescriptionRequest request, CancellationToken cancellationToken)
    {
        var productDescription = await productDescriptionRepository.FindAsync(request.Id, cancellationToken);
        if (productDescription is null)
            return ErrorModel.Create("InvalidId");

        productDescription.Update(request.Description, request.LanguageId);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
