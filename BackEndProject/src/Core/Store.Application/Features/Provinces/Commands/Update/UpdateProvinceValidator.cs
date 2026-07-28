using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Provinces.Commands;

public class UpdateProvinceValidator : AbstractValidator<UpdateProvinceRequest>
{
    public UpdateProvinceValidator(IProvinceRepository provinceRepository)
    {
        RuleFor(x => new { x.Id, x.Name, x.Slug })
             .MustAsync(async (x, CancellationToken)
                    => !await provinceRepository.AnyAsync(c => c.Id != x.Id && c.Name == x.Name.Trim() || x.Slug == x.Slug.Trim()))
             .WithMessage(MessageKeys.DuplicateTitleOrAddress)
             .When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.Name)
            .MaximumLength(25)
            .WithMessage(MessageKeys.MaxLength25Characters);

        RuleFor(x => x.Name)
            .MinimumLength(5)
            .WithMessage(MessageKeys.MinLength5Characters);

        RuleFor(x => x.Slug)
            .MaximumLength(30)
            .WithMessage(MessageKeys.MaxLength30Characters);

        RuleFor(x => x.TelPrefix)
            .MaximumLength(6)
            .WithMessage(MessageKeys.MaxLength6Characters);

        RuleFor(x => x.Description)
            .MaximumLength(200)
            .WithMessage(MessageKeys.MaxLength200Characters);
    }
}
