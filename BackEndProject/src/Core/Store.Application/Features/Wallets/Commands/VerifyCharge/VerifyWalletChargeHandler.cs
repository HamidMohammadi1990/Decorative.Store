using Edition.Application.Contracts;
using Edition.Application.Contracts.Orders;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.Persistence;
using Edition.Application.Features.Wallets.Common;
using Store.Common.Models;
using Store.Domain.Enums;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Wallets.Commands;

public class VerifyWalletChargeHandler
    (IUnitOfWork uow,
     IBankTransactionRepository bankTransactionRepository,
     ICurrentUserContext currentUser,
     IBankPaymentVerificationService bankPaymentVerificationService,
     IWalletMapperService mapper)
    : IRequestHandler<VerifyWalletChargeRequest, OperationResult<VerifyWalletChargeResponse>>
{
    public async Task<OperationResult<VerifyWalletChargeResponse>> Handle(
        VerifyWalletChargeRequest request,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        var charge = await bankTransactionRepository.GetWalletChargeAsync(
            userId,
            request.BankTransactionId,
            cancellationToken);

        if (charge is null)
            return ErrorModel.Create("BankTransactionNotFound");

        if (charge.BankTransaction.Status == TransactionStatusType.Completed &&
            charge.WalletTransaction.Status == WalletTransactionStatusType.Completed)
            return mapper.MapVerifyCharge(charge, isPaymentSuccessful: true);

        if (charge.BankTransaction.Status is TransactionStatusType.Failed or TransactionStatusType.Canceled)
            return ErrorModel.Create("PaymentFailed");

        if (charge.BankTransaction.Status != TransactionStatusType.Pending)
            return ErrorModel.Create("NoPendingWalletCharge");

        var isVerified = await bankPaymentVerificationService.VerifyAsync(
            charge.BankTransaction,
            charge.BankTransaction.BankAccount,
            request.GatewayReference,
            cancellationToken);

        if (isVerified)
            WalletChargeSettlement.CompleteSuccessfulBankCharge(charge, request.GatewayReference);
        else
            WalletChargeSettlement.FailBankCharge(charge);

        var saveResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return saveResult.ToGenericFailure<VerifyWalletChargeResponse>();

        if (!isVerified)
            return ErrorModel.Create("PaymentFailed");

        return mapper.MapVerifyCharge(charge, isPaymentSuccessful: true);
    }
}
