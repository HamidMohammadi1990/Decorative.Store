using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.UserStoryLikes.Commands;

public class ToggleUserStoryLikeHandler
    (
        IUnitOfWork uow,
        IUserStoryRepository userStoryRepository,
        IUserStoryLikeRepository userStoryLikeRepository,
        ICurrentUserContext currentUser)
    : IRequestHandler<ToggleUserStoryLikeRequest, OperationResult<ToggleUserStoryLikeResponse>>
{
    public async Task<OperationResult<ToggleUserStoryLikeResponse>> Handle(
        ToggleUserStoryLikeRequest request,
        CancellationToken cancellationToken)
    {
        var story = await userStoryRepository.FindByIdAsync(request.UserStoryId, cancellationToken);
        if (story is null || !story.IsActive)
            return ErrorModel.Create("InvalidUserStoryId");

        var userId = currentUser.UserId;
        var existing = await userStoryLikeRepository.FindByUserAndStoryAsync(userId, request.UserStoryId, cancellationToken);

        if (existing is not null)
        {
            userStoryLikeRepository.Remove(existing);
        }
        else
        {
            userStoryLikeRepository.Add(UserStoryLike.Create(userId, request.UserStoryId));
        }

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<ToggleUserStoryLikeResponse>();

        var storyIds = new[] { request.UserStoryId };
        var likeCounts = await userStoryLikeRepository.GetLikeCountsByStoryIdsAsync(storyIds, cancellationToken);
        var liked = existing is null;

        return new ToggleUserStoryLikeResponse
        {
            Liked = liked,
            LikeCount = likeCounts.GetValueOrDefault(request.UserStoryId),
        };
    }
}
