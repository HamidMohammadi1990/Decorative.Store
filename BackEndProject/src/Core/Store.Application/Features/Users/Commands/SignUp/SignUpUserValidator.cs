using FluentValidation;
using Store.Common.Extensions;
using Store.Common.Localization;

namespace Edition.Application.Features.Users.Commands;

public class SignUpUserValidator : AbstractValidator<SignUpUserRequest>
{
    public SignUpUserValidator()
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

        RuleFor(x => x.FirstName)
            .NotNull()
            .WithMessage(MessageKeys.FirstNameRequired)
            .MaximumLength(20)
            .WithMessage(MessageKeys.FirstNameMaxLength50);

        RuleFor(x => x.LastName)
            .NotNull()
            .WithMessage(MessageKeys.LastNameRequired)
            .MaximumLength(20)
            .WithMessage(MessageKeys.LastNameMaxLength50);
    }
}
