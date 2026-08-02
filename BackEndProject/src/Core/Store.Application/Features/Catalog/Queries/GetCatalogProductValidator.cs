using FluentValidation;

namespace Edition.Application.Features.Catalog.Queries;

public class GetCatalogProductValidator : AbstractValidator<GetCatalogProductRequest>
{
    public GetCatalogProductValidator()
    {
        RuleFor(x => x.Slug)
            .NotEmpty()
            .MaximumLength(200);
    }
}
