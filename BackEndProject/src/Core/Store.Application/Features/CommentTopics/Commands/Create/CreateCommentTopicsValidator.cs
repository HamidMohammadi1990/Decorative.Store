using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CommentTopics.Commands;

public class CreateCommentTopicsValidator : AbstractValidator<CreateCommentTopicRequest>
{
    public CreateCommentTopicsValidator(ICommentTopicRepository commentTopicRepository)
    {
        RuleFor(x => x.Title)
            .NotNull()
            .WithMessage(MessageKeys.TitleRequired)
            .MustAsync(async (x, CancellationToken)
                   => !await commentTopicRepository.AnyAsync(c => c.Title == x.Trim()))
            .WithMessage(MessageKeys.DuplicateTitle)
            .When(x => !string.IsNullOrWhiteSpace(x.Title));
    }
}
