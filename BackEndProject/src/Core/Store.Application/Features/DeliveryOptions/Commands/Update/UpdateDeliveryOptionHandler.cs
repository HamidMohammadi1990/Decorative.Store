using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.DeliveryOptions.Commands;

public class UpdateDeliveryOptionHandler
    (IDeliveryOptionRepository deliveryOptionRepository, IUnitOfWork uow)
    : IRequestHandler<UpdateDeliveryOptionRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateDeliveryOptionRequest request, CancellationToken cancellationToken)
    {
        var deliveryOption = await deliveryOptionRepository.FindAsync(request.Id);
        if (deliveryOption is null)
            return ErrorModel.Create("InvalidId");

        deliveryOption.Update(request.Title, request.DeliveryDays);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}