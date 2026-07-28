using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Companies.Commands;

public class DeleteCompanyValidator : AbstractValidator<DeleteCompanyRequest>
{
    public DeleteCompanyValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
