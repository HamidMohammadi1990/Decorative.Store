using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using MediatR;
using Store.Common.Models;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Edition.Application.Features.UserStories.Commands;

public class CreateUserStoryHandler
    (
        IUnitOfWork uow,
        ICurrentUserContext currentUser,
        IProductRepository productRepository,
        IUserStoryRepository userStoryRepository)
    : IRequestHandler<CreateUserStoryRequest, OperationResult<CreateUserStoryResponse>>
{
    public async Task<OperationResult<CreateUserStoryResponse>> Handle(
        CreateUserStoryRequest request,
        CancellationToken cancellationToken)
    {
        int? productId = null;
        if (!string.IsNullOrWhiteSpace(request.ProductSlug))
        {
            var product = await productRepository.GetCatalogProductBySlugAsync(request.ProductSlug.Trim(), cancellationToken);
            if (product is null || product.NotFound || product.Product is null)
                return ErrorModel.Create("InvalidProduct");

            productId = product.Product.Id;
        }

        var userStory = UserStory.Create(
            currentUser.UserId,
            request.Title.Trim(),
            string.IsNullOrWhiteSpace(request.Caption) ? null : request.Caption.Trim(),
            request.MediaType,
            request.MediaPath.Trim(),
            request.MediaAlt.Trim(),
            string.IsNullOrWhiteSpace(request.PosterPath) ? null : request.PosterPath.Trim(),
            productId);

        userStoryRepository.Add(userStory);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateUserStoryResponse>();

        return new CreateUserStoryResponse { Id = userStory.Id };
    }
}
