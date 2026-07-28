using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Sections.Queries;

public class GetAllSectionValidator : AbstractValidator<GetAllSectionRequest>
{
    public GetAllSectionValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.SectionTypeId).MustBeValidOptionalEntityId();
        RuleFor(x => x.ParentId).MustBeValidOptionalEntityId();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.Section.Title);
    }
}
