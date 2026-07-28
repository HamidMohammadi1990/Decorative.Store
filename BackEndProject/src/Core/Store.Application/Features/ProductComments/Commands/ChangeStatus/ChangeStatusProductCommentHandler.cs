using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductComments.Commands;

public class ChangeStatusProductCommentHandler
    (IUnitOfWork uow, IProductCommentRepository productCommentRepository)
    : IRequestHandler<ChangeStatusProductCommentRequest, OperationResult>
{
    public async Task<OperationResult> Handle(ChangeStatusProductCommentRequest request, CancellationToken cancellationToken)
    {
        var productComment = await productCommentRepository.FindAsync(request.Id, cancellationToken);
        if (productComment is null)
            return ErrorModel.Create("InvalidId");

        if (productComment.IsActive == request.IsActive)
            return OperationResult.Success();

        if (request.IsActive)
            productComment.Active();
        else
            productComment.InActive();

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}