using FluentValidation;

namespace Edition.Application.Features.Catalog.Queries;

public class GetNewestCatalogProductsValidator : AbstractValidator<GetNewestCatalogProductsRequest>
{
    public GetNewestCatalogProductsValidator()
    {
        RuleFor(x => x.Limit)
            .InclusiveBetween(1, 12);
    }
}
