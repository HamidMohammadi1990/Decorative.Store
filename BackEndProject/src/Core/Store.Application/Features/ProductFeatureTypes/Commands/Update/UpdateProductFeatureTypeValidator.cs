using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductFeatureTypes.Commands;

public class UpdateProductFeatureTypeValidator : AbstractValidator<UpdateProductFeatureTypeRequest>
{
    public UpdateProductFeatureTypeValidator(IProductFeatureTypeRepository productFeatureTypeRepository)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired);

        RuleFor(x => new { x.Id, x.Name })
            .MustAsync(async (x, cancellationToken)
                => !await productFeatureTypeRepository.AnyAsync(c => c.Id != x.Id && c.Name == x.Name.Trim()))
            .WithMessage(MessageKeys.DuplicateTitle);
    }
}
