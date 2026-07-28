using FluentValidation;
using Edition.Application.Common.Validation;
using Store.Common.Localization;

namespace Edition.Application.Features.ProductDescriptions.Commands;

public class UpdateProductDescriptionValidator : AbstractValidator<UpdateProductDescriptionRequest>
{
    public UpdateProductDescriptionValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
        RuleFor(x => x.ProductId).MustBeValidEntityId();

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(MessageKeys.DescriptionRequired)
            .MaximumLength(EntityFieldLengths.ProductDescription.Description)
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
