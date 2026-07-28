using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.FinancialYears.Queries;

public class GetFinancialYearHandler
    (IFinancialYearRepository financialYearRepository, IFinancialYearMapperService mapper)
    : IRequestHandler<GetFinancialYearRequest, OperationResult<GetFinancialYearResponse>>
{
    public async Task<OperationResult<GetFinancialYearResponse>> Handle(GetFinancialYearRequest request, CancellationToken cancellationToken)
    {
        var financialYear = await financialYearRepository.GetAsNoTrackingAsync(request.Id);
        if (financialYear is null)
            return ErrorModel.Create("InvalidFinancialYearId");

        var result = mapper.Map(financialYear);
        return result;
    }
}