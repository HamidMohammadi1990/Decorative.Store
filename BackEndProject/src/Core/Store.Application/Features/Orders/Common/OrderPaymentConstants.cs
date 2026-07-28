namespace Edition.Application.Features.Orders.Common;

public static class OrderPaymentConstants
{
    public const decimal VatRate = 0.10m;

    public const int CustomerChartOfAccountId = 11;
    public const int IntermediaryChartOfAccountId = 146;
    public const int VatChartOfAccountId = 147;

    /// <summary>
    /// Temporary stub until bank gateway verification is integrated.
    /// </summary>
    public const bool BankVerificationStubSucceeded = true;
}
