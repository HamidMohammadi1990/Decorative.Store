using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ChartOfAccounts.Queries;

public class GetAllChartOfAccountHandler
    (IChartOfAccountRepository chartOfAccountRepository, IChartOfAccountMapperService mapper)
    : IRequestHandler<GetAllChartOfAccountRequest, OperationResult<PagedResult<GetAllChartOfAccountResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllChartOfAccountResponse>>> Handle(GetAllChartOfAccountRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var accounts = await chartOfAccountRepository.GetAllAsync(requestModel);
        var result = mapper.Map(accounts);
        return result;
    }
}