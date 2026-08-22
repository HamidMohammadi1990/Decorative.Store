using Edition.Application.Contracts;
using Edition.Application.Contracts.Localization;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductComments.Queries;

public class GetMyProductCommentsHandler
    (
        ICurrentUserContext currentUser,
        ICurrentLanguageContext languageContext,
        ILanguageRegistry languageRegistry,
        IProductCommentRepository productCommentRepository)
    : IRequestHandler<GetMyProductCommentsRequest, OperationResult<GetMyProductCommentsResponse>>
{
    public async Task<OperationResult<GetMyProductCommentsResponse>> Handle(
        GetMyProductCommentsRequest request,
        CancellationToken cancellationToken)
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;

        var comments = await productCommentRepository.GetByUserIdAsync(
            currentUser.UserId,
            languageId,
            defaultLanguage.Id,
            cancellationToken);

        return new GetMyProductCommentsResponse
        {
            Items = comments.Select(x => new GetMyProductCommentItemResponse
            {
                Id = x.Id,
                ProductTitle = x.ProductTitle,
                ProductSlug = x.ProductSlug,
                CommentRate = x.CommentRate,
                Description = x.Description,
                IsActive = x.IsActive,
            }).ToList(),
        };
    }
}
