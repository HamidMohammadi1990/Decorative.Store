using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.SectionTypes.Commands;

public class DeleteSectionTypeValidator : AbstractValidator<DeleteSectionTypeRequest>
{
    public DeleteSectionTypeValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
