using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.PropertyItems.Queries;

public class GetPropertyItemValidator : AbstractValidator<GetPropertyItemRequest>
{
    public GetPropertyItemValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
