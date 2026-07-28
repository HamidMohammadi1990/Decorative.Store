using FluentValidation;

namespace Edition.Application.Features.Orders.Commands;

public class VerifyPaymentOrderValidator : AbstractValidator<VerifyPaymentOrderRequest>
{
    public VerifyPaymentOrderValidator()
    {
        RuleFor(x => x.GatewayReference)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.GatewayReference));
    }
}
