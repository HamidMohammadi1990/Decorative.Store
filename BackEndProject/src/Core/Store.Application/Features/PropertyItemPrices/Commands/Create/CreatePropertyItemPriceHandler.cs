using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.PropertyItemPrices.Commands;

public class CreatePropertyItemPriceHandler
    (IUnitOfWork uow, IPropertyItemPriceRepository propertyItemPriceRepository)
    : IRequestHandler<CreatePropertyItemPriceRequest, OperationResult<CreatePropertyItemPriceResponse>>
{
    public async Task<OperationResult<CreatePropertyItemPriceResponse>> Handle(CreatePropertyItemPriceRequest request, CancellationToken cancellationToken)
    {
        var propertyItemPrice = PropertyItemPrice.Create(request.CompanyId, request.PropertyItemId, request.Price, request.CooperationPrice);

        propertyItemPriceRepository.Add(propertyItemPrice);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreatePropertyItemPriceResponse>();

        return new CreatePropertyItemPriceResponse { Id = propertyItemPrice.Id };
    }
}