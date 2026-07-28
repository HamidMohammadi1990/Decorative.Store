using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductComments.Commands;

public class DeleteProductCommentHandler
    (IUnitOfWork uow, IProductCommentRepository productCommentRepository)
    : IRequestHandler<DeleteProductCommentRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteProductCommentRequest request, CancellationToken cancellationToken)
    {
        var productComment = await productCommentRepository.FindAsync(request.Id, cancellationToken);
        if (productComment is null)
            return ErrorModel.Create("InvalidId");

        productComment.InActive();

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}