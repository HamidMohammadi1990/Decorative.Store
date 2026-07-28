using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PropertyCategories.Commands;

public class UpdatePropertyCategoryValidator : AbstractValidator<UpdatePropertyCategoryRequest>
{
    public UpdatePropertyCategoryValidator(IPropertyCategoryRepository propertyCategoryRepository)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired);

        RuleFor(x => x.Title)
            .MustAsync(async (x, CancellationToken)
                   => !await propertyCategoryRepository
                   .AnyAsync(c => c.Title == x.Trim()))
                   .WithMessage(MessageKeys.DuplicateProduct)
            .When(x => !string.IsNullOrEmpty(x.Title));
    }
}