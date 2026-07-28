using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Banks.Commands;

public class DeleteBankValidator : AbstractValidator<DeleteBankRequest>
{
    public DeleteBankValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
