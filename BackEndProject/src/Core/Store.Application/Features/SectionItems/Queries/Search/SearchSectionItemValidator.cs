using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.SectionItems.Queries;

public class SearchSectionItemValidator : AbstractValidator<SearchSectionItemRequest>
{
    public SearchSectionItemValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.SectionId).MustBeValidOptionalEntityId();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.SectionItem.Title);
    }
}
