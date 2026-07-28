using FluentValidation;
using Edition.Application.Common.Validation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PropertyItems.Commands;

public class UpdatePropertyItemValidator : AbstractValidator<UpdatePropertyItemRequest>
{
    public UpdatePropertyItemValidator(IPropertyItemRepository propertyItemRepository)
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
        RuleFor(x => x.PropertyId).MustBeValidEntityId();

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired)
            .MaximumLength(EntityFieldLengths.PropertyItem.Title)
            .WithMessage(MessageKeys.MaxLength30Characters);

        RuleFor(x => new { x.Id, x.Title })
            .MustAsync(async (x, _) =>
                !await propertyItemRepository.AnyAsync(p => p.Id != x.Id && p.Title == x.Title.Trim()))
            .When(x => !string.IsNullOrWhiteSpace(x.Title))
            .WithMessage(MessageKeys.DuplicateTitle);
    }
}
