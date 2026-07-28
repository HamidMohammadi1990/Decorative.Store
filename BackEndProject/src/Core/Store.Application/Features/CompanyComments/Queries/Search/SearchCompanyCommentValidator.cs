using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.CompanyComments.Queries;

public class SearchCompanyCommentValidator : AbstractValidator<SearchCompanyCommentRequest>
{
    public SearchCompanyCommentValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.CompanyId).MustBeValidOptionalEntityId();
    }
}
