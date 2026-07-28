using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPropertyRules.Commands;

public class CreateProductPropertyRuleValidator : AbstractValidator<CreateProductPropertyRuleRequest>
{
    public CreateProductPropertyRuleValidator(IProductPropertyRuleRepository productPropertyRuleRepository)
    {
        RuleFor(x => x)
            .MustAsync(async (x, cancellationToken)
                => !await productPropertyRuleRepository.AnyAsync(c => c.ProductPropertyId == x.ProductPropertyId))
            .WithMessage(MessageKeys.ProductPropertyRuleAlreadyExists);
    }
}
