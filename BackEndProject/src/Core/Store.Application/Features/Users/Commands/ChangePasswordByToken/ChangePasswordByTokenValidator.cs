using FluentValidation;
using Store.Common.Extensions;
using Store.Common.Localization;

namespace Edition.Application.Features.Users.Commands;

public class ChangePasswordByTokenValidator : AbstractValidator<ChangePasswordByTokenRequest>
{
    public ChangePasswordByTokenValidator()
    {
        RuleFor(x => x.Token)
            .NotNull()
            .NotEmpty()
            .Must(x => x.Length > 0)
            .WithMessage(MessageKeys.OtpRequired);

        RuleFor(x => x.UserName)
            .NotNull()
            .Must(x => x.Length > 0)
            .WithMessage(MessageKeys.UserNameIsNotValid)
            .Must(x => x.IsEmail() || x.IsMobile())
            .WithMessage(MessageKeys.EnterMobileOrEmailAsUserName);

        RuleFor(x => x.Password)
            .NotNull()
            .Must(x => x.Length > 0)
            .WithMessage(MessageKeys.PasswordIsNotValid);

        RuleFor(x => x.OptionType)
            .IsInEnum()
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
