using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.DeliveryOptions.Commands;

public class CreateDeliveryOptionHandler 
    (IDeliveryOptionRepository deliveryOptionRepository, IUnitOfWork uow)
    : IRequestHandler<CreateDeliveryOptionRequest, OperationResult<CreateDeliveryOptionResponse>>
{
    public async Task<OperationResult<CreateDeliveryOptionResponse>> Handle(CreateDeliveryOptionRequest request, CancellationToken cancellationToken)
    {
        var deliveryOptions = DeliveryOption.Create(request.Title, request.DeliveryDays);
        deliveryOptionRepository.Add(deliveryOptions);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateDeliveryOptionResponse>();

        return new CreateDeliveryOptionResponse { Id = deliveryOptions.Id };
    }
}