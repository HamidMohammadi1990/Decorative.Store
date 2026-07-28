using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.SubCategories.Commands;

public class DeleteSubCategoryValidator : AbstractValidator<DeleteSubCategoryRequest>
{
    public DeleteSubCategoryValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
