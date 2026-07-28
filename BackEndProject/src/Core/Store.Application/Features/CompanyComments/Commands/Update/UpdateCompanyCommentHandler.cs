using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyComments.Commands;

public class UpdateCompanyCommentHandler 
    (ICompanyCommentRepository companyCommentRepository, IUnitOfWork uow)
    : IRequestHandler<UpdateCompanyCommentRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateCompanyCommentRequest request, CancellationToken cancellationToken)
    {
        var companyComment = await companyCommentRepository.FindAsync(request.Id);
        if (companyComment is null)
            return ErrorModel.Create("InvalidId");

        companyComment.Update(request.Title, request.Description, request.Rate);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}