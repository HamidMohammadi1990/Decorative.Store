using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPropertyRules.Commands;

public class UpdateProductPropertyRuleValidator : AbstractValidator<UpdateProductPropertyRuleRequest>
{
    public UpdateProductPropertyRuleValidator(IProductPropertyRuleRepository productPropertyRuleRepository)
    {
        RuleFor(x => new { x.Id, x.ProductPropertyId })
            .MustAsync(async (x, cancellationToken)
                => !await productPropertyRuleRepository.AnyAsync(c => c.Id != x.Id && c.ProductPropertyId == x.ProductPropertyId))
            .WithMessage(MessageKeys.ProductPropertyRuleAlreadyExists);
    }
}
