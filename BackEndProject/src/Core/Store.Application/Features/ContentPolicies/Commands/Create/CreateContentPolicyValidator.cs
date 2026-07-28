using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;
using Store.Domain.ContentPolicies;
using Store.Domain.Enums;

namespace Edition.Application.Features.ContentPolicies.Commands;

public class CreateContentPolicyValidator : AbstractValidator<CreateContentPolicyRequest>
{
    public CreateContentPolicyValidator(
        IRoleRepository roleRepository,
        IUserRepository userRepository,
        IContentEntityTypeRegistry entityTypeRegistry)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(MessageKeys.NameRequired);

        ContentPolicyEntityTypeValidationRules.ApplyRequiredEntityTypeRules(
            this,
            x => x.EntityType,
            entityTypeRegistry);

        RuleFor(x => x.QueryAction)
            .IsInEnum()
            .WithMessage(MessageKeys.InvalidRequest);

        ContentPolicyScopeValidationRules.ApplyScopeRules(
            this,
            x => x.RoleId,
            x => x.UserId,
            roleRepository,
            userRepository);

        RuleFor(x => x.MergeMode)
            .Equal(ContentPolicyMergeMode.Additive)
            .When(x => x.RoleId is > 0)
            .WithMessage(MessageKeys.ContentPolicyMergeModeInvalidForRole);
    }
}
