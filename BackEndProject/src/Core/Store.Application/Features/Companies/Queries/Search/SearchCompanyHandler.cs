using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.Companies.Queries;

public class SearchCompanyHandler
    (ICompanyRepository companyRepository, ICompanyMapperService mapper)
    : IRequestHandler<SearchCompanyRequest, OperationResult<PagedResult<SearchCompanyResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchCompanyResponse>>> Handle(SearchCompanyRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var companies = await companyRepository.SearchAsync(requestModel);
        var result = mapper.Map(companies);
        return result;
    }
}