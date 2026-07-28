using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Wallets.Commands;

public class ChargeWalletValidator : AbstractValidator<ChargeWalletRequest>
{
    public ChargeWalletValidator()
    {
        RuleFor(x => x.WalletId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.WalletNotSelected);

        RuleFor(x => x.BankId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.BankNotSelected);

        RuleFor(x => x.Amount)
            .GreaterThan(0);
    }
}
