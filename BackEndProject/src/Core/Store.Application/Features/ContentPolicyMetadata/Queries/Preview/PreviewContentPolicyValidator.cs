using FluentValidation;
using Edition.Domain.Enums;
using Edition.Application.Features.ContentPolicies.Commands;
using Store.Common.Localization;
using Store.Domain.ContentPolicies;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ContentPolicyMetadata.Queries;

public class PreviewContentPolicyValidator : AbstractValidator<PreviewContentPolicyRequest>
{
    public PreviewContentPolicyValidator(
        IUserRepository userRepository,
        IContentEntityTypeRegistry entityTypeRegistry)
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidUser)
            .MustAsync(async (userId, _) => await userRepository.AnyAsync(x => x.Id == userId))
            .WithMessage(MessageKeys.UserNotFound);

        ContentPolicyEntityTypeValidationRules.ApplyRequiredEntityTypeRules(
            this,
            x => x.EntityType,
            entityTypeRegistry);

        RuleFor(x => x.QueryAction)
            .IsInEnum()
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.SampleSize)
            .InclusiveBetween(1, 50)
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
