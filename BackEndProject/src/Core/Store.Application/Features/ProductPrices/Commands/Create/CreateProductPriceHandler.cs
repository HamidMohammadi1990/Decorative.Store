using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPrices.Commands;

public class CreateProductPriceHandler
    (IUnitOfWork uow, IProductPriceRepository productPriceRepository)
    : IRequestHandler<CreateProductPriceRequest, OperationResult<CreateProductPriceResponse>>
{
    public async Task<OperationResult<CreateProductPriceResponse>> Handle(CreateProductPriceRequest request, CancellationToken cancellationToken)
    {
        var productPrice = ProductPrice.Create(request.Price, request.CooperationPrice, request.ProductId, request.CompanyId);
        productPriceRepository.Add(productPrice);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateProductPriceResponse>();

        return new CreateProductPriceResponse { Id = productPrice.Id };
    }
}