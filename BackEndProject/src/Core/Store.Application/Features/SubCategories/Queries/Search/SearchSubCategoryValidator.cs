using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.SubCategories.Queries;

public class SearchSubCategoryValidator : AbstractValidator<SearchSubCategoryRequest>
{
    public SearchSubCategoryValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.CategoryId).MustBeValidOptionalEntityId();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.SubCategory.Title);
        RuleFor(x => x.Slug).MaximumLengthWhenNotEmpty(EntityFieldLengths.SubCategory.Slug);
        RuleFor(x => x.Code).MaximumLengthWhenNotEmpty(EntityFieldLengths.SubCategory.Code);
        RuleFor(x => x.CategoryTitle).MaximumLengthWhenNotEmpty(EntityFieldLengths.Category.Title);
        RuleFor(x => x.CategoryCode).MaximumLengthWhenNotEmpty(EntityFieldLengths.Category.Code);
    }
}
