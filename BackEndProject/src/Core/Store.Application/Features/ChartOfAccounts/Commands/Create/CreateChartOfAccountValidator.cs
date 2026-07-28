using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.ChartOfAccounts.Commands;

public class CreateChartOfAccountValidator : AbstractValidator<CreateChartOfAccountRequest>
{
    public CreateChartOfAccountValidator()
    {
        RuleFor(x => x.AccountCode)
            .NotEmpty()
            .WithMessage(MessageKeys.AccountCodeRequired)
            .MaximumLength(20)
            .WithMessage(MessageKeys.AccountCodeMaxLength20);

        RuleFor(x => x.AccountTitle)
            .NotEmpty()
            .WithMessage(MessageKeys.AccountTitleRequired)
            .MaximumLength(50)
            .WithMessage(MessageKeys.AccountTitleMaxLength50);

        RuleFor(x => x.AccountType)
            .IsInEnum()
            .WithMessage(MessageKeys.InvalidAccountType);

        RuleFor(x => x.DetailType)
            .IsInEnum()
            .WithMessage(MessageKeys.InvalidAccountDetail);
    }
}
