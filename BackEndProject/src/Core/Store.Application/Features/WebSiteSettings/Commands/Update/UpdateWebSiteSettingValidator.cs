using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.WebSiteSettings.Commands;

public class UpdateWebSiteSettingValidator : AbstractValidator<UpdateWebSiteSettingRequest>
{
    public UpdateWebSiteSettingValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage(MessageKeys.EmailIsNotValid);
    }
}
