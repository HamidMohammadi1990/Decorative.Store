using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductProperties.Commands;

public class CreateProductPropertyValidator : AbstractValidator<CreateProductPropertyRequest>
{
    public CreateProductPropertyValidator(IProductPropertyRepository productPropertyRepository)
    {
        RuleFor(x => new { x.ProductId, x.PropertyId })
            .MustAsync(async (x, cancellationToken)
                => !await productPropertyRepository.AnyAsync(c => c.ProductId == x.ProductId && c.PropertyId == x.PropertyId))
            .WithMessage(MessageKeys.DuplicateProductProperty);
    }
}
