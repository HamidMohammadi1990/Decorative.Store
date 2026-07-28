using FluentValidation;
using Edition.Application.Common.Validation;
using Edition.Application.Features.ContentPolicies.Commands;
using Store.Common.Localization;
using Store.Domain.ContentPolicies;

namespace Edition.Application.Features.ContentPolicies.Queries;

public class GetAllContentPolicyValidator : AbstractValidator<GetAllContentPolicyRequest>
{
    public GetAllContentPolicyValidator(IContentEntityTypeRegistry entityTypeRegistry)
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.RoleId).MustBeValidOptionalEntityId();
        RuleFor(x => x.UserId).MustBeValidOptionalEntityId();
        RuleFor(x => x.Name).MaximumLengthWhenNotEmpty(EntityFieldLengths.ContentPolicy.Name);

        ContentPolicyEntityTypeValidationRules.ApplyOptionalEntityTypeRules(
            this,
            x => x.EntityType,
            entityTypeRegistry);

        RuleFor(x => x.QueryAction)
            .IsInEnum()
            .When(x => x.QueryAction.HasValue)
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
