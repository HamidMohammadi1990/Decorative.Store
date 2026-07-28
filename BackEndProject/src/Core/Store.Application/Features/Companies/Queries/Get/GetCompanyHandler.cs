using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Companies.Queries;

public class GetCompanyHandler
    (ICompanyRepository companyRepository, ICompanyMapperService mapper)
    : IRequestHandler<GetCompanyRequest, OperationResult<GetCompanyResponse?>>
{
    public async Task<OperationResult<GetCompanyResponse?>> Handle(GetCompanyRequest request, CancellationToken cancellationToken)
    {
        var company = await companyRepository.GetAsNoTrackingAsync(request.Id);
        if (company is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(company);
        return result;
    }
}