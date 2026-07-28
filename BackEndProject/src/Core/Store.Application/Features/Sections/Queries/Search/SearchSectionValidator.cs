using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Sections.Queries;

public class SearchSectionValidator : AbstractValidator<SearchSectionRequest>
{
    public SearchSectionValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.SectionTypeId).MustBeValidOptionalEntityId();
        RuleFor(x => x.ParentId).MustBeValidOptionalEntityId();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.Section.Title);
    }
}
