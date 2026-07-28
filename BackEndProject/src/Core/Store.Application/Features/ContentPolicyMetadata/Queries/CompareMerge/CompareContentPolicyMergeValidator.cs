using FluentValidation;
using Store.Domain.Enums;
using Edition.Application.Features.ContentPolicies.Commands;
using Store.Common.Localization;
using Store.Domain.ContentPolicies;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ContentPolicyMetadata.Queries;

public class CompareContentPolicyMergeValidator : AbstractValidator<CompareContentPolicyMergeRequest>
{
    public CompareContentPolicyMergeValidator(
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

        When(x => x.DraftUserPolicy is not null, () =>
        {
            RuleFor(x => x.DraftUserPolicy!.Name)
                .NotEmpty()
                .WithMessage(MessageKeys.NameRequired);

            RuleForEach(x => x.DraftUserPolicy!.Rules)
                .ChildRules(rule =>
                {
                    rule.RuleFor(r => r.FieldPath)
                        .NotEmpty()
                        .WithMessage(MessageKeys.InvalidRequest);
                });
        });
    }
}
