using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.CompanyComments.Commands;

public class DeleteCompanyCommentValidator : AbstractValidator<DeleteCompanyCommentRequest>
{
    public DeleteCompanyCommentValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
