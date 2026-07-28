using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PageSections.Commands;

public class UpdatePageSectionValidator : AbstractValidator<UpdatePageSectionRequest>
{
    public UpdatePageSectionValidator(IPageSectionRepository repository)
    {
        RuleFor(x => new { x.Id, x.PageId, x.SectionId })
            .MustAsync(async (x, cancellationToken)
                => !await repository.AnyAsync(r => r.Id != x.Id && r.PageId == x.PageId && r.SectionId == x.SectionId))
            .WithMessage(MessageKeys.DuplicateRecord);
    }
}
