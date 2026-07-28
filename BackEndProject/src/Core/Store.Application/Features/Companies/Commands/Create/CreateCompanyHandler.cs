using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.Companies.Commands;

public class CreateCompanyHandler
    (IUnitOfWork uow, ICompanyRepository companyRepository, ICurrentUserContext currentUser)
    : IRequestHandler<CreateCompanyRequest, OperationResult<CreateCompanyResponse>>
{
    public async Task<OperationResult<CreateCompanyResponse>> Handle(CreateCompanyRequest request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        var company = Company.Create(userId, request.CityId, request.CompanyName, request.CompanyCode,
                                      request.PhoneNumber, request.Email, request.PostalCode,
                                      request.Address, request.Description, request.Latitude, request.Longitude);
        companyRepository.Add(company);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateCompanyResponse>();

        return new CreateCompanyResponse { Id = company.Id };
    }
}