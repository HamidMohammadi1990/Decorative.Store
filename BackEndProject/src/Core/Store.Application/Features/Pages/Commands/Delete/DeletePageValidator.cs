using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Pages.Commands;

public class DeletePageValidator : AbstractValidator<DeletePageRequest>
{
    public DeletePageValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
