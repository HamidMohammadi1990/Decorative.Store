using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.Products.Commands;

public class CreateProductHandler
    (IUnitOfWork uow, IProductRepository productRepository)
    : IRequestHandler<CreateProductRequest, OperationResult<CreateProductResponse>>
{
    public async Task<OperationResult<CreateProductResponse>> Handle(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = Product.Create(request.Title, request.Slug, request.Description, request.ProductCode, request.SubCategoryId);
        productRepository.Add(product);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateProductResponse>();

        return new CreateProductResponse { Id = product.Id };
    }
}