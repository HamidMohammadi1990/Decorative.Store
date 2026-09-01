using Edition.Application.Common.Validation;
using FluentValidation;

namespace Edition.Application.Features.NewsletterSubscribers;

public class GetAllNewsletterSubscriberValidator : AbstractValidator<Queries.GetAllNewsletterSubscriberRequest>
{
    public GetAllNewsletterSubscriberValidator()
    {
        RuleFor(x => x.Email).MaximumLengthWhenNotEmpty(EntityFieldLengths.NewsletterSubscriber.Email);
        RuleFor(x => x.Pagination).NotNull();
    }
}
