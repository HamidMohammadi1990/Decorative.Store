using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Products.Commands;

public class CreateProductValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductValidator(IProductRepository productRepository)
    {
        RuleFor(x => x.Title)
            .NotNull()
            .WithMessage(MessageKeys.TitleRequired);

        RuleFor(x => x.ProductCode)
         .NotNull()
         .WithMessage(MessageKeys.ProductCodeRequired);

        RuleFor(x => new { x.Title, x.ProductCode })
            .MustAsync(async (x, CancellationToken)
                   => !await productRepository
                   .AnyAsync(c => c.ProductCode == x.ProductCode || c.Title == x.Title.Trim()))
                   .WithMessage(MessageKeys.DuplicateProduct)
            .When(x => !string.IsNullOrEmpty(x.Title) && !string.IsNullOrEmpty(x.ProductCode));
    }
}