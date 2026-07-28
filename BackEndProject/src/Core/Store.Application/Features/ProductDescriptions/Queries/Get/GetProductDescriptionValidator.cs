using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductDescriptions.Queries;

public class GetProductDescriptionValidator : AbstractValidator<GetProductDescriptionRequest>
{
    public GetProductDescriptionValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
