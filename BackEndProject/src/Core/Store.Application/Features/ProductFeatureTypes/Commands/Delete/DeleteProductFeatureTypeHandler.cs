using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductFeatureTypes.Commands;

public class DeleteProductFeatureTypeHandler
    (IUnitOfWork uow, IProductFeatureTypeRepository productFeatureTypeRepository)
    : IRequestHandler<DeleteProductFeatureTypeRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteProductFeatureTypeRequest request, CancellationToken cancellationToken)
    {
        var productFeatureType = await productFeatureTypeRepository.FindAsync(request.Id);
        if (productFeatureType is null)
            return ErrorModel.Create("InvalidId");

        productFeatureTypeRepository.Remove(productFeatureType);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
