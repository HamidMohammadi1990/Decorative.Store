using FluentValidation;
using Edition.Application.Common.Validation;
using Edition.Application.Features.AssistantFaqs.Queries;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.AssistantFaqs.Commands;

public class CreateAssistantFaqValidator : AbstractValidator<CreateAssistantFaqRequest>
{
    public CreateAssistantFaqValidator(
        IAssistantFaqRepository repository,
        ILanguageRepository languageRepository)
    {
        RuleFor(x => x.LanguageId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidRequest)
            .MustAsync(async (languageId, cancellationToken) =>
                await languageRepository.FindAsync(languageId, cancellationToken) is { IsActive: true })
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Question)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired)
            .MaximumLength(200)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Answer)
            .NotEmpty()
            .WithMessage(MessageKeys.DescriptionRequired)
            .MaximumLength(2000)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(0)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
                !await repository.AnyAsync(
                    x => x.LanguageId == request.LanguageId &&
                         x.Question == request.Question.Trim(),
                    cancellationToken))
            .WithMessage(MessageKeys.DuplicateTitle);
    }
}

public class UpdateAssistantFaqValidator : AbstractValidator<UpdateAssistantFaqRequest>
{
    public UpdateAssistantFaqValidator(
        IAssistantFaqRepository repository,
        ILanguageRepository languageRepository)
    {
        RuleFor(x => x.Id).MustBeValidEntityId();

        RuleFor(x => x.LanguageId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidRequest)
            .MustAsync(async (languageId, cancellationToken) =>
                await languageRepository.FindAsync(languageId, cancellationToken) is { IsActive: true })
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Question)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired)
            .MaximumLength(200)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Answer)
            .NotEmpty()
            .WithMessage(MessageKeys.DescriptionRequired)
            .MaximumLength(2000)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(0)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
                !await repository.AnyAsync(
                    x => x.Id != request.Id &&
                         x.LanguageId == request.LanguageId &&
                         x.Question == request.Question.Trim(),
                    cancellationToken))
            .WithMessage(MessageKeys.DuplicateTitle);
    }
}

public class DeleteAssistantFaqValidator : AbstractValidator<DeleteAssistantFaqRequest>
{
    public DeleteAssistantFaqValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}

public class GetAllAssistantFaqValidator : AbstractValidator<GetAllAssistantFaqRequest>
{
    public GetAllAssistantFaqValidator()
    {
        RuleFor(x => x.Pagination).NotNull();
    }
}

public class GetAssistantFaqValidator : AbstractValidator<GetAssistantFaqRequest>
{
    public GetAssistantFaqValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}

public class SearchAssistantFaqValidator : AbstractValidator<SearchAssistantFaqRequest>
{
    public SearchAssistantFaqValidator()
    {
        RuleFor(x => x.LanguageId).GreaterThan(0).WithMessage(MessageKeys.InvalidRequest);
        RuleFor(x => x.Pagination).NotNull();
    }
}
