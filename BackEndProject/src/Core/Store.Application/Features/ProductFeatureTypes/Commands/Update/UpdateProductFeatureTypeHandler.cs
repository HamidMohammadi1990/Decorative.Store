using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductFeatureTypes.Commands;

public class UpdateProductFeatureTypeHandler
    (IUnitOfWork uow, IProductFeatureTypeRepository productFeatureTypeRepository)
    : IRequestHandler<UpdateProductFeatureTypeRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateProductFeatureTypeRequest request, CancellationToken cancellationToken)
    {
        var productFeatureType = await productFeatureTypeRepository.FindAsync(request.Id);
        if (productFeatureType is null)
            return ErrorModel.Create("InvalidId");

        productFeatureType.Update(request.Name, request.Type, request.DataType, request.Description, request.IsActive);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
