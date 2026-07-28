using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.CompanyComments.Queries;

public class GetCompanyCommentValidator : AbstractValidator<GetCompanyCommentRequest>
{
    public GetCompanyCommentValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
