using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.SubCategories.Queries;

public class GetSubCategoryValidator : AbstractValidator<GetSubCategoryRequest>
{
    public GetSubCategoryValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
