using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyComments.Commands;

public class ChangeStatusCompanyCommentHandler
    (ICompanyCommentRepository companyCommentRepository, IUnitOfWork uow)
    : IRequestHandler<ChangeStatusCompanyCommentRequest, OperationResult>
{
    public async Task<OperationResult> Handle(ChangeStatusCompanyCommentRequest request, CancellationToken cancellationToken)
    {
        var companyComment = await companyCommentRepository.FindAsync(request.Id);
        if (companyComment is null)
            return ErrorModel.Create("InvalidId");

        if (companyComment.StatusType == request.Status)
            return OperationResult.Success();

        companyComment.ChangeStatus(request.Status);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}