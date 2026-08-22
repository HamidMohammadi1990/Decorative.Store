using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.ProductWishlists.Commands;

public class CreateProductWishlistValidator : AbstractValidator<CreateProductWishlistRequest>
{
    public CreateProductWishlistValidator()
    {
        RuleFor(x => x.Slug)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired);
    }
}
