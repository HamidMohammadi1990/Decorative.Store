using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.PageSections.Queries;

public class GetPageSectionValidator : AbstractValidator<GetPageSectionRequest>
{
    public GetPageSectionValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
