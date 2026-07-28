using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Cities.Queries;

public class GetCityValidator : AbstractValidator<GetCityRequest>
{
    public GetCityValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
