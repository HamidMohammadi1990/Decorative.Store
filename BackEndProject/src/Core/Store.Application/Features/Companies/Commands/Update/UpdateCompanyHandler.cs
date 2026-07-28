using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Companies.Commands;

public class UpdateCompanyHandler
    (IUnitOfWork uow, ICompanyRepository companyRepository, ICurrentUserContext currentUser)
    : IRequestHandler<UpdateCompanyRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateCompanyRequest request, CancellationToken cancellationToken)
    {
        var company = await companyRepository.FindAsync(request.Id);
        if (company is null)
            return ErrorModel.Create("InvalidId");

        var userId = currentUser.UserId;
        if (company.UserId != userId)
            return ErrorModel.Create("InvalidRequest");

        company.Update(
            request.CityId, request.CompanyName, request.CompanyCode,
            request.PhoneNumber, request.Email, request.PostalCode,
            request.Address, request.Description, request.Latitude, request.Longitude);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}