using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.DeliveryTypes.Commands;

public class UpdateDeliveryTypeHandler
    (IUnitOfWork uow, IDeliveryTypeRepository deliveryTypeRepository)
    : IRequestHandler<UpdateDeliveryTypeRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateDeliveryTypeRequest request, CancellationToken cancellationToken)
    {
        var deliveryType = await deliveryTypeRepository.FindAsync(request.Id);
        if (deliveryType is null)
            return ErrorModel.Create("InvalidId");

        deliveryType.Update(request.Title, request.Priority, request.IsActive);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
