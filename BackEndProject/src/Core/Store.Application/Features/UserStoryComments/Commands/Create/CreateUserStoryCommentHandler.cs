using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.UserStoryComments.Commands;

public class CreateUserStoryCommentHandler
    (
        IUnitOfWork uow,
        IUserStoryRepository userStoryRepository,
        IUserStoryCommentRepository userStoryCommentRepository,
        ICurrentUserContext currentUser)
    : IRequestHandler<CreateUserStoryCommentRequest, OperationResult<CreateUserStoryCommentResponse>>
{
    public async Task<OperationResult<CreateUserStoryCommentResponse>> Handle(
        CreateUserStoryCommentRequest request,
        CancellationToken cancellationToken)
    {
        var story = await userStoryRepository.FindByIdAsync(request.UserStoryId, cancellationToken);
        if (story is null || !story.IsActive)
            return ErrorModel.Create("InvalidUserStoryId");

        var userId = currentUser.UserId;
        var comment = UserStoryComment.Create(userId, request.UserStoryId, request.Content);

        userStoryCommentRepository.Add(comment);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateUserStoryCommentResponse>();

        return new CreateUserStoryCommentResponse { Id = comment.Id };
    }
}
