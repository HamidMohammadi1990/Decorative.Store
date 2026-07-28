using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductFeatureTypes.Commands;

public class DeleteProductFeatureTypeValidator : AbstractValidator<DeleteProductFeatureTypeRequest>
{
    public DeleteProductFeatureTypeValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
