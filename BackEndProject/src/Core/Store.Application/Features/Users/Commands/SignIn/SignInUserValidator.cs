using FluentValidation;
using Store.Common.Extensions;
using Store.Common.Localization;

namespace Edition.Application.Features.Users.Commands;

public class SignInUserValidator : AbstractValidator<SignInUserRequest>
{
    public SignInUserValidator()
    {
        RuleFor(x => x.UserName)
            .NotNull()
            .Must(x => x.Length > 0)
            .WithMessage(MessageKeys.UserNameIsNotValid)
            .Must(x => x.IsEmail() || x.IsMobile())
            .WithMessage(MessageKeys.EnterMobileOrEmailAsUserName);

        RuleFor(x => x.Password)
            .NotNull()
            .MinimumLength(5)
            .WithMessage(MessageKeys.PasswordIsNotValid);
    }
}
