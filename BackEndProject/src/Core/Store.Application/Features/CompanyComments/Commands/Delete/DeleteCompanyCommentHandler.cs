using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Enums;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyComments.Commands;

public class DeleteCompanyCommentHandler
    (ICompanyCommentRepository companyCommentRepository, IUnitOfWork uow)
    : IRequestHandler<DeleteCompanyCommentRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteCompanyCommentRequest request, CancellationToken cancellationToken)
    {
        var companyComment = await companyCommentRepository.FindAsync(request.Id);
        if (companyComment is null)
            return ErrorModel.Create("InvalidId");

        if (companyComment.StatusType is CommentStatusType.Deleted)
            return OperationResult.Success();

        companyComment.ChangeStatus(CommentStatusType.Deleted);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}