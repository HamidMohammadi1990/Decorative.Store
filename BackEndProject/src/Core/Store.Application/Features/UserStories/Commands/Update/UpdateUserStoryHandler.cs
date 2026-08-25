using Edition.Application.Common.Directories;
using Edition.Application.Common.Utilities.Contracts;
using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using MediatR;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.UserStories.Commands;

public class UpdateUserStoryHandler
    (
        IUnitOfWork uow,
        ICurrentUserContext currentUser,
        ILocalFileService localFileService,
        IProductRepository productRepository,
        IUserStoryRepository userStoryRepository)
    : IRequestHandler<UpdateUserStoryRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateUserStoryRequest request, CancellationToken cancellationToken)
    {
        var userStory = await userStoryRepository.FindByIdAndUserIdAsync(request.Id, currentUser.UserId, cancellationToken);
        if (userStory is null)
            return ErrorModel.Create("InvalidId");

        int? productId = null;
        if (!string.IsNullOrWhiteSpace(request.ProductSlug))
        {
            var product = await productRepository.GetCatalogProductBySlugAsync(request.ProductSlug.Trim(), cancellationToken);
            if (product is null || product.NotFound || product.Product is null)
                return ErrorModel.Create("InvalidProduct");

            productId = product.Product.Id;
        }

        userStory.Update(
            request.Title.Trim(),
            string.IsNullOrWhiteSpace(request.Caption) ? null : request.Caption.Trim(),
            request.IsActive,
            productId);

        if (!string.IsNullOrWhiteSpace(request.MediaPath) && request.MediaType.HasValue)
        {
            var previousMediaPath = userStory.MediaPath;
            var previousPosterPath = userStory.PosterPath;
            var nextMediaPath = request.MediaPath.Trim();

            userStory.UpdateMedia(
                request.MediaType.Value,
                nextMediaPath,
                string.IsNullOrWhiteSpace(request.MediaAlt) ? userStory.MediaAlt : request.MediaAlt.Trim(),
                string.IsNullOrWhiteSpace(request.PosterPath) ? null : request.PosterPath.Trim());

            if (!string.Equals(previousMediaPath, nextMediaPath, StringComparison.OrdinalIgnoreCase))
                DeleteMediaFile(previousMediaPath);

            if (!string.IsNullOrWhiteSpace(previousPosterPath)
                && !string.Equals(previousPosterPath, userStory.PosterPath, StringComparison.OrdinalIgnoreCase))
            {
                DeleteMediaFile(previousPosterPath);
            }
        }

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        return saveChangesResult.IsSuccess ? OperationResult.Success() : saveChangesResult;
    }

    private void DeleteMediaFile(string publicPath)
    {
        if (!publicPath.StartsWith(UserStoryDirectory.PublicMediaPrefix, StringComparison.OrdinalIgnoreCase))
            return;

        var fileName = publicPath[UserStoryDirectory.PublicMediaPrefix.Length..];
        localFileService.DeleteFile(UserStoryDirectory.StoryMedia, fileName);
    }
}
