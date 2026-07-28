using FluentValidation;
using Store.Common.Extensions;
using Store.Common.Localization;

namespace Edition.Application.Features.Users.Commands;

public class ChangeEmailValidator : AbstractValidator<ChangeEmailRequest>
{
    public ChangeEmailValidator()
    {
        RuleFor(x => x.Token)
            .NotNull()
            .NotEmpty()
            .Must(x => x.Length > 0)
            .WithMessage(MessageKeys.OtpRequired);

        RuleFor(x => x.Email)
            .NotNull()
            .WithMessage(MessageKeys.EmailRequired)
            .Must(x => x.IsEmail())
            .WithMessage(MessageKeys.EmailIsNotValid);
    }
}
