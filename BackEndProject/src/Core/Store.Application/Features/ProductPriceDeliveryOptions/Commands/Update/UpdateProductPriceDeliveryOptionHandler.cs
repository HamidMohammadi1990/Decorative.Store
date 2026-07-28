using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPriceDeliveryOptions.Commands;

public class UpdateProductPriceDeliveryOptionHandler 
    (IProductPriceDeliveryOptionRepository productPriceDeliveryOptionRepository, IUnitOfWork uow)
    : IRequestHandler<UpdateProductPriceDeliveryOptionRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateProductPriceDeliveryOptionRequest request, CancellationToken cancellationToken)
    {
        var productPriceDeliveryOption = await productPriceDeliveryOptionRepository.FindAsync(request.Id);
        if (productPriceDeliveryOption is null)
            return ErrorModel.Create("InvalidId");

        productPriceDeliveryOption.Update(request.ProductPriceId, request.DeliveryOptionId, request.Price, request.CooperationPrice);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}