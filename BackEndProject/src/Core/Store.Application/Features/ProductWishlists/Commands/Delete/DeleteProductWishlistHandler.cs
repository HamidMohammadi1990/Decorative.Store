using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using MediatR;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductWishlists.Commands;

public class DeleteProductWishlistHandler
    (
        IUnitOfWork uow,
        ICurrentUserContext currentUser,
        IProductRepository productRepository,
        IProductWishlistRepository productWishlistRepository)
    : IRequestHandler<DeleteProductWishlistRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteProductWishlistRequest request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetCatalogProductBySlugAsync(request.Slug, cancellationToken);
        if (product is null || product.NotFound || product.Product is null)
            return ErrorModel.Create("InvalidProduct");

        var wishlistItem = await productWishlistRepository.FindByUserAndProductAsync(
            currentUser.UserId,
            product.Product.Id,
            cancellationToken);

        if (wishlistItem is null)
            return OperationResult.Success();

        productWishlistRepository.Remove(wishlistItem);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        return saveChangesResult.IsSuccess ? OperationResult.Success() : saveChangesResult;
    }
}
