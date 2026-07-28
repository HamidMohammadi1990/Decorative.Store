using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.SectionTypes.Queries;

public class GetAllSectionTypeValidator : AbstractValidator<GetAllSectionTypeRequest>
{
    public GetAllSectionTypeValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.Name).MaximumLengthWhenNotEmpty(EntityFieldLengths.SectionType.Title);
    }
}
