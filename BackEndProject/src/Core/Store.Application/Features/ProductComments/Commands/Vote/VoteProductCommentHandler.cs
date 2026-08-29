using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductComments.Commands;

public class VoteProductCommentHandler
    (IUnitOfWork uow, IProductCommentRepository productCommentRepository, ICurrentUserContext currentUser)
    : IRequestHandler<VoteProductCommentRequest, OperationResult<VoteProductCommentResponse>>
{
    public async Task<OperationResult<VoteProductCommentResponse>> Handle(
        VoteProductCommentRequest request,
        CancellationToken cancellationToken)
    {
        var comment = await productCommentRepository.FindAsync(request.Id, cancellationToken);
        if (comment is null || !comment.IsActive)
            return ErrorModel.Create("CommentNotFound");

        var userId = currentUser.UserId;
        var existing = await productCommentRepository.FindReactionAsync(userId, request.Id, cancellationToken);

        if (existing is not null && existing.IsHelpful == request.IsHelpful)
        {
            productCommentRepository.RemoveReaction(existing);
        }
        else if (existing is not null)
        {
            existing.SetHelpful(request.IsHelpful);
        }
        else
        {
            productCommentRepository.AddReaction(
                ProductCommentReaction.Create(request.Id, userId, request.IsHelpful));
        }

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<VoteProductCommentResponse>();

        var (helpfulCount, notHelpfulCount) =
            await productCommentRepository.GetReactionCountsAsync(request.Id, cancellationToken);
        var userReaction = await productCommentRepository.FindReactionAsync(userId, request.Id, cancellationToken);

        return new VoteProductCommentResponse
        {
            HelpfulCount = helpfulCount,
            NotHelpfulCount = notHelpfulCount,
            UserVoteHelpful = userReaction?.IsHelpful,
        };
    }
}
