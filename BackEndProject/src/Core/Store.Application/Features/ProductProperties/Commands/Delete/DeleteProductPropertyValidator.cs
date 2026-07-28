using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductProperties.Commands;

public class DeleteProductPropertyValidator : AbstractValidator<DeleteProductPropertyRequest>
{
    public DeleteProductPropertyValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
