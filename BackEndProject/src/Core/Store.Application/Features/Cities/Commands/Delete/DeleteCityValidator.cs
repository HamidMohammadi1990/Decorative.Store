using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Cities.Commands;

public class DeleteCityValidator : AbstractValidator<DeleteCityRequest>
{
    public DeleteCityValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
