using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PropertyItemPrices.Commands;

public class UpdatePropertyItemPriceHandler
    (IUnitOfWork uow, IPropertyItemPriceRepository propertyItemPriceRepository)
    : IRequestHandler<UpdatePropertyItemPriceRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdatePropertyItemPriceRequest request, CancellationToken cancellationToken)
    {
        var propertyItemPrice = await propertyItemPriceRepository.FindAsync(request.Id);
        if (propertyItemPrice is null)
            return ErrorModel.Create("InvalidId");

        propertyItemPrice.Update(request.Price, request.CooperationPrice, request.PropertyItemId, request.IsActive);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}