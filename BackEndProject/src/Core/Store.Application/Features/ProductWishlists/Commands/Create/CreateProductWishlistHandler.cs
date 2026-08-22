using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using MediatR;
using Store.Common.Models;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductWishlists.Commands;

public class CreateProductWishlistHandler
    (
        IUnitOfWork uow,
        ICurrentUserContext currentUser,
        IProductRepository productRepository,
        IProductWishlistRepository productWishlistRepository)
    : IRequestHandler<CreateProductWishlistRequest, OperationResult>
{
    public async Task<OperationResult> Handle(CreateProductWishlistRequest request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetCatalogProductBySlugAsync(request.Slug, cancellationToken);
        if (product is null || product.NotFound || product.Product is null)
            return ErrorModel.Create("InvalidProduct");

        var productId = product.Product.Id;
        var userId = currentUser.UserId;

        var exists = await productWishlistRepository.AnyAsync(
            x => x.UserId == userId && x.ProductId == productId,
            cancellationToken);

        if (exists)
            return OperationResult.Success();

        productWishlistRepository.Add(ProductWishlist.Create(userId, productId));

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        return saveChangesResult.IsSuccess ? OperationResult.Success() : saveChangesResult;
    }
}
