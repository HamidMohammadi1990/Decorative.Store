using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.DeliveryTypes.Commands;

public class CreateDeliveryTypeHandler
    (IUnitOfWork uow, IDeliveryTypeRepository deliveryTypeRepository)
    : IRequestHandler<CreateDeliveryTypeRequest, OperationResult<CreateDeliveryTypeResponse>>
{
    public async Task<OperationResult<CreateDeliveryTypeResponse>> Handle(CreateDeliveryTypeRequest request, CancellationToken cancellationToken)
    {
        var deliveryType = DeliveryType.Create(request.Title, request.Priority);
        deliveryTypeRepository.Add(deliveryType);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateDeliveryTypeResponse>();

        return new CreateDeliveryTypeResponse { Id = deliveryType.Id };
    }
}