using Edition.Application.Contracts.Orders;
using Edition.Application.Features.Orders.Common;
using Store.Domain.Entities;

namespace Edition.Application.Services.Orders;

public class BankPaymentVerificationService
    : IBankPaymentVerificationService
{
    public Task<bool> VerifyAsync(
        BankTransaction bankTransaction,
        BankAccount bankAccount,
        string? gatewayReference,
        CancellationToken cancellationToken = default)
    {
        _ = bankTransaction;
        _ = bankAccount;
        _ = gatewayReference;
        _ = cancellationToken;

        // TODO: call bankAccount.VerifyPaymentUrl gateway and validate amount/reference.
        return Task.FromResult(OrderPaymentConstants.BankVerificationStubSucceeded);
    }
}
