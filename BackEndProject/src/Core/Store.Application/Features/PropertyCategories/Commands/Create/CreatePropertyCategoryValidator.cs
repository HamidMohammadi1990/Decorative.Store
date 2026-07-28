using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PropertyCategories.Commands;

public class CreatePropertyCategoryValidator : AbstractValidator<CreatePropertyCategoryRequest>
{
    public CreatePropertyCategoryValidator(IPropertyCategoryRepository propertyCategoryRepository)
    {
        RuleFor(x => x.Title)
            .NotNull()
            .WithMessage(MessageKeys.TitleRequired);

        RuleFor(x => x.Title)
            .MustAsync(async (x, CancellationToken)
                   => !await propertyCategoryRepository
                   .AnyAsync(c =>  c.Title == x.Trim()))
                   .WithMessage(MessageKeys.DuplicateProduct)
            .When(x => !string.IsNullOrEmpty(x.Title));
    }
}