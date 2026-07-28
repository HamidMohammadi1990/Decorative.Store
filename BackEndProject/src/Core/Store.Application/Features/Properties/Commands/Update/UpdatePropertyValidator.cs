using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Properties.Commands;

public class UpdatePropertyValidator : AbstractValidator<UpdatePropertyRequest>
{
    public UpdatePropertyValidator(IPropertyRepository propertyRepository)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidId)
            .MustAsync(async (id, cancellationToken) =>
                await propertyRepository.FindAsync(id, cancellationToken) is not null)
            .WithMessage(MessageKeys.InvalidId);

        RuleFor(x => x.Title)
            .NotNull()
            .WithMessage(MessageKeys.TitleRequired);

        RuleFor(x => x.Title)
            .MustAsync(async (request, title, _) =>
                !await propertyRepository.AnyAsync(c =>
                    c.Id != request.Id && c.Title == title.Trim()))
            .WithMessage(MessageKeys.DuplicateTitle)
            .When(x => !string.IsNullOrWhiteSpace(x.Title));
    }
}
