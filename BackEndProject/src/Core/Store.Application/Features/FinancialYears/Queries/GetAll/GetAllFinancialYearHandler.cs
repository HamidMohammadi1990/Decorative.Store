using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.FinancialYears.Queries;

public class GetAllFinancialYearHandler
    (IFinancialYearRepository financialYearRepository, IFinancialYearMapperService mapper)
    : IRequestHandler<GetAllFinancialYearRequest, OperationResult<PagedResult<GetAllFinancialYearResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllFinancialYearResponse>>> Handle(GetAllFinancialYearRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var financialYears = await financialYearRepository.GetAllAsync(requestModel);
        var result = mapper.Map(financialYears);
        return result;
    }
}