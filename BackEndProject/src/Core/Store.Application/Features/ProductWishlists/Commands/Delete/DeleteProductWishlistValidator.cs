using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.ProductWishlists.Commands;

public class DeleteProductWishlistValidator : AbstractValidator<DeleteProductWishlistRequest>
{
    public DeleteProductWishlistValidator()
    {
        RuleFor(x => x.Slug)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired);
    }
}
