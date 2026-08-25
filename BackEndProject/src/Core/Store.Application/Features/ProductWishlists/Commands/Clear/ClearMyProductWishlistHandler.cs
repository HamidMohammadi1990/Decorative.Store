using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using MediatR;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductWishlists.Commands;

public class ClearMyProductWishlistHandler
    (
        IUnitOfWork uow,
        ICurrentUserContext currentUser,
        IProductWishlistRepository productWishlistRepository)
    : IRequestHandler<ClearMyProductWishlistRequest, OperationResult>
{
    public async Task<OperationResult> Handle(
        ClearMyProductWishlistRequest request,
        CancellationToken cancellationToken)
    {
        var items = await productWishlistRepository.GetByUserIdAsync(currentUser.UserId, cancellationToken);
        if (items.Count == 0)
            return OperationResult.Success();

        foreach (var item in items)
            productWishlistRepository.Remove(item);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        return saveChangesResult.IsSuccess ? OperationResult.Success() : saveChangesResult;
    }
}
