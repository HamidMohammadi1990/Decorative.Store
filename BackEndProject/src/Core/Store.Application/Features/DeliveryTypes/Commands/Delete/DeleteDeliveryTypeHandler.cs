using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.DeliveryTypes.Commands;

public class DeleteDeliveryTypeHandler
    (IUnitOfWork uow, IDeliveryTypeRepository deliveryTypeRepository)
    : IRequestHandler<DeleteDeliveryTypeRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteDeliveryTypeRequest request, CancellationToken cancellationToken)
    {
        var deliveryType = await deliveryTypeRepository.FindAsync(request.Id);
        if (deliveryType is null)
            return ErrorModel.Create("InvalidId");

        deliveryTypeRepository.Remove(deliveryType);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
