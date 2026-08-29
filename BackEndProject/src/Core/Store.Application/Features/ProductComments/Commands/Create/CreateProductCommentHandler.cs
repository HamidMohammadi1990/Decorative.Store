using Store.Common.Extensions;
using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.ProductComments.Commands;

public class CreateProductCommentHandler
    (IUnitOfWork uow, IProductCommentRepository productCommentRpository, ICurrentUserContext currentUser)
    : IRequestHandler<CreateProductCommentRequest, OperationResult<CreateProductCommentResponse>>
{
    public async Task<OperationResult<CreateProductCommentResponse>> Handle(CreateProductCommentRequest request, CancellationToken cancellationToken)
    {
        if (request.ParentId.HasValue)
        {
            var parent = await productCommentRpository.FindAsync(request.ParentId.Value, cancellationToken);
            if (parent is null || parent.ProductId != request.ProductId)
                return ErrorModel.Create("InvalidParentComment");
        }

        var productComment = ProductComment.Create(
            currentUser.UserId,
            request.ProductId,
            request.CommentRate,
            request.QualityRating,
            request.CommentTopicId,
            request.Description,
            request.AffordableRating,
            request.ParentId);

        productCommentRpository.Add(productComment);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateProductCommentResponse>();

        return new CreateProductCommentResponse { Id = productComment.Id };
    }
}