using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Pages.Queries;

public class GetPageTypeGuidesValidator : AbstractValidator<GetPageTypeGuidesRequest>
{
    public GetPageTypeGuidesValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
