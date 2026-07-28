using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Banks.Commands;

public class CreateBankValidator : AbstractValidator<CreateBankRequest>
{
    public CreateBankValidator(IBankRepository bankRepository)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired)
            .MustAsync(async (title, cancellationToken)
                => !await bankRepository.AnyAsync(x => x.Title == title.Trim()))
            .WithMessage(MessageKeys.DuplicateTitle);

        RuleFor(x => x.Icon)
            .NotEmpty()
            .WithMessage(MessageKeys.IconRequired);
    }
}
