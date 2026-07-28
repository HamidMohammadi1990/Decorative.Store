using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Categories.Commands;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryValidator(ICategoryRepository categoryRepository)
    {
        RuleFor(x => new { x.Id, x.Title, x.Code })
            .NotNull()
            .WithMessage(MessageKeys.TitleRequired)
            .MustAsync(async (x, CancellationToken)
                   => !await categoryRepository.AnyAsync(c => c.Id != x.Id && (c.Title == x.Title.Trim() || c.Code == x.Code.Trim())))
            .WithMessage(MessageKeys.DuplicateTitle);
    }
}
