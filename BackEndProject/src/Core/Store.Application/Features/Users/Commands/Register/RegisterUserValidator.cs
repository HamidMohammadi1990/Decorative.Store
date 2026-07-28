using FluentValidation;
using Store.Common.Extensions;
using Store.Common.Localization;

namespace Edition.Application.Features.Users.Commands;

public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.UserName)
            .NotNull()
            .WithMessage(MessageKeys.UserNameRequired)
            .Must(x => x.IsMobile() || x.IsEmail())
            .WithMessage(MessageKeys.UserNameMustBeMobileOrEmail);

        RuleFor(x => x.Token)
            .NotNull()
            .NotEmpty()
            .Must(x => x.Length > 0)
            .WithMessage(MessageKeys.OtpRequired);
    }
}
