using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductProperties.Queries;

public class GetAllProductPropertyValidator : AbstractValidator<GetAllProductPropertyRequest>
{
    public GetAllProductPropertyValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.ProductId).MustBeValidOptionalEntityId();
        RuleFor(x => x.PropertyId).MustBeValidOptionalEntityId();
    }
}
