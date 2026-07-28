using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductComments.Commands;

public class UpdateProductCommentHandler
    (IUnitOfWork uow, IProductCommentRepository productCommentRepository)
    : IRequestHandler<UpdateProductCommentRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateProductCommentRequest request, CancellationToken cancellationToken)
    {
        var productComment = await productCommentRepository.FindAsync(request.Id);
        if (productComment is null)
            return ErrorModel.Create("InvalidId");

        productComment.Update(request.CommentRate, request.QualityRating, request.CommentTopicId, request.Description, request.AffordableRating);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}