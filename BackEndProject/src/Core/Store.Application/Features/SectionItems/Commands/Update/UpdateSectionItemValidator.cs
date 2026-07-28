using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.SectionItems.Commands;

public class UpdateSectionItemValidator : AbstractValidator<UpdateSectionItemRequest>
{
    public UpdateSectionItemValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired);
    }
}
