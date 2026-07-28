using FluentValidation;
using Store.Common.Extensions;
using Store.Common.Localization;

namespace Edition.Application.Features.Users.Commands;

public class ForgetPasswordValidator : AbstractValidator<ForgetPasswordRequest>
{
    public ForgetPasswordValidator()
    {
        RuleFor(x => x.UserName)
            .NotNull()
            .Must(x => x.Length > 0)
            .WithMessage(MessageKeys.UserNameIsNotValid)
            .Must(x => x.IsEmail() || x.IsMobile())
            .WithMessage(MessageKeys.EnterMobileOrEmailAsUserName);

        RuleFor(x => x.OptionType)
            .IsInEnum()
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
