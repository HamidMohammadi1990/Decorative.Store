using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.SectionItems.Commands;

public class CreateSectionItemValidator : AbstractValidator<CreateSectionItemRequest>
{
    public CreateSectionItemValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired);
    }
}
