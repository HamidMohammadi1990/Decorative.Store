using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Enums;

namespace Edition.Application.Features.Orders.Commands;

public class PaymentOrderValidator : AbstractValidator<PaymentOrderRequest>
{
    public PaymentOrderValidator()
    {
        RuleFor(x => x.PaymentOption)
            .IsInEnum()
            .WithMessage(MessageKeys.InvalidPaymentOption);

        RuleFor(x => x.BankId)
            .NotNull()
            .When(x => x.PaymentOption is PaymentOptionType.BankOnly or PaymentOptionType.WalletAndBank)
            .WithMessage(MessageKeys.BankNotSelected);

        RuleFor(x => x.WalletId)
            .NotNull()
            .When(x => x.PaymentOption is PaymentOptionType.WalletOnly or PaymentOptionType.WalletAndBank)
            .WithMessage(MessageKeys.WalletNotSelected);
    }
}
