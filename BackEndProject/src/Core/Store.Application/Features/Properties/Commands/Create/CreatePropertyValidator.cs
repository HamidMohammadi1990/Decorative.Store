using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Properties.Commands;

public class CreatePropertyValidator : AbstractValidator<CreatePropertyRequest>
{
    public CreatePropertyValidator(IPropertyRepository propertyRepository)
    {
        RuleFor(x => x.Title)
            .NotNull()
            .WithMessage(MessageKeys.TitleRequired);

        RuleFor(x => x.Title)
            .MustAsync(async (x, CancellationToken)
                => !await propertyRepository
                .AnyAsync(c => c.Title == x.Trim()))
                .WithMessage(MessageKeys.DuplicateTitle)
                .When(x => !string.IsNullOrEmpty(x.Title));
    }
}