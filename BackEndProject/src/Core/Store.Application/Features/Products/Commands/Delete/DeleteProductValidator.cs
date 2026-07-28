using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Products.Commands;

public class DeleteProductValidator : AbstractValidator<DeleteProductRequest>
{
    public DeleteProductValidator()
    {
        RuleFor(u => u.Id)
        .NotEqual(0)
        .WithMessage(MessageKeys.InvalidId);
    }
}