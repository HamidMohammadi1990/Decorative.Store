using Edition.Application.Contracts;
using Edition.Application.Contracts.Localization;
using MediatR;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductWishlists.Queries;

public class GetMyProductWishlistHandler
    (
        ICurrentUserContext currentUser,
        ICurrentLanguageContext languageContext,
        ILanguageRegistry languageRegistry,
        IProductWishlistRepository productWishlistRepository)
    : IRequestHandler<GetMyProductWishlistRequest, OperationResult<GetMyProductWishlistResponse>>
{
    public async Task<OperationResult<GetMyProductWishlistResponse>> Handle(
        GetMyProductWishlistRequest request,
        CancellationToken cancellationToken)
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var slugs = await productWishlistRepository.GetSlugsByUserIdAsync(
            currentUser.UserId,
            languageContext.LanguageId,
            defaultLanguage.Id,
            cancellationToken);

        return new GetMyProductWishlistResponse { Slugs = slugs };
    }
}
