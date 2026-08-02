using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.ProductDescriptions.Commands;

public class CreateProductDescriptionHandler
    (IUnitOfWork uow, IProductDescriptionRepository productDescriptionRepository)
    : IRequestHandler<CreateProductDescriptionRequest, OperationResult<CreateProductDescriptionResponse>>
{
    public async Task<OperationResult<CreateProductDescriptionResponse>> Handle(CreateProductDescriptionRequest request, CancellationToken cancellationToken)
    {
        var productDescription = ProductDescription.Create(request.Description, request.ProductId, request.LanguageId);
        productDescriptionRepository.Add(productDescription);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateProductDescriptionResponse>();

        return new CreateProductDescriptionResponse { Id = productDescription.Id };
    }
}
