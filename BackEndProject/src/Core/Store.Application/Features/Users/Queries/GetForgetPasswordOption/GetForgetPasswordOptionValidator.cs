using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Users.Queries;

public class GetForgetPasswordOptionValidator : AbstractValidator<GetForgetPasswordOptionRequest>
{
    public GetForgetPasswordOptionValidator()
    {
        RuleFor(x => x.UserName)
            .NotNull()
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage(MessageKeys.UserNameIsNotValid);
    }
}
