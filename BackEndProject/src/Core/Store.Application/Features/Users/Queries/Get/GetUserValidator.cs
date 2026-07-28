using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Users.Queries;

public class GetUserValidator : AbstractValidator<GetUserRequest>
{
    public GetUserValidator()
    {
        RuleFor(u => u.Id)
            .NotEmpty()
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidId);
    }
}
