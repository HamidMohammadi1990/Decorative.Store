using FluentValidation;
using Store.Common.Extensions;
using Store.Common.Localization;

namespace Edition.Application.Features.Users.Commands;

public class ChangePhoneNumberValidator : AbstractValidator<ChangePhoneNumberRequest>
{
    public ChangePhoneNumberValidator()
    {
        RuleFor(x => x.Token)
            .NotNull()
            .NotEmpty()
            .Must(x => x.Length > 0)
            .WithMessage(MessageKeys.OtpRequired);

        RuleFor(x => x.PhoneNumber)
            .NotNull()
            .Must(x => x.IsMobile())
            .WithMessage(MessageKeys.MobileIsNotValid);
    }
}
