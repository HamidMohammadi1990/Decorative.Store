using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PageSections.Commands;

public class CreatePageSectionValidator : AbstractValidator<CreatePageSectionRequest>
{
    public CreatePageSectionValidator(IPageSectionRepository repository)
    {
        RuleFor(x => new { x.PageId, x.SectionId })
            .MustAsync(async (x, cancellationToken)
                => !await repository.AnyAsync(r => r.PageId == x.PageId && r.SectionId == x.SectionId))
            .WithMessage(MessageKeys.DuplicateRecord);
    }
}
