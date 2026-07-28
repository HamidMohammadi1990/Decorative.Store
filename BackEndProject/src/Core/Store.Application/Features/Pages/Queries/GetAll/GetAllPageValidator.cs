using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Pages.Queries;

public class GetAllPageValidator : AbstractValidator<GetAllPageRequest>
{
    public GetAllPageValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.Page.Title);
        RuleFor(x => x.Slug).MaximumLengthWhenNotEmpty(EntityFieldLengths.Page.Slug);
    }
}
