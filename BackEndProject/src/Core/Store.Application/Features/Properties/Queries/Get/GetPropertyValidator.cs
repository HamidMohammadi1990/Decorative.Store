using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Properties.Queries;

public class GetPropertyValidator : AbstractValidator<GetPropertyRequest>
{
    public GetPropertyValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
