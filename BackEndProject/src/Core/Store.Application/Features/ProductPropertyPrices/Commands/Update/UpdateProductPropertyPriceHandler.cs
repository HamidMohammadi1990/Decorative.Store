using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPropertyPrices.Commands;

public class UpdateProductPropertyPriceHandler
    (IUnitOfWork uow, IProductPropertyPriceRepository productPropertyPriceRepository)
    : IRequestHandler<UpdateProductPropertyPriceRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateProductPropertyPriceRequest request, CancellationToken cancellationToken)
    {
        var productPropertyPrice = await productPropertyPriceRepository.FindAsync(request.Id);
        if (productPropertyPrice is null)
            return ErrorModel.Create("InvalidId");

        if (productPropertyPrice.CompanyId != request.CompanyId || productPropertyPrice.ProductPropertyId != request.ProductPropertyId)
            return ErrorModel.Create("InvalidRequest");

        productPropertyPrice.Update(request.Price, request.CooperationPrice, request.IsActive);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}