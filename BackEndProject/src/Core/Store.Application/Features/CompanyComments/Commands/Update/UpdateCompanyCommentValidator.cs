using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.CompanyComments.Commands;

public class UpdateCompanyCommentValidator : AbstractValidator<UpdateCompanyCommentRequest>
{
    public UpdateCompanyCommentValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(50)
            .WithMessage(MessageKeys.MaxLength50Characters)
            .MinimumLength(5)
            .WithMessage(MessageKeys.MinLength5Characters);

        RuleFor(x => x.Description)
            .MaximumLength(250)
            .WithMessage(MessageKeys.MaxLength250Characters)
            .MinimumLength(10)
            .WithMessage(MessageKeys.MinLength10Characters);

        RuleFor(x => x.Rate)
            .Must(x => x >= 0 && x <= 5)
            .WithMessage(MessageKeys.RatingRange0To5);
    }
}
