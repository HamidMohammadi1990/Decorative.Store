using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPriceDeliveryOptions.Commands;

public class DeleteProductPriceDeliveryOptionHandler
    (IProductPriceDeliveryOptionRepository productPriceDeliveryOptionRepository, IUnitOfWork uow)
    : IRequestHandler<DeleteProductPriceDeliveryOptionRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteProductPriceDeliveryOptionRequest request, CancellationToken cancellationToken)
    {
        var productPriceDeliveryOption = await productPriceDeliveryOptionRepository.FindAsync(request.Id);
        if (productPriceDeliveryOption is null)
            return ErrorModel.Create("InvalidId");

        productPriceDeliveryOptionRepository.Remove(productPriceDeliveryOption);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}