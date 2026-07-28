using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductPropertyRules.Commands;

public class DeleteProductPropertyRuleValidator : AbstractValidator<DeleteProductPropertyRuleRequest>
{
    public DeleteProductPropertyRuleValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
