using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.PropertyItems.Commands;

public class DeletePropertyItemValidator : AbstractValidator<DeletePropertyItemRequest>
{
    public DeletePropertyItemValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
