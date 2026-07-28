using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductProperties.Commands;

public class UpdateProductPropertyHandler
    (IUnitOfWork uow, IProductPropertyRepository productPropertyRepository)
    : IRequestHandler<UpdateProductPropertyRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateProductPropertyRequest request, CancellationToken cancellationToken)
    {
        var productProperty = await productPropertyRepository.FindAsync(request.Id);
        if (productProperty is null)
            return ErrorModel.Create("InvalidId");

        productProperty.Update(request.ProductId, request.PropertyId, request.IsActive);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
