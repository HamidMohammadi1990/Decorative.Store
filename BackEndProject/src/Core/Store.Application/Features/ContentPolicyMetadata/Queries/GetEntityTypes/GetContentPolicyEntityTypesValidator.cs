using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.ContentPolicyMetadata.Queries;

public class GetContentPolicyEntityTypesValidator : AbstractValidator<GetContentPolicyEntityTypesRequest>
{
    public GetContentPolicyEntityTypesValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
