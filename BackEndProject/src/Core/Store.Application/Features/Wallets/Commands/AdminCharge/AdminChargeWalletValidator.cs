using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Wallets.Commands;

public class AdminChargeWalletValidator : AbstractValidator<AdminChargeWalletRequest>
{
    public AdminChargeWalletValidator()
    {
        RuleFor(x => x.WalletId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.WalletNotSelected);

        RuleFor(x => x.FinancialYearId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.FinancialYearNotFound);

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.Description)
            .MaximumLength(200);
    }
}
