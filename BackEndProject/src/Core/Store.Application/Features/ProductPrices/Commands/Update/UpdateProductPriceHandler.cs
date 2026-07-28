using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPrices.Commands;

public class UpdateProductPriceHandler
    (IUnitOfWork uow, IProductPriceRepository productPriceRepository)
    : IRequestHandler<UpdateProductPriceRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateProductPriceRequest request, CancellationToken cancellationToken)
    {
        var productPrice = await productPriceRepository.FindAsync(request.Id);
        if (productPrice is null)
            return ErrorModel.Create("InvalidId");

        if (productPrice.CompanyId != request.CompanyId || productPrice.ProductId != request.ProductId)
            return ErrorModel.Create("InvalidRequest");

        productPrice.Update(request.Price, request.CooperationPrice, request.IsActive);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}