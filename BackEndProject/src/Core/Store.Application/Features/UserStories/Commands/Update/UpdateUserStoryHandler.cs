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

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        return saveChangesResult.IsSuccess ? OperationResult.Success() : saveChangesResult;
    }
}
