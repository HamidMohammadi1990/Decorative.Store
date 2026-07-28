using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SectionTypes.Commands;

public class UpdateSectionTypeValidator : AbstractValidator<UpdateSectionTypeRequest>
{
    public UpdateSectionTypeValidator(ISectionTypeRepository repository)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired);

        RuleFor(x => new { x.Id, x.Name })
            .MustAsync(async (x, cancellationToken)
                => !await repository.AnyAsync(b => b.Id != x.Id && b.Name == x.Name.Trim()))
            .WithMessage(MessageKeys.DuplicateTitle);
    }
}
