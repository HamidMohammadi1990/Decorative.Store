using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.PropertyCategories.Queries;

public class GetAllPropertyCategoryValidator : AbstractValidator<GetAllPropertyCategoryRequest>
{
    public GetAllPropertyCategoryValidator()
    {
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.PropertyCategory.Title);
    }
}
