using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPosts.Commands;

public class CreateBlogPostValidator : AbstractValidator<CreateBlogPostRequest>
{
    public CreateBlogPostValidator(IBlogPostRepository blogPostRepository, ILanguageRepository languageRepository)
    {
        RuleFor(x => x.LanguageId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidRequest)
            .MustAsync(async (languageId, cancellationToken) =>
                await languageRepository.FindAsync(languageId, cancellationToken) is { IsActive: true })
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired);

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
                !await blogPostRepository.ExistsCodeAsync(request.Code, cancellationToken: cancellationToken))
            .WithMessage(MessageKeys.DuplicateTitle);

        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
                !await blogPostRepository.ExistsTranslationAsync(
                    request.LanguageId,
                    request.Title,
                    request.Slug,
                    cancellationToken: cancellationToken))
            .WithMessage(MessageKeys.DuplicateTitle);
    }
}
