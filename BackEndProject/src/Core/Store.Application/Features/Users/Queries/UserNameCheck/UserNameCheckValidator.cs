using FluentValidation;
using Store.Common.Extensions;
using Store.Common.Localization;

namespace Edition.Application.Features.Users.Queries;

public class UserNameCheckValidator : AbstractValidator<UserNameCheckRequest>
{
    public UserNameCheckValidator()
    {
        RuleFor(x => x.UserName)
               .Must(x => x.IsEmail() || x.IsMobile())
               .WithMessage(MessageKeys.EnterMobileOrEmail);
    }
}
