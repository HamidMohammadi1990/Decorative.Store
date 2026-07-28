using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Products.Commands;

public class UpdateProductValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductValidator(IProductRepository productRepository)
    {
        RuleFor(u => u.Id)
           .NotEqual(0)
           .WithMessage(MessageKeys.InvalidId);

        RuleFor(x => x.Title)            
            .NotNull()
            .WithMessage(MessageKeys.TitleRequired)
            .MaximumLength(50)
            .WithMessage(MessageKeys.TitleMaxLength50)
            .MinimumLength(10)
            .WithMessage(MessageKeys.TitleMinLength10);            

        RuleFor(x => x.Status)
            .NotNull()
            .WithMessage(MessageKeys.StatusTypeRequired);

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