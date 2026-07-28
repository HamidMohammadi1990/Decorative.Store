using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyStoryComments.Commands;

public class DeleteCompanyStoryCommentHandler
    (IUnitOfWork uow, ICompanyStoryCommentRepository companyStoryCommentRepository)
    : IRequestHandler<DeleteCompanyStoryCommentRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteCompanyStoryCommentRequest request, CancellationToken cancellationToken)
    {
        var companyStoryComment = await companyStoryCommentRepository.FindAsync(request.Id, cancellationToken);
        if (companyStoryComment is null)
            return ErrorModel.Create("InvalidId");

        companyStoryCommentRepository.Remove(companyStoryComment);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
