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
        IUserStoryRepository userStoryRepository,
        IUserStoryLikeRepository userStoryLikeRepository,
        IUserStoryCommentRepository userStoryCommentRepository)
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

        var storyIds = stories.Select(x => x.Id).ToList();
        var likeCounts = await userStoryLikeRepository.GetLikeCountsByStoryIdsAsync(storyIds, cancellationToken);
        var commentCounts = await userStoryCommentRepository.GetApprovedCommentCountsByStoryIdsAsync(storyIds, cancellationToken);

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
                LikeCount = likeCounts.GetValueOrDefault(x.Id),
                CommentCount = commentCounts.GetValueOrDefault(x.Id),
            }).ToList(),
        };
    }
}
