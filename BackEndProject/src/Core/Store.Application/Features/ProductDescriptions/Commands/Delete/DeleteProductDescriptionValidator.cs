using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductDescriptions.Commands;

public class DeleteProductDescriptionValidator : AbstractValidator<DeleteProductDescriptionRequest>
{
    public DeleteProductDescriptionValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
