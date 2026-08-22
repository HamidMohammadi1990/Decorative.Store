using Edition.Application.Contracts.Localization;
using MediatR;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.UserStories.Queries;

public class SearchActiveUserStoriesHandler
    (
        ICurrentLanguageContext languageContext,
        ILanguageRegistry languageRegistry,
        IUserStoryRepository userStoryRepository)
    : IRequestHandler<SearchActiveUserStoriesRequest, OperationResult<SearchActiveUserStoriesResponse>>
{
    public async Task<OperationResult<SearchActiveUserStoriesResponse>> Handle(
        SearchActiveUserStoriesRequest request,
        CancellationToken cancellationToken)
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var stories = await userStoryRepository.GetActiveAsync(
            languageContext.LanguageId,
            defaultLanguage.Id,
            request.Limit,
            cancellationToken);

        return new SearchActiveUserStoriesResponse
        {
            Items = stories.Select(x => new SearchActiveUserStoryItemResponse
            {
                Id = x.Id,
                Title = x.Title,
                Caption = x.Caption,
                MediaType = x.MediaType,
                MediaUrl = x.MediaPath,
                MediaAlt = x.MediaAlt,
                PosterUrl = x.PosterPath,
                ProductSlug = x.ProductSlug,
                CreatedOnUtc = x.CreatedOnUtc,
                OwnerFirstName = x.OwnerFirstName,
                OwnerLastName = x.OwnerLastName,
            }).ToList(),
        };
    }
}
