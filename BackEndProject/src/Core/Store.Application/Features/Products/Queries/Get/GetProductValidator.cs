using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Products.Queries;

public class GetProductValidator : AbstractValidator<GetProductRequest>
{
    public GetProductValidator()
    {
        RuleFor(u => u.Id)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidId);
    }
}