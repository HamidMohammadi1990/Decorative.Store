using Store.Domain.Entities;

namespace Edition.Application.Contracts.Orders;

public interface IBankPaymentVerificationService
{
    Task<bool> VerifyAsync(
        BankTransaction bankTransaction,
        BankAccount bankAccount,
        string? gatewayReference,
        CancellationToken cancellationToken = default);
}
