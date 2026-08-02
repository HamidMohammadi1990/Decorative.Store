using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Products.Commands;

public class UpdateProductHandler
      (IUnitOfWork uow, IProductRepository productRepository)
      : IRequestHandler<UpdateProductRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var product = await productRepository.FindWithTranslationsAsync(request.Id, cancellationToken);
        if (product is null)
            return ErrorModel.Create("InvalidId");

        product.Update(
            request.Status,
            request.ProductCode,
            request.SubCategoryId,
            request.LanguageId,
            request.Title,
            request.Slug,
            request.Description);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
