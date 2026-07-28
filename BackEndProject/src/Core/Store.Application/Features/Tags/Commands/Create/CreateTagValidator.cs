using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Tags.Commands;

public class CreateTagValidator : AbstractValidator<CreateTagRequest>
{
    public CreateTagValidator(ITagRepository tagRepository)
    {
        RuleFor(x => x.Title)
         .NotNull()
         .WithMessage(MessageKeys.TitleRequired)
         .MustAsync(async (x, CancellationToken)
                => !await tagRepository.AnyAsync(c => c.Title == x.Trim()))
         .WithMessage(MessageKeys.DuplicateTitle);
    }
}
