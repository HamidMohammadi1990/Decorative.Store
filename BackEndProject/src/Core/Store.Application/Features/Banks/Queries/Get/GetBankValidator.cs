using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Banks.Queries;

public class GetBankValidator : AbstractValidator<GetBankRequest>
{
    public GetBankValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
