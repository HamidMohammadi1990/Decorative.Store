using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.ProductPriceDeliveryOptions.Commands;

public class CreateProductPriceDeliveryOptionHandler
    (IProductPriceDeliveryOptionRepository productPriceDeliveryOptionRepository, IUnitOfWork uow)
    : IRequestHandler<CreateProductPriceDeliveryOptionRequest, OperationResult<CreateProductPriceDeliveryOptionResponse>>
{
    public async Task<OperationResult<CreateProductPriceDeliveryOptionResponse>> Handle(CreateProductPriceDeliveryOptionRequest request, CancellationToken cancellationToken)
    {
        var productPriceDeliveryOption = ProductPriceDeliveryOption
            .Create(request.ProductPriceId, request.DeliveryOptionId,
                    request.Price, request.CooperationPrice);

        productPriceDeliveryOptionRepository.Add(productPriceDeliveryOption);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateProductPriceDeliveryOptionResponse>();

        return new CreateProductPriceDeliveryOptionResponse { Id = productPriceDeliveryOption.Id };
    }
}