using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Banks.Commands;

public class UpdateBankValidator : AbstractValidator<UpdateBankRequest>
{
    public UpdateBankValidator(IBankRepository bankRepository)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired);

        RuleFor(x => new { x.Id, x.Title })
            .MustAsync(async (x, cancellationToken)
                => !await bankRepository.AnyAsync(b => b.Id != x.Id && b.Title == x.Title.Trim()))
            .WithMessage(MessageKeys.DuplicateTitle);

        RuleFor(x => x.Icon)
            .NotEmpty()
            .WithMessage(MessageKeys.IconRequired);
    }
}
