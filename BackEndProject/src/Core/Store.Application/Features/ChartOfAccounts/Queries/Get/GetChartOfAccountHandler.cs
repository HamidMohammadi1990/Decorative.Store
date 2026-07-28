using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ChartOfAccounts.Queries;

public class GetChartOfAccountHandler
    (IChartOfAccountRepository chartOfAccountRepository, IChartOfAccountMapperService mapper)
    : IRequestHandler<GetChartOfAccountRequest, OperationResult<GetChartOfAccountResponse?>>
{
    public async Task<OperationResult<GetChartOfAccountResponse?>> Handle(GetChartOfAccountRequest request, CancellationToken cancellationToken)
    {
        var chartOfAccount = await chartOfAccountRepository.GetAsNoTrackingAsync(request.Id);
        if (chartOfAccount is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(chartOfAccount);
        return result;
    }
}