using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyStoryComments.Commands;

public class UpdateCompanyStoryCommentHandler
    (IUnitOfWork uow, ICompanyStoryCommentRepository companyStoryCommentRepository)
    : IRequestHandler<UpdateCompanyStoryCommentRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateCompanyStoryCommentRequest request, CancellationToken cancellationToken)
    {
        var companyStoryComment = await companyStoryCommentRepository.FindAsync(request.Id, cancellationToken);
        if (companyStoryComment is null)
            return ErrorModel.Create("InvalidId");

        companyStoryComment.Update(request.ParentId, request.Content);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
