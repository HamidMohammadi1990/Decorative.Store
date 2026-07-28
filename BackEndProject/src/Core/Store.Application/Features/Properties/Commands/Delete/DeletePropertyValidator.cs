using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Properties.Commands;

public class DeletePropertyValidator : AbstractValidator<DeletePropertyRequest>
{
    public DeletePropertyValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
