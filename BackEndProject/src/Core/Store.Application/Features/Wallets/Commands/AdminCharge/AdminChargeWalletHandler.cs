using Edition.Application.Contracts.Persistence;
using Edition.Application.Features.Wallets.Common;
using Store.Common.Models;
using Store.Domain.Enums;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.Wallets.Commands;

public class AdminChargeWalletHandler
    (IUnitOfWork uow,
     IWalletRepository walletRepository,
     IFinancialYearRepository financialYearRepository,
     IFinancialDocumentRepository financialDocumentRepository)
    : IRequestHandler<AdminChargeWalletRequest, OperationResult<AdminChargeWalletResponse>>
{
    public async Task<OperationResult<AdminChargeWalletResponse>> Handle(
        AdminChargeWalletRequest request,
        CancellationToken cancellationToken)
    {
        var wallet = await walletRepository.FindAsync(request.WalletId, cancellationToken);
        if (wallet is null)
            return ErrorModel.Create("WalletNotFound");

        if (wallet.Status != WalletStatusType.Active)
            return ErrorModel.Create("WalletIsNotActive");

        var financialYear = await financialYearRepository.FindAsync(request.FinancialYearId, cancellationToken);
        if (financialYear is null)
            return ErrorModel.Create("FinancialYearNotFound");

        var description = string.IsNullOrWhiteSpace(request.Description)
            ? $"شارژ کیف پول {wallet.Title}"
            : request.Description.Trim();

        var financialDocument = WalletFinancialDocumentBuilder.BuildCredit(
            wallet,
            request.Amount,
            financialYear.Id,
            description);

        var walletTransaction = WalletTransaction.CreateIncremental(
            wallet.Id,
            request.Amount,
            description,
            WalletTransactionStatusType.Completed,
            wallet.UserId);

        financialDocument.AddWalletTransaction(walletTransaction);
        wallet.IncreaseBalance(request.Amount);

        financialDocumentRepository.Add(financialDocument);

        var saveResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return saveResult.ToGenericFailure<AdminChargeWalletResponse>();

        return new AdminChargeWalletResponse
        {
            WalletTransactionId = walletTransaction.Id,
            NewBalance = wallet.Balance
        };
    }
}
