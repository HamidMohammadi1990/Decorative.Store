using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.SectionTypes.Queries;

public class GetSectionTypeValidator : AbstractValidator<GetSectionTypeRequest>
{
    public GetSectionTypeValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
