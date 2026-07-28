using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.PostTypes.Queries;

public class GetPostTypeValidator : AbstractValidator<GetPostTypeRequest>
{
    public GetPostTypeValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
