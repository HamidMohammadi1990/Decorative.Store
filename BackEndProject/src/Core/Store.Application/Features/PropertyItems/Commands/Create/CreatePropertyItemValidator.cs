using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PropertyItems.Commands;

public class CreatePropertyItemValidator : AbstractValidator<CreatePropertyItemRequest>
{
    public CreatePropertyItemValidator(IPropertyItemRepository propertyItemRepository)
    {
        RuleFor(x => x.Title)
            .NotNull()
            .WithMessage(MessageKeys.TitleRequired);

        RuleFor(x => x.Title)
            .MustAsync(async (x, CancellationToken)
                   => !await propertyItemRepository
                   .AnyAsync(c => c.Title == x.Trim()))
                   .WithMessage(MessageKeys.DuplicateProduct)
            .When(x => !string.IsNullOrEmpty(x.Title));
    }
}
