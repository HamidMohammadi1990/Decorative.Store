using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Categories.Queries;

public class GetCategoriesWithProductsValidator : AbstractValidator<GetCategoriesWithProductsRequest>
{
    public GetCategoriesWithProductsValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
