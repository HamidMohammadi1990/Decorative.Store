using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductDescriptions.Queries;

public class GetAllProductDescriptionValidator : AbstractValidator<GetAllProductDescriptionRequest>
{
    public GetAllProductDescriptionValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.ProductId).MustBeValidEntityId();
    }
}
