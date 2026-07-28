using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.ChartOfAccounts.Commands;

public class UpdateChartOfAccountValidator : AbstractValidator<UpdateChartOfAccountRequest>
{
    public UpdateChartOfAccountValidator()
    {
        RuleFor(x => x.AccountCode)
            .NotEmpty().WithMessage(MessageKeys.AccountCodeRequired)
            .MaximumLength(20).WithMessage(MessageKeys.AccountCodeMaxLength20);

        RuleFor(x => x.AccountTitle)
            .NotEmpty().WithMessage(MessageKeys.AccountTitleRequired)
            .MaximumLength(50).WithMessage(MessageKeys.AccountTitleMaxLength50);

        RuleFor(x => x.AccountCode)
            .Matches(@"^[A-Za-z0-9]+$").WithMessage(MessageKeys.AccountCodeAlphanumericRequired);

        RuleFor(x => x.AccountTitle)
            .Matches(@"^[\w\s]+$").WithMessage(MessageKeys.AccountTitleLettersAndSpacesRequired);
    }
}
