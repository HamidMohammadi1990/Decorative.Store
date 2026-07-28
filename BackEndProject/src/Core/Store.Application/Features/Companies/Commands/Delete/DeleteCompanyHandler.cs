using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Companies.Commands;

public class DeleteCompanyHandler
    (IUnitOfWork uow, ICompanyRepository companyRepository, ICurrentUserContext currentUser)
    : IRequestHandler<DeleteCompanyRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteCompanyRequest request, CancellationToken cancellationToken)
    {
        var company = await companyRepository.FindAsync(request.Id);
        if (company is null)
            return ErrorModel.Create("InvalidId");

        var userid = currentUser.UserId;
        if (company.UserId != userid)
            return ErrorModel.Create("InvalidRequest");

        if (!company.IsActive)
            return OperationResult.Success();

        company.InActive();

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}