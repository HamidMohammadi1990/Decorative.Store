using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.SectionItems.Commands;

public class DeleteSectionItemValidator : AbstractValidator<DeleteSectionItemRequest>
{
    public DeleteSectionItemValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
