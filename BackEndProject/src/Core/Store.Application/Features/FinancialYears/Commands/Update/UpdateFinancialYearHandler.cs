using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.FinancialYears.Commands;

public class UpdateFinancialYearHandler
    (IUnitOfWork uow, IFinancialYearRepository financialYearRepository)
    : IRequestHandler<UpdateFinancialYearRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateFinancialYearRequest request, CancellationToken cancellationToken)
    {
        var financialYear = await financialYearRepository.FindAsync(request.Id, cancellationToken);

        if (financialYear is null)
            return ErrorModel.Create("InvalidFinancialYearId");

        financialYear.Update(request.Name, request.StartDate, request.EndDate, request.IsActive, request.CompanyId);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}