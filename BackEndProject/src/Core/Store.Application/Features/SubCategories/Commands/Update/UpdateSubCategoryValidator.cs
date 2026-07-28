using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SubCategories.Commands;

public class UpdateSubCategoryValidator : AbstractValidator<UpdateSubCategoryRequest>
{
    public UpdateSubCategoryValidator(ISubCategoryRepository subCategoryRepository)
    {
        RuleFor(x => new { x.Id, x.Title, x.Code })
        .NotNull()
        .WithMessage(MessageKeys.TitleRequired)
        .MustAsync(async (x, CancellationToken)
               => !await subCategoryRepository.AnyAsync(c => c.Id != x.Id && (c.Title == x.Title.Trim() || c.Code == x.Code.Trim())))
        .WithMessage(MessageKeys.DuplicateTitle)
        .When(x => !string.IsNullOrEmpty(x.Title));
    }
}
