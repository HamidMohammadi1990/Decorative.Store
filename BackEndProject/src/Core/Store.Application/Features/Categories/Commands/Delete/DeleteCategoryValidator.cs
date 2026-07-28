using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Categories.Commands;

public class DeleteCategoryValidator : AbstractValidator<DeleteCategoryRequest>
{
    public DeleteCategoryValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
