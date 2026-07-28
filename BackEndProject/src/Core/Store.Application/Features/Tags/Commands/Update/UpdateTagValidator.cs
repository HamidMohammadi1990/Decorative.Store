using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Tags.Commands;

public class UpdateTagValidator : AbstractValidator<UpdateTagRequest>
{
    public UpdateTagValidator(ITagRepository tagRepository)
    {
        RuleFor(x => new { x.Id, x.Title })
        .NotNull()
        .WithMessage(MessageKeys.TitleRequired)
        .MustAsync(async (x, CancellationToken)
               => !await tagRepository.AnyAsync(c => c.Id != x.Id && c.Title == x.Title.Trim()))
        .WithMessage(MessageKeys.DuplicateTitle);
    }
}
