using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Edition.Application.Features.Wallets.Common;
using Store.Common.Models;
using Store.Domain.Enums;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.Wallets.Commands;

public class ChargeWalletHandler
    (IUnitOfWork uow,
     IWalletRepository walletRepository,
     IBankAccountRepository bankAccountRepository,
     IFinancialYearRepository financialYearRepository,
     IFinancialDocumentRepository financialDocumentRepository,
     ICurrentUserContext currentUser)
    : IRequestHandler<ChargeWalletRequest, OperationResult<ChargeWalletResponse>>
{
    public async Task<OperationResult<ChargeWalletResponse>> Handle(
        ChargeWalletRequest request,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        var wallet = await walletRepository.FindByUserIdAsync(userId, request.WalletId, cancellationToken);
        if (wallet is null)
            return ErrorModel.Create("WalletNotFound");

        if (wallet.Status != WalletStatusType.Active)
            return ErrorModel.Create("WalletIsNotActive");

        if (await walletRepository.HasPendingBankChargeAsync(wallet.Id, cancellationToken))
            return ErrorModel.Create("WalletChargeInProgress");

        var bankAccount = await bankAccountRepository.GetActiveByIdAsync(request.BankId, cancellationToken);
        if (bankAccount is null)
            return ErrorModel.Create("InvalidBankId");

        var financialYear = wallet.CompanyId.HasValue
            ? await financialYearRepository.GetByCompanyIdAsync(wallet.CompanyId.Value)
            : await financialYearRepository.GetFirstActiveAsync(cancellationToken);

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
            WalletTransactionStatusType.Pending,
            userId);

        financialDocument.AddWalletTransaction(walletTransaction);

        var bankTransaction = BankTransaction.Create(
            TransactionStatusType.Pending,
            bankAccount.Id,
            userId,
            request.Amount,
            string.Empty,
            description);

        financialDocument.AddBankTransaction(bankTransaction);
        financialDocumentRepository.Add(financialDocument);

        var saveResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return saveResult.ToGenericFailure<ChargeWalletResponse>();

        return new ChargeWalletResponse
        {
            BankTransactionId = bankTransaction.Id,
            PaymentUrl = bankAccount.PaymentUrl,
            Amount = request.Amount
        };
    }
}
