using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.PageSections.Queries;

public class GetAllPageSectionValidator : AbstractValidator<GetAllPageSectionRequest>
{
    public GetAllPageSectionValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.PageId).MustBeValidOptionalEntityId();
        RuleFor(x => x.SectionId).MustBeValidOptionalEntityId();
    }
}
