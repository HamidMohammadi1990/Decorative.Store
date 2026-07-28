using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;
using Store.Domain.ContentPolicies;
using Store.Domain.Enums;

namespace Edition.Application.Features.ContentPolicies.Commands;

public class UpdateContentPolicyValidator : AbstractValidator<UpdateContentPolicyRequest>
{
    public UpdateContentPolicyValidator(
        IRoleRepository roleRepository,
        IUserRepository userRepository,
        IContentPolicyRepository contentPolicyRepository,
        IContentEntityTypeRegistry entityTypeRegistry)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidId);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(MessageKeys.NameRequired);

        RuleFor(x => x.Id)
            .MustAsync(async (id, _) => await contentPolicyRepository.FindAsync(id) is not null)
            .WithMessage(MessageKeys.InvalidId);

        ContentPolicyEntityTypeValidationRules.ApplyRequiredEntityTypeRules(
            this,
            x => x.EntityType,
            entityTypeRegistry);

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
