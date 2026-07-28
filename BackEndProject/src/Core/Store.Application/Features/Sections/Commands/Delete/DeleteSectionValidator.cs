using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Sections.Commands;

public class DeleteSectionValidator : AbstractValidator<DeleteSectionRequest>
{
    public DeleteSectionValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
