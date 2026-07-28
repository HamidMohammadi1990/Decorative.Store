using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Users.Commands;

public class ChangePasswordByOldPasswordValidator : AbstractValidator<ChangePasswordByOldPasswordRequest>
{
    public ChangePasswordByOldPasswordValidator()
    {
        RuleFor(x => x.OldPassword)
            .NotNull()
            .Must(x => x.Length > 0)
            .WithMessage(MessageKeys.PasswordIsNotValid);

        RuleFor(x => x.NewPassword)
            .NotNull()
            .Must(x => x.Length > 0)
            .WithMessage(MessageKeys.PasswordIsNotValid);
    }
}
