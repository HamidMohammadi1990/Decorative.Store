using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.ProductProperties.Commands;

public class CreateProductPropertyHandler
    (IUnitOfWork uow, IProductPropertyRepository productPropertyRepository)
    : IRequestHandler<CreateProductPropertyRequest, OperationResult<CreateProductPropertyResponse>>
{
    public async Task<OperationResult<CreateProductPropertyResponse>> Handle(CreateProductPropertyRequest request, CancellationToken cancellationToken)
    {
        var productProperty = ProductProperty.Create(request.ProductId, request.PropertyId, request.IsActive);
        productPropertyRepository.Add(productProperty);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateProductPropertyResponse>();

        return new CreateProductPropertyResponse { Id = productProperty.Id };
    }
}
