using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Categories.Queries;

public class GetCategoryValidator : AbstractValidator<GetCategoryRequest>
{
    public GetCategoryValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
