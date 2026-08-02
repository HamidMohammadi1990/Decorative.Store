using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Catalog.Queries;

public class GetCatalogListingValidator : AbstractValidator<GetCatalogListingRequest>
{
    public GetCatalogListingValidator()
    {
        RuleFor(x => x.Path)
            .NotNull()
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
