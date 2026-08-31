using Edition.Application.Contracts;
using Edition.Application.Contracts.Localization;
using MediatR;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.UserStories.Queries;

public class SearchActiveUserStoriesHandler
    (
        ICurrentLanguageContext languageContext,
        ILanguageRegistry languageRegistry,
        IUserStoryRepository userStoryRepository,
        IUserStoryLikeRepository userStoryLikeRepository,
        IUserStoryCommentRepository userStoryCommentRepository,
        ICurrentUserContext currentUser)
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

        var storyIds = stories.Select(x => x.Id).ToList();
        var likeCounts = await userStoryLikeRepository.GetLikeCountsByStoryIdsAsync(storyIds, cancellationToken);
        var commentCounts = await userStoryCommentRepository.GetApprovedCommentCountsByStoryIdsAsync(storyIds, cancellationToken);

        HashSet<int> likedStoryIds = [];
        if (currentUser.IsAuthenticated && storyIds.Count > 0)
            likedStoryIds = await userStoryLikeRepository.GetLikedStoryIdsForUserAsync(
                currentUser.UserId,
                storyIds,
                cancellationToken);

        return new SearchActiveUserStoriesResponse
        {
            Items = stories.Select(x => new SearchActiveUserStoryItemResponse
            {
                Id = x.Id,
                UserId = x.UserId,
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
                LikeCount = likeCounts.GetValueOrDefault(x.Id),
                CommentCount = commentCounts.GetValueOrDefault(x.Id),
                IsLikedByCurrentUser = likedStoryIds.Contains(x.Id),
            }).ToList(),
        };
    }
}
