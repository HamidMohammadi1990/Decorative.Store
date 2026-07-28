using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.ProductFeatureTypes.Commands;

public class CreateProductFeatureTypeHandler
    (IUnitOfWork uow, IProductFeatureTypeRepository productFeatureTypeRepository)
    : IRequestHandler<CreateProductFeatureTypeRequest, OperationResult<CreateProductFeatureTypeResponse>>
{
    public async Task<OperationResult<CreateProductFeatureTypeResponse>> Handle(CreateProductFeatureTypeRequest request, CancellationToken cancellationToken)
    {
        var productFeatureType = ProductFeatureType.Create(request.Name, request.Type, request.DataType, request.Description, request.IsActive);
        productFeatureTypeRepository.Add(productFeatureType);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateProductFeatureTypeResponse>();

        return new CreateProductFeatureTypeResponse { Id = productFeatureType.Id };
    }
}
