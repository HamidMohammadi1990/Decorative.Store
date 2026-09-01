using Edition.Application.Common.Validation;
using FluentValidation;
using Store.Common.Extensions;
using Store.Common.Localization;

namespace Edition.Application.Features.NewsletterSubscribers.Commands;

public class SubscribeNewsletterValidator : AbstractValidator<SubscribeNewsletterRequest>
{
    public SubscribeNewsletterValidator()
    {
        RuleFor(x => x.LanguageId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(MessageKeys.EmailIsNotValid)
            .MaximumLength(EntityFieldLengths.NewsletterSubscriber.Email)
            .WithMessage(MessageKeys.EmailIsNotValid)
            .Must(x => x.Trim().IsEmail())
            .WithMessage(MessageKeys.EmailIsNotValid);
    }
}
