using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Sections.Commands;

public class UpdateSectionValidator : AbstractValidator<UpdateSectionRequest>
{
    public UpdateSectionValidator(ISectionRepository sectionRepository)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired);

        RuleFor(x => x.Url)
            .NotEmpty()
            .WithMessage(MessageKeys.AddressRequired);

        RuleFor(x => x)
            .Must(x => !x.ParentId.HasValue || x.ParentId.Value != x.Id)
            .WithMessage(MessageKeys.SectionCannotBeOwnParent);

        RuleFor(x => x)
            .Must(x => !x.StartDateOnUtc.HasValue || !x.EndDateOnUtc.HasValue || x.StartDateOnUtc <= x.EndDateOnUtc)
            .WithMessage(MessageKeys.StartDateMustBeBeforeEndDate);

        When(x => x.ParentId.HasValue, () =>
        {
            RuleFor(x => x.ParentId!.Value)
                .MustAsync(async (parentId, cancellationToken)
                    => await sectionRepository.AnyAsync(s => s.Id == parentId))
                .WithMessage(MessageKeys.ParentSectionNotFound);
        });
    }
}
