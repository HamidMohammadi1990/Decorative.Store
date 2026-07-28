using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Sections.Commands;

public class CreateSectionValidator : AbstractValidator<CreateSectionRequest>
{
    public CreateSectionValidator(ISectionTypeRepository sectionTypeRepository)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired);

        RuleFor(x => x.Url)
            .NotEmpty()
            .WithMessage(MessageKeys.AddressRequired);

        RuleFor(x => x.SectionTypeId)
            .MustAsync(async (sectionTypeId, cancellationToken)
                => await sectionTypeRepository.AnyAsync(s => s.Id == sectionTypeId))
            .WithMessage(MessageKeys.SectionTypeNotFound);

        RuleFor(x => x)
            .Must(x => !x.StartDateOnUtc.HasValue || !x.EndDateOnUtc.HasValue || x.StartDateOnUtc <= x.EndDateOnUtc)
            .WithMessage(MessageKeys.StartDateMustBeBeforeEndDate);
    }
}
