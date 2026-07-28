using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.PropertyCategories.Commands;

public class DeletePropertyCategoryValidator : AbstractValidator<DeletePropertyCategoryRequest>
{
    public DeletePropertyCategoryValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
