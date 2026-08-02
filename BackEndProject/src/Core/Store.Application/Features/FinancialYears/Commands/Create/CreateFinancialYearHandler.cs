using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.FinancialYears.Commands;

public class CreateFinancialYearHandler
    (IUnitOfWork uow, IFinancialYearRepository financialYearRepository)
    : IRequestHandler<CreateFinancialYearRequest, OperationResult<CreateFinancialYearResponse>>
{
    public async Task<OperationResult<CreateFinancialYearResponse>> Handle(CreateFinancialYearRequest request, CancellationToken cancellationToken)
    {
        var financialYear = FinancialYear.Create(
            request.Name,
            request.StartDate,
            request.EndDate);

        financialYearRepository.Add(financialYear);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateFinancialYearResponse>();

        return new CreateFinancialYearResponse { Id = financialYear.Id };
    }
}
