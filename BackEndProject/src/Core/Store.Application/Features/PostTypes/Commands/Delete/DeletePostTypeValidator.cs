using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.PostTypes.Commands;

public class DeletePostTypeValidator : AbstractValidator<DeletePostTypeRequest>
{
    public DeletePostTypeValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
