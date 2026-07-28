using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SectionTypes.Commands;

public class CreateSectionTypeValidator : AbstractValidator<CreateSectionTypeRequest>
{
    public CreateSectionTypeValidator(ISectionTypeRepository repository)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired)
            .MustAsync(async (name, cancellationToken)
                => !await repository.AnyAsync(x => x.Name == name.Trim()))
            .WithMessage(MessageKeys.DuplicateTitle);
    }
}
