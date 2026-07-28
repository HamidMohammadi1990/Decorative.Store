using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.SectionItems.Queries;

public class GetSectionItemValidator : AbstractValidator<GetSectionItemRequest>
{
    public GetSectionItemValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
