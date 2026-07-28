using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductFeatureTypes.Commands;

public class CreateProductFeatureTypeValidator : AbstractValidator<CreateProductFeatureTypeRequest>
{
    public CreateProductFeatureTypeValidator(IProductFeatureTypeRepository productFeatureTypeRepository)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired)
            .MustAsync(async (name, cancellationToken)
                => !await productFeatureTypeRepository.AnyAsync(x => x.Name == name.Trim()))
            .WithMessage(MessageKeys.DuplicateTitle);
    }
}
