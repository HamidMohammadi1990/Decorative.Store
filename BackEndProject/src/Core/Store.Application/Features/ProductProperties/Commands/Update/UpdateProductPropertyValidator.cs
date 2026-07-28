using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductProperties.Commands;

public class UpdateProductPropertyValidator : AbstractValidator<UpdateProductPropertyRequest>
{
    public UpdateProductPropertyValidator(IProductPropertyRepository productPropertyRepository)
    {
        RuleFor(x => new { x.Id, x.ProductId, x.PropertyId })
            .MustAsync(async (x, cancellationToken)
                => !await productPropertyRepository.AnyAsync(c => c.Id != x.Id && c.ProductId == x.ProductId && c.PropertyId == x.PropertyId))
            .WithMessage(MessageKeys.DuplicateProductProperty);
    }
}
