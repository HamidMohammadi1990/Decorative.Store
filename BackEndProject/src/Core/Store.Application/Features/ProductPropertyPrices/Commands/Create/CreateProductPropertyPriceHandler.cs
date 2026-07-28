using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.ProductPropertyPrices.Commands;

public class CreateProductPropertyPriceHandler
    (IUnitOfWork uow, IProductPropertyPriceRepository productPropertyPriceRepository)
    : IRequestHandler<CreateProductPropertyPriceRequest, OperationResult<CreateProductPropertyPriceResponse>>
{
    public async Task<OperationResult<CreateProductPropertyPriceResponse>> Handle(CreateProductPropertyPriceRequest request, CancellationToken cancellationToken)
    {
        var productPropertyPrice = ProductPropertyPrice.Create(request.CompanyId, request.ProductPropertyId, request.Price, request.CooperationPrice);
        productPropertyPriceRepository.Add(productPropertyPrice);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateProductPropertyPriceResponse>();

        return new CreateProductPropertyPriceResponse { Id = productPropertyPrice.Id };
    }
}