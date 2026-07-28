using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Pages.Queries;

public class GetPageBySlugValidator : AbstractValidator<GetPageBySlugRequest>
{
    public GetPageBySlugValidator()
    {
        RuleFor(x => x.Slug)
            .NotEmpty()
            .WithMessage(MessageKeys.SlugRequired);
    }
}
