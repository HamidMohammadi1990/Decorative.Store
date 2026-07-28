using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.UserSessions.Queries;

public class GetActiveUserSessionsValidator : AbstractValidator<GetActiveUserSessionsRequest>
{
    public GetActiveUserSessionsValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
