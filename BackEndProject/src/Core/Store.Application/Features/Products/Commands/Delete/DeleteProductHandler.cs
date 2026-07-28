using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Products.Commands;

public class DeleteProductHandler
    (IUnitOfWork uow, IProductRepository productRepository)
    : IRequestHandler<DeleteProductRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
    {
        var product = await productRepository.FindAsync(request.Id, cancellationToken);
        if (product is null)
            return ErrorModel.Create("InvalidId");

        product.DeActive();

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}