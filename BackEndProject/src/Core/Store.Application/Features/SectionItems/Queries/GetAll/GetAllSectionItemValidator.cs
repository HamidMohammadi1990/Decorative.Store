using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.SectionItems.Queries;

public class GetAllSectionItemValidator : AbstractValidator<GetAllSectionItemRequest>
{
    public GetAllSectionItemValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.SectionId).MustBeValidOptionalEntityId();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.SectionItem.Title);
    }
}
