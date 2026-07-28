using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Companies.Queries;

public class GetCompanyValidator : AbstractValidator<GetCompanyRequest>
{
    public GetCompanyValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
