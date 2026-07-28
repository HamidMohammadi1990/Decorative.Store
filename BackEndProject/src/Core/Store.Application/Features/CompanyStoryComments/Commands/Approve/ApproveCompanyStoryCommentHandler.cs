using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyStoryComments.Commands;

public class ApproveCompanyStoryCommentHandler
    (IUnitOfWork uow, ICompanyStoryCommentRepository companyStoryCommentRepository, ICurrentUserContext currentUser)
    : IRequestHandler<ApproveCompanyStoryCommentRequest, OperationResult>
{
    public async Task<OperationResult> Handle(ApproveCompanyStoryCommentRequest request, CancellationToken cancellationToken)
    {
        var companyStoryComment = await companyStoryCommentRepository.FindAsync(request.Id, cancellationToken);
        if (companyStoryComment is null)
            return ErrorModel.Create("InvalidId");

        var userId = currentUser.UserId;
        companyStoryComment.Approve(userId);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
