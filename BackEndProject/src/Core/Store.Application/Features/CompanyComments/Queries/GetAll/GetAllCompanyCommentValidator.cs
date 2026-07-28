using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.CompanyComments.Queries;

public class GetAllCompanyCommentValidator : AbstractValidator<GetAllCompanyCommentRequest>
{
    public GetAllCompanyCommentValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.CompanyId).MustBeValidOptionalEntityId();
        RuleFor(x => x.UserId).MustBeValidOptionalEntityId();
    }
}
