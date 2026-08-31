using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductProperties.Commands;

public class CreateProductPropertyValidator : AbstractValidator<CreateProductPropertyRequest>
{
    public CreateProductPropertyValidator(
        IProductPropertyRepository productPropertyRepository,
        IProductRepository productRepository,
        IPropertyRepository propertyRepository,
        IPropertyItemRepository propertyItemRepository)
    {
        RuleFor(x => x.ProductId)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidProductId)
            .MustAsync(async (productId, cancellationToken) =>
                await productRepository.FindAsync(productId, cancellationToken) is not null)
            .WithMessage("InvalidReference");

        RuleFor(x => x.PropertyId)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidRequest)
            .MustAsync(async (propertyId, cancellationToken) =>
                await propertyRepository.FindAsync(propertyId, cancellationToken) is not null)
            .WithMessage("InvalidReference");

        RuleFor(x => new { x.ProductId, x.PropertyId })
            .MustAsync(async (x, cancellationToken) =>
                !await productPropertyRepository.AnyAsync(c =>
                    c.ProductId == x.ProductId && c.PropertyId == x.PropertyId))
            .WithMessage(MessageKeys.DuplicateProductProperty);

        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
            {
                var propertyItemId = ProductPropertyReferenceRules.NormalizePropertyItemId(request.PropertyItemId);
                if (!propertyItemId.HasValue)
                    return true;

                var propertyItem = await propertyItemRepository.FindAsync(propertyItemId.Value, cancellationToken);
                return propertyItem is not null && propertyItem.PropertyId == request.PropertyId;
            })
            .WithMessage("InvalidReference");
    }
}
