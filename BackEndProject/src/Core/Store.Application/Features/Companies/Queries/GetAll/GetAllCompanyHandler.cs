using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.Companies.Queries;

public class GetAllCompanyHandler
    (ICompanyRepository companyRepository, ICompanyMapperService mapper)
    : IRequestHandler<GetAllCompanyRequest, OperationResult<PagedResult<GetAllCompanyResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllCompanyResponse>>> Handle(GetAllCompanyRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var companies = await companyRepository.GetAllAsync(requestModel);
        var result = mapper.Map(companies);
        return result;
    }
}