using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Sections.Queries;

public class GetSectionValidator : AbstractValidator<GetSectionRequest>
{
    public GetSectionValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
