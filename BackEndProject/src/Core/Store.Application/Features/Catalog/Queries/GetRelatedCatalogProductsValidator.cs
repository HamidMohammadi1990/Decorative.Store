using FluentValidation;

namespace Edition.Application.Features.Catalog.Queries;

public class GetRelatedCatalogProductsValidator : AbstractValidator<GetRelatedCatalogProductsRequest>
{
    public GetRelatedCatalogProductsValidator()
    {
        RuleFor(x => x.Slug)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Limit)
            .InclusiveBetween(1, 12);
    }
}
