using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductProperties.Commands;

public class DeleteProductPropertyHandler
    (IUnitOfWork uow, IProductPropertyRepository productPropertyRepository)
    : IRequestHandler<DeleteProductPropertyRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteProductPropertyRequest request, CancellationToken cancellationToken)
    {
        var productProperty = await productPropertyRepository.FindAsync(request.Id);
        if (productProperty is null)
            return ErrorModel.Create("InvalidId");

        productPropertyRepository.Remove(productProperty);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
