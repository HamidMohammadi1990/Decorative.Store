using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductProperties.Queries;

public class GetProductPropertyValidator : AbstractValidator<GetProductPropertyRequest>
{
    public GetProductPropertyValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
