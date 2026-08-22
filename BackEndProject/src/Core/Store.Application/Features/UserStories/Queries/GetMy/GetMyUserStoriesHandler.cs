using Edition.Application.Contracts;
using Edition.Application.Contracts.Localization;
using MediatR;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.UserStories.Queries;

public class GetMyUserStoriesHandler
    (
        ICurrentUserContext currentUser,
        ICurrentLanguageContext languageContext,
        ILanguageRegistry languageRegistry,
        IUserStoryRepository userStoryRepository)
    : IRequestHandler<GetMyUserStoriesRequest, OperationResult<GetMyUserStoriesResponse>>
{
    public async Task<OperationResult<GetMyUserStoriesResponse>> Handle(
        GetMyUserStoriesRequest request,
        CancellationToken cancellationToken)
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var stories = await userStoryRepository.GetByUserIdAsync(
            currentUser.UserId,
            languageContext.LanguageId,
            defaultLanguage.Id,
            cancellationToken);

        return new GetMyUserStoriesResponse
        {
            Items = stories.Select(x => new GetMyUserStoryItemResponse
            {
                Id = x.Id,
                Title = x.Title,
                Caption = x.Caption,
                MediaType = x.MediaType,
                MediaUrl = x.MediaPath,
                MediaAlt = x.MediaAlt,
                PosterUrl = x.PosterPath,
                ProductSlug = x.ProductSlug,
                IsActive = x.IsActive,
                CreatedOnUtc = x.CreatedOnUtc,
            }).ToList(),
        };
    }
}
