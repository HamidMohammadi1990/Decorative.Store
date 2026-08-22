using Edition.Application.Common.Directories;
using Edition.Application.Common.Utilities.Contracts;
using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using MediatR;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.UserStories.Commands;

public class DeleteUserStoryHandler
    (
        IUnitOfWork uow,
        ICurrentUserContext currentUser,
        ILocalFileService localFileService,
        IUserStoryRepository userStoryRepository)
    : IRequestHandler<DeleteUserStoryRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteUserStoryRequest request, CancellationToken cancellationToken)
    {
        var userStory = await userStoryRepository.FindByIdAndUserIdAsync(request.Id, currentUser.UserId, cancellationToken);
        if (userStory is null)
            return ErrorModel.Create("InvalidId");

        DeleteMediaFile(userStory.MediaPath);
        if (!string.IsNullOrWhiteSpace(userStory.PosterPath))
            DeleteMediaFile(userStory.PosterPath);

        userStoryRepository.Remove(userStory);

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
