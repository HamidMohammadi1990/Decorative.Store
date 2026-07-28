using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PostTypes.Commands;

public class CreatePostTypeValidator : AbstractValidator<CreatePostTypeRequest>
{
    public CreatePostTypeValidator(IPostTypeRepository postTypeRepository)
    {
        RuleFor(x => x.Title)            
            .NotNull()
            .WithMessage(MessageKeys.TitleRequired)
            .MustAsync(async (x, CancellationToken)
                   => !await postTypeRepository.AnyAsync(c => c.Title == x.Trim()))
            .WithMessage(MessageKeys.DuplicateTitle);
    }
}