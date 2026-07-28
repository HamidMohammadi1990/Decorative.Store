using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ChartOfAccounts.Commands;

public class DeleteChartOfAccountHandler
    (IUnitOfWork uow, IChartOfAccountRepository chartOfAccountRepository)
    : IRequestHandler<DeleteChartOfAccountRequest, OperationResult>
{
    async Task<OperationResult> IRequestHandler<DeleteChartOfAccountRequest, OperationResult>.Handle(DeleteChartOfAccountRequest request, CancellationToken cancellationToken)
    {
        var chartOfAccount = await chartOfAccountRepository.FindAsync(request.Id, cancellationToken);
        if (chartOfAccount is null)
            return ErrorModel.Create("InvalidChartOfAccountId");

        chartOfAccountRepository.Remove(chartOfAccount);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}