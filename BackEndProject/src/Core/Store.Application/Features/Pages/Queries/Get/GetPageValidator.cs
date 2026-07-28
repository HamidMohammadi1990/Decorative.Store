using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Pages.Queries;

public class GetPageValidator : AbstractValidator<GetPageRequest>
{
    public GetPageValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
