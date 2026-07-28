using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.ProductDescriptions.Commands;

public class CreateProductDescriptionValidator : AbstractValidator<CreateProductDescriptionRequest>
{
    public CreateProductDescriptionValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidProductId);

        RuleFor(x => x.Description)
            .NotNull()
            .WithMessage(MessageKeys.DescriptionRequired);
    }
}