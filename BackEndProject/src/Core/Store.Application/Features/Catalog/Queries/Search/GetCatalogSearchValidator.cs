using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Catalog.Queries;

public class GetCatalogSearchValidator : AbstractValidator<GetCatalogSearchRequest>
{
    public GetCatalogSearchValidator()
    {
        RuleFor(x => x.Query)
            .NotEmpty()
            .WithMessage(MessageKeys.InvalidRequest)
            .MaximumLength(100);

        RuleFor(x => x.Limit)
            .InclusiveBetween(1, 50);
    }
}
