using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Tags.Queries;

public class GetTagValidator : AbstractValidator<GetTagRequest>
{
    public GetTagValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
