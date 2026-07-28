using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.PageSections.Commands;

public class DeletePageSectionValidator : AbstractValidator<DeletePageSectionRequest>
{
    public DeletePageSectionValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
