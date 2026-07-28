using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Products.Queries;

public class GetAllProductValidator : AbstractValidator<GetAllProductRequest>
{
    public GetAllProductValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.CategoryId).MustBeValidOptionalEntityId();
        RuleFor(x => x.SubCategoryId).MustBeValidOptionalEntityId();
        RuleFor(x => x.CategorySlug).MaximumLengthWhenNotEmpty(EntityFieldLengths.Category.Slug);
        RuleFor(x => x.SubCategorySlug).MaximumLengthWhenNotEmpty(EntityFieldLengths.SubCategory.Slug);
        RuleFor(x => x.Slug).MaximumLengthWhenNotEmpty(EntityFieldLengths.Product.Slug);
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.Product.Title);
        RuleFor(x => x.ProductCode).MaximumLengthWhenNotEmpty(EntityFieldLengths.Product.ProductCode);
    }
}
