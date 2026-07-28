using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.PropertyCategories.Queries;

public class GetPropertyCategoryValidator : AbstractValidator<GetPropertyCategoryRequest>
{
    public GetPropertyCategoryValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
